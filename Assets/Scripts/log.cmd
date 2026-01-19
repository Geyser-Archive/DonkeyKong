@echo off

setlocal EnableDelayedExpansion

set "directory="
set "extensions=*.cs"

if "%directory%" == "" (
  set "directory=."
)

if "%extensions%" == "" (
  set "extensions=*"
)

set /a "fileCount=0"
set /a "lineCount=0"

for /r "%directory%"  %%i in ("%extensions%") do (
  set "filePath=%%i"
  
  for /f %%j in ('find /v /c "" ^< "!filePath!"') do (
    set /a "fileCount+=1"
    set /a "lineCount+=%%j"
    echo !fileCount!. %%~nxi - %%j lines
  )
)

echo --------------------------------

echo Total data: %fileCount% files, %lineCount% lines

pause