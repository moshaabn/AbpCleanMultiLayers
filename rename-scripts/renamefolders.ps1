<#
.SYNOPSIS
  Rename files and directories by replacing a substring, with optional content replacement.

.DESCRIPTION
  - Replaces $OldString with $NewString in file/directory names under $Directory.
  - Optionally replaces occurrences inside text files (choose file types).
  - Renames directories deepest-first to avoid path-change problems.
  - Supports a -WhatIf preview mode.
  - Logs actions to a CSV (rename-log.csv by default).

.PARAMETER Directory
  Root path to process.

.PARAMETER OldString
  The text to replace.

.PARAMETER NewString
  The replacement text.

.PARAMETER ReplaceFileContents
  If set, replace occurrences inside files (for the extensions in $FilePatterns).

.PARAMETER FilePatterns
  Array of filename patterns to consider for content replacement (defaults to common text/code files).

.PARAMETER Recurse
  Process subfolders when present.

.PARAMETER WhatIf
  Preview only: show what would be changed, don't write changes.

#>

param(
    [string]$Directory = "D:\Work\YourCompany\yourproject-be",
    [string]$OldString = "MoShaabn.CleanArch",
    [string]$NewString = "YourCompany.YourProject",
    [switch]$ReplaceFileContents,
    [string[]]$FilePatterns = @('*.sln', '*.csproj', '*.cs', '*.config', '*.xml', '*.json', '*.md', '*.txt', '*.ps1', '*.psm1', '*.psd1', '*.xaml', '*.cshtml', '*.vb', '*.fs'),
    [switch]$Recurse,
    [switch]$WhatIf
)

# Initialize logging helper
$log = @()
function Add-Log {
    param($Type, $Path, $Detail)
    $log += [PSCustomObject]@{
        Time   = (Get-Date).ToString("s")
        Type   = $Type
        Path   = $Path
        Detail = $Detail
    }
}

Write-Host "Starting rename run..." -ForegroundColor Cyan

# Validate directory
if (-not (Test-Path -LiteralPath $Directory -PathType Container)) {
    Write-Error "Directory not found: $Directory"
    exit 1
}

# Normalize old/new strings for use with -replace
$escapedOldRegex = [regex]::Escape($OldString)

# 1) Optionally replace file contents (text files)
if ($ReplaceFileContents) {
    Write-Host "Searching files for content replacement..." -ForegroundColor Cyan

    # -Include works properly with -Recurse, so we pass the switch boolean
    $fileList = Get-ChildItem -Path $Directory -File -Recurse:$Recurse -Include $FilePatterns -ErrorAction SilentlyContinue

    foreach ($file in $fileList) {
        try {
            $content = Get-Content -LiteralPath $file.FullName -Raw -ErrorAction Stop
        }
        catch {
            Write-Warning "Skipping unreadable or locked file: $($file.FullName)"
            Add-Log -Type "ContentSkip" -Path $file.FullName -Detail $_.Exception.Message
            continue
        }

        if ($content -match $escapedOldRegex) {
            $newContent = $content -replace $escapedOldRegex, $NewString
            if ($WhatIf) {
                Write-Host "[WhatIf] Would update contents: $($file.FullName)"
                Add-Log -Type "ContentPreview" -Path $file.FullName -Detail "Would replace content occurrences"
            }
            else {
                try {
                    Set-Content -LiteralPath $file.FullName -Value $newContent -Encoding UTF8 -Force -ErrorAction Stop
                    Write-Host "Updated contents:" $file.FullName -ForegroundColor Green
                    Add-Log -Type "ContentUpdated" -Path $file.FullName -Detail "Replaced content occurrences"
                }
                catch {
                    Write-Warning "Failed to update file content: $($file.FullName) - $($_.Exception.Message)"
                    Add-Log -Type "ContentFail" -Path $file.FullName -Detail $_.Exception.Message
                }
            }
        }
    }
}

# 2) Rename files (names only)
Write-Host "Scanning files for name replacements..." -ForegroundColor Cyan
$filesToRename = Get-ChildItem -Path $Directory -File -Recurse:$Recurse -ErrorAction SilentlyContinue |
Where-Object { $_.Name -like "*$OldString*" }

