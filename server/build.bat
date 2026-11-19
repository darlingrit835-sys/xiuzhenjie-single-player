@echo off
chcp 65001 >nul
cd /d "%~dp0"
if exist "%~dp0jdk8\bin\javac.exe" set "JAVA_HOME=%~dp0jdk8"
if defined JAVA_HOME set "PATH=%JAVA_HOME%\bin;%PATH%"
if not exist out mkdir out
echo [build] compiling...
powershell.exe -NoProfile -Command ^
    "$ErrorActionPreference = 'Stop'; $sources = @(Get-ChildItem -LiteralPath 'src' -Recurse -File -Filter '*.java' | ForEach-Object { Resolve-Path -Relative -LiteralPath $_.FullName }); & javac -encoding UTF-8 -J-Xmx2g -cp 'lib/*' -d out @sources; exit $LASTEXITCODE"
if %errorlevel% neq 0 (
    echo [build] FAILED
    pause
    exit /b 1
)
echo [build] OK
