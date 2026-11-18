@echo off
cd /d %~dp0
if exist "%~dp0jdk8\bin\javaw.exe" set "JAVA_HOME=%~dp0jdk8"
if defined JAVA_HOME set "PATH=%JAVA_HOME%\bin;%PATH%"
javaw -cp "out;lib/*" server.Main >> "data\server.log" 2>&1
