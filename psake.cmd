@echo off
rem Helper script for those who want to run psake from cmd.exe

powershell -NoProfile -ExecutionPolicy Bypass -Command "invoke-psake %*; if ($psake.build_success -eq $false) { exit 1 } else { exit 0 }"
