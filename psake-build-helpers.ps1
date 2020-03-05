
function Get-Copyright {
    $date = Get-Date
    $year = $date.Year
    $copyrightSpan = if ($year -eq $yearInitiated) { $year } else { "$yearInitiated-$year" }
    return "© $copyrightSpan $owner"
}

function Publish-Project {
    $project = Split-Path $pwd -Leaf
    Write-Host "Publishing $project"
    dotnet publish --configuration $configuration --no-build --output $publish/$project /nologo
}

function Set-Regenerated-File($path, $newContent) {
    if (-not (test-path $path -PathType Leaf)) {
        $oldContent = $null
    } else {
        $oldContent = [IO.File]::ReadAllText($path)
    }

    if ($newContent -ne $oldContent) {
        write-host "Generating $path"
        [System.IO.File]::WriteAllText($path, $newContent, [System.Text.Encoding]::UTF8)
    }
}

function Remove-Directory-Silently($path) {
    if (test-path $path) {
        write-host "Deleting $path"
        Remove-Item $path -recurse -force -ErrorAction SilentlyContinue | out-null
    }
}
