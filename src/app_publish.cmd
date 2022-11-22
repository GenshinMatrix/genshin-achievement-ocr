cd /d %~dp0
@cd GenshinAchievementOcr
dotnet publish -p:PublishProfile=FolderProfile
rd /s /q ".\GenshinAchievementOcr\bin\x64\Release\net6.0-windows10.0.18362.0\publish\win-x64\"
explorer /select, ".\bin\x64\Release\net6.0-windows10.0.18362.0\publish\win-x64\GenshinAchievementOcr.exe"
cd ".\bin\x64\Release\net6.0-windows10.0.18362.0\publish\win-x64\"
mkdir GenshinAchievementOcr
copy *.* .\GenshinAchievementOcr\
7z.exe a GenshinAchievementOcr.7z GenshinAchievementOcr\ 
@pause
