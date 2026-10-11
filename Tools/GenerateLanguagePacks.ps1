param(
    [string]$SourceLanguage = 'es'
)

$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path -Parent $PSScriptRoot
$uiTextPath = Join-Path $projectRoot 'CapturaLibre\UiText.cs'
$outputFolder = Join-Path $projectRoot 'CapturaLibre\LanguagePacks'
[System.IO.Directory]::CreateDirectory($outputFolder) | Out-Null

$source = [System.IO.File]::ReadAllText($uiTextPath)
$start = $source.IndexOf('["es"] = new()', [StringComparison]::Ordinal)
$end = $source.IndexOf('["en"] = new()', $start, [StringComparison]::Ordinal)
if ($start -lt 0 -or $end -lt 0) { throw 'No se pudo localizar el bloque español de UiText.cs.' }
$spanishBlock = $source.Substring($start, $end - $start)
$matches = [regex]::Matches($spanishBlock, '\["(?<key>[^"]+)"\]\s*=\s*"(?<value>(?:\\.|[^"\\])*)"')
$sourceTexts = [ordered]@{}
foreach ($match in $matches) {
    $encoded = '"' + $match.Groups['value'].Value + '"'
    $sourceTexts[$match.Groups['key'].Value] = [System.Text.Json.JsonSerializer]::Deserialize($encoded, [string])
}
if ($sourceTexts.Count -lt 60) { throw "Solo se encontraron $($sourceTexts.Count) textos de origen." }

$languages = @(
    @{ Code='ca'; Target='ca'; Name='Català'; Order=10 },
    @{ Code='val'; Target='ca'; Name='Valencià'; Order=11 },
    @{ Code='gl'; Target='gl'; Name='Galego'; Order=12 },
    @{ Code='eu'; Target='eu'; Name='Euskara'; Order=13 },
    @{ Code='oc-ES'; Target='oc'; Name='Aranés / Occitan'; Order=14 },
    @{ Code='nl'; Target='nl'; Name='Nederlands'; Order=100 },
    @{ Code='pl'; Target='pl'; Name='Polski'; Order=101 },
    @{ Code='ro'; Target='ro'; Name='Română'; Order=102 },
    @{ Code='ru'; Target='ru'; Name='Русский'; Order=103 },
    @{ Code='da'; Target='da'; Name='Dansk'; Order=104 },
    @{ Code='sv'; Target='sv'; Name='Svenska'; Order=105 },
    @{ Code='no'; Target='no'; Name='Norsk'; Order=106 },
    @{ Code='fi'; Target='fi'; Name='Suomi'; Order=107 },
    @{ Code='is'; Target='is'; Name='Íslenska'; Order=108 },
    @{ Code='cs'; Target='cs'; Name='Čeština'; Order=109 },
    @{ Code='sk'; Target='sk'; Name='Slovenčina'; Order=110 },
    @{ Code='hu'; Target='hu'; Name='Magyar'; Order=111 },
    @{ Code='el'; Target='el'; Name='Ελληνικά'; Order=112 },
    @{ Code='bg'; Target='bg'; Name='Български'; Order=113 },
    @{ Code='uk'; Target='uk'; Name='Українська'; Order=114 },
    @{ Code='hr'; Target='hr'; Name='Hrvatski'; Order=115 },
    @{ Code='sr'; Target='sr-Cyrl'; Name='Српски'; Order=116 },
    @{ Code='sl'; Target='sl'; Name='Slovenščina'; Order=117 },
    @{ Code='tr'; Target='tr'; Name='Türkçe'; Order=118 },
    @{ Code='et'; Target='et'; Name='Eesti'; Order=119 },
    @{ Code='lv'; Target='lv'; Name='Latviešu'; Order=120 },
    @{ Code='lt'; Target='lt'; Name='Lietuvių'; Order=121 },
    @{ Code='mk'; Target='mk'; Name='Македонски'; Order=122 },
    @{ Code='sq'; Target='sq'; Name='Shqip'; Order=123 },
    @{ Code='ga'; Target='ga'; Name='Gaeilge'; Order=124 },
    @{ Code='cy'; Target='cy'; Name='Cymraeg'; Order=125 },
    @{ Code='mt'; Target='mt'; Name='Malti'; Order=126 },
    @{ Code='pt-BR'; Target='pt'; Name='Português (Brasil)'; Order=127 },
    @{ Code='zh-Hans'; Target='zh-Hans'; Name='简体中文'; Order=200 },
    @{ Code='zh-Hant'; Target='zh-Hant'; Name='繁體中文'; Order=201 },
    @{ Code='ja'; Target='ja'; Name='日本語'; Order=202 },
    @{ Code='ko'; Target='ko'; Name='한국어'; Order=203 },
    @{ Code='hi'; Target='hi'; Name='हिन्दी'; Order=204 },
    @{ Code='bn'; Target='bn'; Name='বাংলা'; Order=205 },
    @{ Code='ta'; Target='ta'; Name='தமிழ்'; Order=206 },
    @{ Code='te'; Target='te'; Name='తెలుగు'; Order=207 },
    @{ Code='mr'; Target='mr'; Name='मराठी'; Order=208 },
    @{ Code='gu'; Target='gu'; Name='ગુજરાતી'; Order=209 },
    @{ Code='pa'; Target='pa'; Name='ਪੰਜਾਬੀ'; Order=210 },
    @{ Code='ar'; Target='ar'; Name='العربية'; Order=211; Rtl=$true },
    @{ Code='he'; Target='he'; Name='עברית'; Order=212; Rtl=$true },
    @{ Code='fa'; Target='fa'; Name='فارسی'; Order=213; Rtl=$true },
    @{ Code='ur'; Target='ur'; Name='اردو'; Order=214; Rtl=$true },
    @{ Code='id'; Target='id'; Name='Bahasa Indonesia'; Order=215 },
    @{ Code='ms'; Target='ms'; Name='Bahasa Melayu'; Order=216 },
    @{ Code='th'; Target='th'; Name='ไทย'; Order=217 },
    @{ Code='vi'; Target='vi'; Name='Tiếng Việt'; Order=218 },
    @{ Code='fil'; Target='fil'; Name='Filipino'; Order=219 },
    @{ Code='sw'; Target='sw'; Name='Kiswahili'; Order=220 },
    @{ Code='af'; Target='af'; Name='Afrikaans'; Order=221 }
)

