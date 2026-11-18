@echo off
chcp 65001 >nul
cd /d %~dp0
if exist "%~dp0jdk8\bin\java.exe" set "JAVA_HOME=%~dp0jdk8"
if defined JAVA_HOME set "PATH=%JAVA_HOME%\bin;%PATH%"
echo ============================================
echo  修真界私服服务端
echo  账号服: https://0.0.0.0:9443
echo  游戏服: tcp://0.0.0.0:17666
echo  战斗服: tcp://0.0.0.0:17667
echo ============================================
java -cp "out;lib/*" server.Main
pause
