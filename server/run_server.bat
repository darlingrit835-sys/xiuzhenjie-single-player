@echo off
setlocal
cd /d "%~dp0"
if not defined XZJ_BACKEND set "XZJ_BACKEND=go"
if /i "%XZJ_BACKEND%"=="go" (
    powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0..\server-go\start.ps1" %* >> "data\server.log" 2>&1
    exit /b
)
if /i not "%XZJ_BACKEND%"=="java" (
    echo XZJ_BACKEND must be go or java.
    exit /b 2
)
if exist "%~dp0jdk8\bin\javaw.exe" set "JAVA_HOME=%~dp0jdk8"
if defined JAVA_HOME set "PATH=%JAVA_HOME%\bin;%PATH%"
if /i "%XZJ_JAVA_PROFILE%"=="default" (
    javaw -cp "out;lib/*" server.Main >> "data\server.log" 2>&1
) else (
    javaw -Xms32m -Xmx256m -XX:+UseSerialGC -XX:ActiveProcessorCount=2 -cp "out;lib/*" server.Main >> "data\server.log" 2>&1
)