$script:BingSession = New-Object Microsoft.PowerShell.Commands.WebRequestSession
$script:BingRequestNumber = 0

function Initialize-BingTranslationSession {
    $page = (Invoke-WebRequest -Uri 'https://www.bing.com/translator' -WebSession $script:BingSession -TimeoutSec 30).Content
    $abuse = [regex]::Match($page, 'params_AbusePreventionHelper\s*=\s*\[(\d+),"([^"]+)"')
    $script:BingKey = $abuse.Groups[1].Value
    $script:BingToken = $abuse.Groups[2].Value
    $script:BingIg = [regex]::Match($page, 'IG:"([^"]+)"').Groups[1].Value
    $script:BingIid = [regex]::Match($page, 'data-iid="([^"]+)"').Groups[1].Value
    $script:BingRequestNumber = 0
    if (-not $script:BingKey -or -not $script:BingToken -or -not $script:BingIg -or -not $script:BingIid) {
        throw 'No se pudieron obtener los datos de la sesión pública de traducción.'
    }
}

Initialize-BingTranslationSession

function Invoke-Translation([string]$text, [string]$target) {
    if ([string]::IsNullOrWhiteSpace($text)) { return $text }
    for ($attempt = 1; $attempt -le 3; $attempt++) {
        try {
            $script:BingRequestNumber++
            $uri = "https://www.bing.com/ttranslatev3?isVertical=1&IG=$script:BingIg&IID=$script:BingIid.$script:BingRequestNumber"
            $response = Invoke-RestMethod -Uri $uri -Method Post -WebSession $script:BingSession -TimeoutSec 30 -Body @{
                fromLang = $SourceLanguage
                text = $text
                to = $target
                token = $script:BingToken
                key = $script:BingKey
            }
            if (-not $response.translations) {
                throw ('Respuesta de traducción no válida: ' + ($response | ConvertTo-Json -Depth 4 -Compress))
            }
            $translatedText = [string]$response.translations[0].text
            if ([string]::IsNullOrWhiteSpace($translatedText)) { throw 'Traducción vacía.' }
            Start-Sleep -Milliseconds 350
            return $translatedText
        }
        catch {
            if ($attempt -eq 3) { throw }
            Start-Sleep -Milliseconds (700 * $attempt)
            Initialize-BingTranslationSession
        }
    }
}

