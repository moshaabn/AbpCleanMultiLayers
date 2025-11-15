# Define the directory containing the files
$directory = "D:\Work\YourCompany\yourproject-be"
Write-Host "Starting script..."

# Define the old and new strings
$oldString = "MoShaabn"
$newString = "YourCompany"
##
$oldString2 = "CleanArch"
$newString2 = "YourProject"

# Get all files in the directory (and subdirectories if needed)
Get-ChildItem -Path $directory -File -Recurse | ForEach-Object {
    # Check if the file name contains the old string
    if ($_.Name -like "*$oldString*") {
        # Replace the old string with the new string in the file name
        $newName = $_.Name -replace [regex]::Escape($oldString), $newString
        # Handle null DirectoryName
        if ($_.DirectoryName) {
            # Create the full path for the new file name
            $newPath = Join-Path -Path $_.DirectoryName -ChildPath $newName

            # # Check if the target already exists and delete it
            # if (Test-Path $newPath) {
            #     Write-Warning "Target already exists: $newPath. Deleting existing file..."
            #     Remove-Item -Path $newPath -Force
            # }
            
            try {
                Rename-Item -Path $_.FullName -NewName $newName -Force -ErrorAction Stop
                Write-Host "Renamed:" $_.FullName "->" $newPath
            }
            catch {
                Write-Warning "Failed to rename: $($_.FullName) - $($_.Exception.Message)"
            }
        } else {
            Write-Warning "Cannot determine directory for: $($_.FullName)"
        }
    }
}

# Process second string replacement
Get-ChildItem -Path $directory -File -Recurse | ForEach-Object {
    # Check if the file name contains the second old string
    if ($_.Name -like "*$oldString2*") {
        # Replace the old string with the new string in the file name
        $newName = $_.Name -replace [regex]::Escape($oldString2), $newString2
        # Handle null DirectoryName
        if ($_.DirectoryName) {
            # Create the full path for the new file name
            $newPath = Join-Path -Path $_.DirectoryName -ChildPath $newName

            # # Check if the target already exists and delete it
            # if (Test-Path $newPath) {
            #     Write-Warning "Target already exists: $newPath. Deleting existing file..."
            #     Remove-Item -Path $newPath -Force
            # }
            
            try {
                Rename-Item -Path $_.FullName -NewName $newName -Force -ErrorAction Stop
                Write-Host "Renamed:" $_.FullName "->" $newPath
            }
            catch {
                Write-Warning "Failed to rename: $($_.FullName) - $($_.Exception.Message)"
            }
        } else {
            Write-Warning "Cannot determine directory for: $($_.FullName)"
        }
    }
}