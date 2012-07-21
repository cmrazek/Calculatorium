; CalcInstall.nsi
; 2010-02-23 CDM Created

; The name of the installer
Name "Calculatorium 1.0.2"

; The file to write
OutFile "Calculatorium_Setup_1.0.2.exe"

; The default installation directory
InstallDir $PROGRAMFILES\Calculatorium

; Registry key to check for directory (so if you install again, it will overwrite the old one automatically)
InstallDirRegKey HKLM "Software\Calculatorium" "Install_Dir"

; Request application privileges for Windows Vista
RequestExecutionLevel admin

;--------------------------------

; Pages

Page components
Page directory
Page instfiles

UninstPage uninstConfirm
UninstPage instfiles

;--------------------------------

; The stuff to install
Section "Calculatorium (required)"
  SectionIn RO
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put file there
  File "Calculatorium.exe"
  File "Default.xml"
  
  ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\Calculatorium "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr   HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Calculatorium" "DisplayName" "Calculatorium 1.0.2"
  WriteRegStr   HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Calculatorium" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Calculatorium" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Calculatorium" "NoRepair" 1
  WriteUninstaller "uninstall.exe"
  
SectionEnd

; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts"

  CreateDirectory "$SMPROGRAMS\Calculatorium"
  CreateShortCut "$SMPROGRAMS\Calculatorium\Uninstall.lnk"     "$INSTDIR\uninstall.exe"    "" "$INSTDIR\uninstall.exe"     0
  CreateShortCut "$SMPROGRAMS\Calculatorium\Calculatorium.lnk" "$INSTDIR\Calculatorium.exe" "" "$INSTDIR\Calculatorium.exe" 0
  
SectionEnd

;--------------------------------

; Uninstaller

Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Calculatorium"
  DeleteRegKey HKLM SOFTWARE\Calculatorium

  ; Remove files and uninstaller
  Delete $INSTDIR\Calculatorium.exe
  Delete $INSTDIR\uninstall.exe

  ; Remove shortcuts, if any
  Delete "$SMPROGRAMS\Calculatorium\*.*"

  ; Remove directories used
  RMDir "$SMPROGRAMS\Calculatorium"
  RMDir "$INSTDIR"

SectionEnd