function Normalize-Texts($texts) {
    $texts['Brand'] = 'CapturaLibre'
    $texts['Main.Title'] = $texts['Main.Title'] -replace '^.*?2\.5\.2', 'CapturaLibre 2.5.3'
    $editorTitle = [string]$texts['Editor.Title']
    $lastSeparator = $editorTitle.LastIndexOf(' - ', [StringComparison]::Ordinal)
    if ($lastSeparator -ge 0) {
        $texts['Editor.Title'] = $editorTitle.Substring(0, $lastSeparator) + ' - CapturaLibre'
    }
    $trigger = [string]$texts['Dnet.Trigger']
    $pathStart = $trigger.IndexOf('%LOCALAPPDATA%', [StringComparison]::OrdinalIgnoreCase)
    if ($pathStart -ge 0) {
        $texts['Dnet.Trigger'] = $trigger.Substring(0, $pathStart) +
            '%LOCALAPPDATA%\CapturaLibre_DNET\capturar_dnet.txt'
    }
}

foreach ($language in $languages) {
    $path = Join-Path $outputFolder ($language.Code + '.json')
    if (Test-Path -LiteralPath $path) {
        $existingPack = Get-Content -LiteralPath $path -Raw | ConvertFrom-Json -AsHashtable
        Normalize-Texts $existingPack.texts
        $existingJson = $existingPack | ConvertTo-Json -Depth 6
        [System.IO.File]::WriteAllText($path, $existingJson, [System.Text.UTF8Encoding]::new($false))
        Write-Host "Conservando $($language.Name) [$($language.Code)] ya generado."
        continue
    }
    Write-Host "Generando $($language.Name) [$($language.Code)]..."
    $translated = [ordered]@{}
    $entries = @($sourceTexts.GetEnumerator())
    $offset = 0
    while ($offset -lt $entries.Count) {
        $batch = @()
        $characters = 0
        while (($offset + $batch.Count) -lt $entries.Count -and $batch.Count -lt 8) {
            $candidate = $entries[$offset + $batch.Count]
            if ($batch.Count -gt 0 -and ($characters + $candidate.Value.Length) -gt 750) { break }
            $batch += $candidate
            $characters += $candidate.Value.Length
        }
        $separator = "`nZXQSEPZXQ`n"
        $joined = ($batch | ForEach-Object { $_.Value }) -join $separator
        $result = Invoke-Translation $joined $language.Target
        $parts = @($result -split 'ZXQSEPZXQ')
        if ($parts.Count -ne $batch.Count) {
            Write-Host "  El lote no conservó los separadores; traduciendo sus textos uno a uno."
            foreach ($item in $batch) {
                $translated[$item.Key] = Invoke-Translation $item.Value $language.Target
            }
            $offset += $batch.Count
            continue
        }
        for ($index = 0; $index -lt $batch.Count; $index++) {
            $translated[$batch[$index].Key] = $parts[$index].Trim("`r", "`n")
        }
        $offset += $batch.Count
    }
    $pack = [ordered]@{
        code = $language.Code
        displayName = $language.Name
        rightToLeft = [bool]$language.Rtl
        sortOrder = $language.Order
        texts = $translated
    }
    Normalize-Texts $pack.texts
    $json = $pack | ConvertTo-Json -Depth 6
    [System.IO.File]::WriteAllText($path, $json, [System.Text.UTF8Encoding]::new($false))
}

Write-Host "Listo: $($languages.Count) paquetes y $($sourceTexts.Count) textos por idioma."
