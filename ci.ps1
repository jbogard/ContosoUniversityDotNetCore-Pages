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

exec { & docker-compose -f .\docker-compose.ci.yml -p contosouniversitydotnetcore-ci up -d --build --remove-orphans --force-recreate }

exec { & docker ps }

exec { & docker run `
  -e "ConnectionStrings:DefaultConnection=Server=test-db;Database=contosouniversity-test;User Id=sa;Password=Pass@word" `
   contosouniversitydotnetcoreci_ci:latest `
   .\Build.ps1  `
}