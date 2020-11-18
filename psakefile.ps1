include "./psake-build-helpers.ps1"

properties {
    $configuration = 'Release'
    $owner = 'Jimmy Bogard'
    $product = 'Contoso University Core'
    $yearInitiated = '2016'
    $projectRootDirectory = "$(resolve-path .)"
    $publish = "$projectRootDirectory/publish"
    $testResults = "$projectRootDirectory/TestResults"
}
 
task default -depends Test
task CI -depends Clean, Test, Publish -description "Continuous Integration process"
task Rebuild -depends Clean, Compile -description "Rebuild the code and database, no testing"

task Info -description "Display runtime information" {
    exec { dotnet --info }
}

task MigrateTest -description "Recreate the testing database" {
    # drop and recreate the test database
    exec { dotnet rh /d=ContosoUniversityDotNetCore-Pages-Test /f=ContosoUniversity/App_Data /s="(LocalDb)\mssqllocaldb" /silent /drop }
    exec { dotnet rh /d=ContosoUniversityDotNetCore-Pages-Test /f=ContosoUniversity/App_Data /s="(LocalDb)\mssqllocaldb" /silent /simple }
}

task Test -depends Compile, MigrateTest -description "Run unit tests" {
    # find any directory that ends in "Tests" and execute a test
    exec { dotnet test --configuration $configuration --no-build -l "trx;LogFileName=$($_.name).trx" -l "html;LogFileName=$($_.name).html" -l "console;verbosity=normal" -r $testResults }
}
 
task Compile -depends Info -description "Compile the solution" {
    exec { dotnet build --configuration $configuration --nologo -p:"Product=$($product)" -p:"Copyright=$(get-copyright)" } -workingDirectory .
}

task Publish -depends Compile -description "Publish the primary projects for distribution" {
    remove-directory-silently $publish
    exec { publish-project } -workingDirectory ContosoUniversity
}

task Migrate -description "Migrate the changes into the runtime database" {
    exec { dotnet rh /d=ContosoUniversityDotNetCore-Pages /f=ContosoUniversity\App_Data /s="(LocalDb)\mssqllocaldb" /silent }
}
  
task Clean -description "Clean out all the binary folders" {
    exec { dotnet clean --configuration $configuration /nologo } -workingDirectory .
    remove-directory-silently $publish
    remove-directory-silently $testResults
}

task ? -alias help -description "Display help content and possible targets" {
    WriteDocumentation
}
