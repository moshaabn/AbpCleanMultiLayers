# Define the directory containing the files
$directory = "D:\Work\YourCompany\yourproject-be"

Write-Host "Starting content replacement script..."

# Define the old and new strings for first replacement
$oldString = "MoShaabn.CleanArch"
$newString = "YourCompany.YourProject"
Write-Host "Processing files for: $oldString -> $newString"

Get-ChildItem -Path $directory -File -Recurse -Include "*.cs", "*.csproj", "*.sln", "*.json", "*.xml", "Dockerfile*", "docker-compose*.yml", "docker-compose*.yaml" | ForEach-Object {
    Write-Host "Checking file:" $_.FullName
    try {
        $content = Get-Content -Path $_.FullName -Raw -ErrorAction Stop
        if ($content -match [regex]::Escape($oldString)) {
            $newContent = $content -replace [regex]::Escape($oldString), $newString
            Set-Content -Path $_.FullName -Value $newContent -ErrorAction Stop
            Write-Host "Updated content in:" $_.FullName
        }
    }
    catch {
        Write-Warning "Failed to process content in: $($_.FullName) - $($_.Exception.Message)"
    }
}

# Define the old and new strings for second replacement
$oldString = "CleanArch"
$newString = "YourProject"
Write-Host "Processing files for: $oldString -> $newString"

Get-ChildItem -Path $directory -File -Recurse -Include "*.cs", "*.csproj", "*.sln", "*.json", "*.xml", "Dockerfile*", "docker-compose*.yml", "docker-compose*.yaml" | ForEach-Object {
    Write-Host "Checking file:" $_.FullName
    try {
        $content = Get-Content -Path $_.FullName -Raw -ErrorAction Stop
        if ($content -match [regex]::Escape($oldString)) {
            $newContent = $content -replace [regex]::Escape($oldString), $newString
            Set-Content -Path $_.FullName -Value $newContent -ErrorAction Stop
            Write-Host "Updated content in:" $_.FullName
        }
    }
    catch {
        Write-Warning "Failed to process content in: $($_.FullName) - $($_.Exception.Message)"
    }
}

Write-Host "Content replacement completed!"