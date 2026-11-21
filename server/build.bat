@echo off
setlocal
chcp 65001 >nul
cd /d "%~dp0"
if exist "%~dp0jdk8\bin\javac.exe" set "JAVA_HOME=%~dp0jdk8"
if defined JAVA_HOME set "PATH=%JAVA_HOME%\bin;%PATH%"
echo [build] compiling...
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0build.ps1"
if errorlevel 1 (
    echo [build] FAILED
    exit /b 1
)
echo [build] OK
exit /b 0
