#!/usr/bin/env pwsh

# Helper script for those who want to run psake from cmd.exe

invoke-psake @args; if ($psake.build_success -eq $false) { exit 1 } else { exit 0 }
