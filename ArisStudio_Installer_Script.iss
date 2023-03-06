; This is the Inno Setup script for building Aris Studio Installer

; If you change a value in this script and that value is specific only
; to your machine (ie. BuildPath), don't stage this file in a commit
; (unless you want to improve this script).
; Make this a generic installer.

#define AppName "Aris Studio"
#define AppNameTrim "ArisStudio"
#define AppVersion "2.2.2-alpha.1" ; <- EDIT THIS VALUE IF NEEDED
#define AppPublisher "Aris Studio Developers"
#define AppURL "https://github.com/kiraio-moe/ArisStudio"
#define AppExeName "ArisStudio.exe"

; Change the BuildPath value and navigate to the folder containing the
; Aris Studio files that you have built. Be sure to always remove the
; Backslash character ( \ ) at the end of the path.
; Example: D:\Projects\Unity\ArisStudio\Build
#define BuildPath "" ; <- EDIT THIS VALUE

; Change the ResourcesPath value and navigate to the folder containing
; resources to build Aris Studio Installer (LICENSE.txt, etc.)
; Example: D:\Projects\Unity\ArisStudio\Installer
#define ResourcesPath "" ; <- EDIT THIS VALUE

;#define SignToolPath "D:\Projects\Unity\ArisStudio\Installer\signtool.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
;SignTool=MicrosoftSignTool {#SignToolPath} sign /f my_certificate.pfx /p mypassword /tr http://timestamp.digicert.com /td sha256 /fd sha256 /a $f
AppId={{DAF358AB-6734-4CDD-B412-9EC569EE2188}
AppName={#AppName}
AppVersion={#AppVersion}
;AppVerName={#AppName} {#AppVersion}
AppPublisher={#AppPublisher}
AppPublisherURL={#AppURL}
AppSupportURL={#AppURL}
AppUpdatesURL={#AppURL}
DefaultDirName={autopf}\{#AppName}
DefaultGroupName={#AppName}
AllowNoIcons=yes
LicenseFile={#ResourcesPath}\LICENSE.txt
InfoBeforeFile={#ResourcesPath}\INSTALL_MESSAGE.txt
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
OutputDir={#BuildPath}
OutputBaseFilename={#AppNameTrim}Installer-{#AppVersion}
SetupIconFile={#ResourcesPath}\{#AppNameTrim}_Icon.ico
Compression=lzma2
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "{#BuildPath}\{#AppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BuildPath}\{#AppNameTrim}_Data\*"; DestDir: "{app}\{#AppNameTrim}_Data"; Flags: ignoreversion recursesubdirs createallsubdirs
; Mono
Source: "{#BuildPath}\MonoBleedingEdge\*"; DestDir: "{app}\MonoBleedingEdge"; Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist
; IL2CPP
Source: "{#BuildPath}\GameAssembly.dll"; DestDir: "{app}"; Flags: ignoreversion skipifsourcedoesntexist
Source: "{#BuildPath}\UnityCrashHandler64.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BuildPath}\UnityPlayer.dll"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#AppName}"; Filename: "{app}\{#AppExeName}"
Name: "{group}\{cm:UninstallProgram,{#AppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#AppName}"; Filename: "{app}\{#AppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#AppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(AppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
