param(
    [string]$LanguageFolder = (Join-Path $PSScriptRoot "..\CapturaLibre\LanguagePacks")
)

$problems = [System.Collections.Generic.List[string]]::new()
$warnings = [System.Collections.Generic.List[string]]::new()
$codes = @{}
$rtlCodes = @("ar", "fa", "he", "ur")
$reference = Get-Content -LiteralPath (Join-Path $LanguageFolder "ca.json") -Raw | ConvertFrom-Json
$requiredKeys = @($reference.texts.PSObject.Properties.Name)
$requiredPatternTokens = @('{fecha}', '{hora}', '{contador}', '{ventana}')
$referenceTokens = @{}
foreach ($property in $reference.texts.PSObject.Properties) {
    $referenceTokens[$property.Name] = @([regex]::Matches([string]$property.Value, '\{[^}]+\}') |
        ForEach-Object { $_.Value })
}

$files = @(Get-ChildItem -LiteralPath $LanguageFolder -Filter "*.json" -File | Sort-Object Name)
foreach ($file in $files) {
    try {
        $document = [System.Text.Json.JsonDocument]::Parse([System.IO.File]::ReadAllText($file.FullName))
        $root = $document.RootElement
        $code = $root.GetProperty("code").GetString()
        $displayName = $root.GetProperty("displayName").GetString()
        $rightToLeft = $root.GetProperty("rightToLeft").GetBoolean()
        $texts = $root.GetProperty("texts")

        if ([string]::IsNullOrWhiteSpace($code)) { $problems.Add("$($file.Name): código vacío") }
        elseif ($codes.ContainsKey($code)) { $problems.Add("Código repetido '$code': $($codes[$code]) y $($file.Name)") }
        else { $codes[$code] = $file.Name }
        if ([string]::IsNullOrWhiteSpace($displayName)) { $problems.Add("$($file.Name): nombre visible vacío") }
        if (($rtlCodes -contains $code) -ne $rightToLeft) {
            $warnings.Add("$($file.Name): revisar rightToLeft para '$code'")
        }

        $seen = @{}
        foreach ($property in $texts.EnumerateObject()) {
            if ($seen.ContainsKey($property.Name)) {
                $problems.Add("$($file.Name): clave repetida '$($property.Name)'")
            }
            $seen[$property.Name] = $true
            if ([string]::IsNullOrWhiteSpace($property.Value.GetString())) {
                $problems.Add("$($file.Name): texto vacío '$($property.Name)'")
            }
            elseif ($property.Value.GetString().Length -gt 170 -and $property.Name -ne "Dnet.HelpBody") {
                $warnings.Add("$($file.Name): texto muy largo '$($property.Name)'")
            }
            if ($referenceTokens.ContainsKey($property.Name)) {
                $actualTokens = @([regex]::Matches($property.Value.GetString(), '\{[^}]+\}') |
                    ForEach-Object { $_.Value })
                if (($actualTokens -join '|') -ne ($referenceTokens[$property.Name] -join '|')) {
                    $problems.Add("$($file.Name): marcadores incorrectos en '$($property.Name)'")
                }
            }
        }
        foreach ($key in $requiredKeys) {
            if (-not $seen.ContainsKey($key)) { $problems.Add("$($file.Name): falta '$key'") }
        }
        if ($seen.ContainsKey('Preferences.PatternHelp')) {
            $patternTokens = @([regex]::Matches($texts.GetProperty('Preferences.PatternHelp').GetString(), '\{[^}]+\}') |
                ForEach-Object { $_.Value })
            if (($patternTokens -join '|') -ne ($requiredPatternTokens -join '|')) {
                $problems.Add("$($file.Name): marcadores de nombre automático incorrectos")
            }
        }
        $document.Dispose()
    }
    catch {
        $problems.Add("$($file.Name): JSON no válido o estructura incompleta: $($_.Exception.Message)")
    }
}

Write-Host "Paquetes revisados: $($files.Count)"
Write-Host "Avisos: $($warnings.Count)"
foreach ($warning in $warnings) { Write-Host "AVISO  $warning" -ForegroundColor Yellow }
Write-Host "Errores: $($problems.Count)"
foreach ($problem in $problems) { Write-Host "ERROR  $problem" -ForegroundColor Red }
if ($problems.Count -gt 0) { exit 1 }
Write-Host "Validación estructural correcta." -ForegroundColor Green
