# Taken from psake https://github.com/psake/psake

<#
.SYNOPSIS
  This is a helper function that runs a scriptblock and checks the PS variable $lastexitcode
  to see if an error occcured. If an error is detected then an exception is thrown.
  This function allows you to run command-line programs without having to
  explicitly check the $lastexitcode variable.
.EXAMPLE
  exec { svn info $repository_trunk } "Error executing SVN. Please verify SVN command-line client is installed"
#>
function Exec
{
    [CmdletBinding()]
    param(
        [Parameter(Position=0,Mandatory=1)][scriptblock]$cmd,
        [Parameter(Position=1,Mandatory=0)][string]$errorMessage = ($msgs.error_bad_command -f $cmd)
    )
    & $cmd
    if ($lastexitcode -ne 0) {
        throw ("Exec: " + $errorMessage)
    }
}

if(Test-Path .\publish) { Remove-Item .\publish -Force -Recurse }


$branch = @{ $true = $env:APPVEYOR_REPO_BRANCH; $false = $(git symbolic-ref --short -q HEAD) }[$env:APPVEYOR_REPO_BRANCH -ne $NULL];
$revision = @{ $true = "{0:00000}" -f [convert]::ToInt32("0" + $env:APPVEYOR_BUILD_NUMBER, 10); $false = "local" }[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];
$suffix = @{ $true = ""; $false = "$($branch.Substring(0, [math]::Min(10,$branch.Length)))-$revision"}[$branch -eq "master" -and $revision -ne "local"]
$commitHash = $(git rev-parse --short HEAD)
$buildSuffix = @{ $true = "$($suffix)-$($commitHash)"; $false = "$($branch)-$($commitHash)" }[$suffix -ne ""]

exec { & rh /d=ContosoUniversityDotNetCore-Pages /f=ContosoUniversity\App_Data /s="(LocalDb)\mssqllocaldb" /silent }
exec { & rh /d=ContosoUniversityDotNetCore-Pages-Test /f=ContosoUniversity\App_Data /s="(LocalDb)\mssqllocaldb" /silent /drop }
exec { & rh /d=ContosoUniversityDotNetCore-Pages-Test /f=ContosoUniversity\App_Data /s="(LocalDb)\mssqllocaldb" /silent /simple }

exec { & dotnet restore }

exec { & dotnet build -c Release --version-suffix=$buildSuffix }

exec { & dotnet test -c Release --no-build }

Push-Location ContosoUniversity

try {
	exec { & dotnet publish -o ..\publish -c Release }
} finally {
	Pop-Location
}
 


