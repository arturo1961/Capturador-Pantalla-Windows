#define MyAppName "CapturaLibre DNET"
#define MyAppVersion "2.5.2"
#define MyAppPublisher "arturo1961"
#define MyAppURL "https://github.com/arturo1961/Capturador-Pantalla-Windows"
#define MyAppExeName "CapturaLibre_DNET.exe"

[Setup]
AppId={{5C83B7C4-4D59-4A1C-94E0-88E2F57D2501}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}/issues
AppUpdatesURL={#MyAppURL}/releases
DefaultDirName={localappdata}\Programs\CapturaLibre DNET
DefaultGroupName=CapturaLibre DNET
DisableProgramGroupPage=yes
LicenseFile=..\LICENSE
OutputDir=..\instalador_2.5.2
OutputBaseFilename=Instalar_CapturaLibre_DNET_2.5.2
SetupIconFile=..\CapturaLibre\Assets\CapturaLibre.ico
UninstallDisplayIcon={app}\{#MyAppExeName}
Compression=lzma2/ultra64
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=lowest
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
CloseApplications=yes
RestartApplications=no
VersionInfoVersion=2.5.2.0
VersionInfoCompany={#MyAppPublisher}
VersionInfoDescription=Instalador de CapturaLibre DNET
VersionInfoProductName={#MyAppName}
VersionInfoProductVersion={#MyAppVersion}

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "french"; MessagesFile: "compiler:Languages\French.isl"
Name: "german"; MessagesFile: "compiler:Languages\German.isl"
Name: "italian"; MessagesFile: "compiler:Languages\Italian.isl"
Name: "portuguese"; MessagesFile: "compiler:Languages\Portuguese.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "..\publicar_2.5.2\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\CapturaLibre DNET"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\CapturaLibre DNET"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,CapturaLibre DNET}"; Flags: nowait postinstall skipifsilent
