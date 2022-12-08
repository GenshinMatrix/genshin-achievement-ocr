cd /d %~dp0
rename GenshinAchievementOcrSetup_*_x64.cer App.cer
rename GenshinAchievementOcrSetup_*_x64.msixbundle App.msixbundle
del App_Setup.exe
nsis\tools\makensis .\nsis\setup.nsi
del GenshinAchievementOcrSetup.exe
rename App_Setup.exe GenshinAchievementOcrSetup.exe
@pause
