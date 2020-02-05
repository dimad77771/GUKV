@echo off
if "%1"=="/?" goto help:
if "%1"=="" goto help:
goto start:
:help
@echo on
@echo Installs/unistalls localized satellite assemblies into/from the GAC
@echo.
@echo GACINSTALL [/u] [culture]
@echo.
@echo   /u        Unistall localized assemblies
@echo   culture   Culture name of a satellite asembly to install/unistall
@echo.
@echo Usage:
@echo.
@echo GACINSTALL he-IL
@echo Installs the hebrew satellite assemblies from the current folder's 'he-IL' subfolder
@echo.
@echo GACINSTALL /u he-IL
@echo Uninstalls the hebrew satellite assemblies
@echo.
@echo GACINSTALL /u
@echo Uninstalls all the installed satellite assemblies
@echo.
@echo off
goto quit
:start

set PathToGacUtilVS2005="%VS80COMNTOOLS%..\..\SDK\v2.0\Bin\"
set PathToGacUtilVS2008x86="%PROGRAMFILES%\Microsoft SDKs\Windows\v6.0A\Bin\"
set PathToGacUtilVS2010x86="%PROGRAMFILES%\Microsoft SDKs\Windows\v7.0A\Bin\"

set PathToGacUtilVS2008x64="%PROGRAMFILES(X86)%\Microsoft SDKs\Windows\v6.0A\Bin\"
set PathToGacUtilVS2010x64="%PROGRAMFILES(X86)%\Microsoft SDKs\Windows\v7.0A\Bin\"

set ArchiPostfix=""
if "%PROCESSOR_ARCHITECTURE%"=="AMD64" set ArchiPostfix=x64\
if "%PROCESSOR_ARCHITEW6432%"=="AMD64" set ArchiPostfix=x64\

set PathToGacUtil=%PathToGacUtilVS2010x86%^%ArchiPostfix%
if not exist %PathToGacUtil%gacutil.exe set PathToGacUtil=%PathToGacUtilVS2008x86%^%ArchiPostfix%
if not exist %PathToGacUtil%gacutil.exe set PathToGacUtil=%PathToGacUtilVS2010x64%^%ArchiPostfix%
if not exist %PathToGacUtil%gacutil.exe set PathToGacUtil=%PathToGacUtilVS2008x64%^%ArchiPostfix%
if not exist %PathToGacUtil%gacutil.exe set PathToGacUtil=%PathToGacUtilVS2005%
set GACUtil=%PathToGacUtil%
set GACUtil=%GACUtil%gacutil.exe
if not exist %GACUtil% (
echo could not find gacutil.exe
goto quit
)

if "%1"=="/u" goto unistall:
set CultureDir=
set DirKey=
if not "%1"=="" set CultureDir=%1"\\"
if not "%CultureDir%"=="" set DirKey=/S
@echo on
FOR /F "usebackq delims==" %%i IN (`dir /B %DirKey% %CultureDir%*.dll`) DO %GACUtil% /i "%%i"
@echo off
goto quit
:unistall
set CultureDir=
set CultureOptions=
if not "%2"=="" set CultureDir=%2"\\"
if not "%2"=="" set CultureOptions=,Culture=%2
echo CultureOptions=%CultureOptions%
FOR /F "usebackq delims==" %%i IN (`dir /B /S %CultureDir%*.dll`) DO %GACUtil% /u %%~ni%CultureOptions%
:quit