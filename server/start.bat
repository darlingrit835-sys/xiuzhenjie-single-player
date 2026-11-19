@echo off
setlocal
chcp 65001 >nul
cd /d "%~dp0"
if not defined XZJ_BACKEND set "XZJ_BACKEND=go"
if /i "%XZJ_BACKEND%"=="go" (
    powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0..\server-go\start.ps1" %*
    exit /b
)
if /i not "%XZJ_BACKEND%"=="java" (
    echo XZJ_BACKEND must be go or java.
    exit /b 2
)
if exist "%~dp0jdk8\bin\java.exe" set "JAVA_HOME=%~dp0jdk8"
if defined JAVA_HOME set "PATH=%JAVA_HOME%\bin;%PATH%"
echo ============================================
echo  修真界私服服务端
echo  账号服: https://0.0.0.0:9443
echo  游戏服: tcp://0.0.0.0:17666
echo  战斗服: tcp://0.0.0.0:17667
echo ============================================
if /i "%XZJ_JAVA_PROFILE%"=="default" (
    java -cp "out;lib/*" server.Main
) else (
    java -Xms32m -Xmx256m -XX:+UseSerialGC -XX:ActiveProcessorCount=2 -cp "out;lib/*" server.Main
)
pause
