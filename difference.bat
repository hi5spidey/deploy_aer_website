@echo off
setlocal

:: Generate a random number between 0 and 32767
set /A "rand=%RANDOM%"

:: Divide by 2 and check the remainder (modulo) to get 0 or 1
set /A "result=%rand% %% 2"

if %result% EQU 0 (
    echo [SUCCESS] The random number %rand% yielded a 0.
    exit /b 0
) else (
    echo [ERROR] The random number %rand% yielded a 1.
    exit /b 1
)