foreach ($f in $filesToRename) {
    $oldName = $f.Name
    $newName = $oldName -replace $escapedOldRegex, $NewString

    if ($newName -eq $oldName) { continue }

    $targetPath = Join-Path -Path $f.DirectoryName -ChildPath $newName

    if (Test-Path -LiteralPath $targetPath) {
        Add-Log -Type "FileCollision" -Path $f.FullName -Detail "Target exists: $targetPath"
        Write-Warning "Skipping rename (target exists): $($f.FullName) -> $newName"
        continue
    }

    # Scriptblock to perform a rename (respects WhatIf)
    $performRename = {
        param($itemFull, $nName)
        try {
            if ($WhatIf) {
                Write-Host "[WhatIf] Would rename file: $itemFull -> $nName"
                Add-Log -Type "FilePreview" -Path $itemFull -Detail "Would rename to $nName"
            }
            else {
                Rename-Item -LiteralPath $itemFull -NewName $nName -Force -ErrorAction Stop
                Add-Log -Type "FileRenamed" -Path $itemFull -Detail "Renamed to $nName"
                Write-Host "Renamed file:" $itemFull "->" $nName -ForegroundColor Green
            }
        }
        catch {
            Add-Log -Type "FileRenameFail" -Path $itemFull -Detail $_.Exception.Message
            Write-Warning "Failed to rename file: $itemFull - $($_.Exception.Message)"
        }
    }

    # If the only difference is case (Windows), perform via temporary name
    if (($oldName.ToLower() -eq $newName.ToLower()) -and ($oldName -ne $newName)) {
        $tmpName = "$newName.__tmp__$(Get-Random -Maximum 999999)"
        $tmpPath = Join-Path -Path $f.DirectoryName -ChildPath $tmpName

        if (Test-Path -LiteralPath $tmpPath) {
            Add-Log -Type "TempCollision" -Path $f.FullName -Detail "Temp name collision, skipping"
            Write-Warning "Temp name collision for: $($f.FullName) - skipping"
            continue
        }

        if ($WhatIf) {
            Write-Host "[WhatIf] Would perform case-only rename via temp: $($f.FullName) -> $tmpName -> $newName"
            Add-Log -Type "FilePreview" -Path $f.FullName -Detail "Would case-only rename via temp"
        }
        else {
            try {
                Rename-Item -LiteralPath $f.FullName -NewName $tmpName -Force -ErrorAction Stop
                Rename-Item -LiteralPath (Join-Path -Path $f.DirectoryName -ChildPath $tmpName) -NewName $newName -Force -ErrorAction Stop
                Write-Host "Renamed (case-only handled):" $f.FullName "->" $newName -ForegroundColor Green
                Add-Log -Type "FileRenamed" -Path $f.FullName -Detail "Renamed to $newName via temp"
            }
            catch {
                Add-Log -Type "FileRenameFail" -Path $f.FullName -Detail $_.Exception.Message
                Write-Warning "Failed to rename (case-only): $($f.FullName) - $($_.Exception.Message)"
            }
        }
    }
    else {
        & $performRename $f.FullName $newName
    }
}

# 3) Rename directories (deepest-first)
Write-Host "Scanning directories for name replacements (deepest-first)..." -ForegroundColor Cyan
$dirsToRename = Get-ChildItem -Path $Directory -Directory -Recurse:$Recurse -ErrorAction SilentlyContinue |
Where-Object { $_.Name -like "*$OldString*" } |
Sort-Object { $_.FullName.Length } -Descending

foreach ($d in $dirsToRename) {
    $oldName = $d.Name
    $newName = $oldName -replace $escapedOldRegex, $NewString
    if ($newName -eq $oldName) { continue }

    # Parent may be $null in rare cases, handle safely
    $parent = if ($d.Parent) { $d.Parent.FullName } else { Split-Path -Path $d.FullName -Parent }
    $targetPath = Join-Path -Path $parent -ChildPath $newName

    if (Test-Path -LiteralPath $targetPath) {
        Add-Log -Type "DirCollision" -Path $d.FullName -Detail "Target exists: $targetPath"
        Write-Warning "Skipping rename (target exists): $($d.FullName) -> $newName"
        continue
    }

    # Case-only handling for directory names
    if (($oldName.ToLower() -eq $newName.ToLower()) -and ($oldName -ne $newName)) {
        $tmpName = "$newName.__tmp__$(Get-Random -Maximum 999999)"
        $tmpPath = Join-Path -Path $parent -ChildPath $tmpName

        if (Test-Path -LiteralPath $tmpPath) {
            Add-Log -Type "DirTempCollision" -Path $d.FullName -Detail "Temp name collision, skipping"
            Write-Warning "Temp name collision for: $($d.FullName) - skipping"
            continue
        }

        if ($WhatIf) {
            Write-Host "[WhatIf] Would perform case-only dir rename via temp: $($d.FullName) -> $tmpName -> $newName"
            Add-Log -Type "DirPreview" -Path $d.FullName -Detail "Would case-only rename via temp"
        }
        else {
            try {
                Rename-Item -LiteralPath $d.FullName -NewName $tmpName -Force -ErrorAction Stop
                Rename-Item -LiteralPath (Join-Path -Path $parent -ChildPath $tmpName) -NewName $newName -Force -ErrorAction Stop
                Write-Host "Renamed directory (case-only handled):" $d.FullName "->" $newName -ForegroundColor Green
                Add-Log -Type "DirRenamed" -Path $d.FullName -Detail "Renamed to $newName via temp"
            }
            catch {
                Add-Log -Type "DirRenameFail" -Path $d.FullName -Detail $_.Exception.Message
                Write-Warning "Failed to rename directory: $($d.FullName) - $($_.Exception.Message)"
            }
        }
    }
    else {
        try {
            if ($WhatIf) {
                Write-Host "[WhatIf] Would rename directory: $($d.FullName) -> $newName"
                Add-Log -Type "DirPreview" -Path $d.FullName -Detail "Would rename to $newName"
            }
            else {
                Rename-Item -LiteralPath $d.FullName -NewName $newName -ErrorAction Stop
                Write-Host "Renamed directory:" $d.FullName "->" $newName -ForegroundColor Green
                Add-Log -Type "DirRenamed" -Path $d.FullName -Detail "Renamed to $newName"
            }
        }
        catch {
            Add-Log -Type "DirRenameFail" -Path $d.FullName -Detail $_.Exception.Message
            Write-Warning "Failed to rename directory: $($d.FullName) - $($_.Exception.Message)"
        }
    }
}

# Export the log
$logPath = Join-Path -Path (Get-Location) -ChildPath "rename-log.csv"
try {
    $log | Export-Csv -Path $logPath -NoTypeInformation -Force
    Write-Host "Log written to: $logPath" -ForegroundColor Cyan
}
catch {
    Write-Warning "Failed to write log: $($_.Exception.Message)"
}

Write-Host "Done." -ForegroundColor Cyan
