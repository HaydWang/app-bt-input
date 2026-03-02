; BT Input Inno Setup Script
; Usage:
; 1) Build publish output:
;    dotnet publish ../BtInput.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
; 2) Update Source path below if needed
; 3) Compile this script with Inno Setup

#define MyAppName "BT Input"
#define MyAppVersion "1.0.18"
#define MyAppPublisher "BT Input Team"
#define MyAppExeName "BtInput.exe"

[Setup]
AppId={{6B29E3DE-5A01-4B4A-BD34-8B69D5C6A1A2}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\BT Input
DefaultGroupName=BT Input
DisableProgramGroupPage=yes
OutputDir=.
OutputBaseFilename=BTInput-Setup-{#MyAppVersion}
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "chinesesimp"; MessagesFile: "compiler:Default.isl"
Name: "english"; MessagesFile: "compiler:Languages\English.isl"

[Files]
Source: "..\bin\Release\net8.0-windows10.0.22621.0\win-x64\publish\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\BT Input"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\BT Input"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "Create a desktop icon"; GroupDescription: "Additional icons:"; Flags: unchecked

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Launch BT Input"; Flags: nowait postinstall skipifsilent
