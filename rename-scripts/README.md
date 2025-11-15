# Rename Scripts Documentation

This folder contains three PowerShell scripts designed to help rename and refactor ABP Clean Architecture projects by replacing old naming conventions with new ones.

## Scripts Overview

1. **`renamefolders.ps1`** - Advanced script for renaming folders and files with comprehensive options
2. **`renamefiles.ps1`** - Simple script for renaming files only
3. **`renamecontent.ps1`** - Script for replacing content inside files

## Prerequisites

- Windows PowerShell 5.1 or later
- Appropriate permissions to modify files and folders in the target directory
- **Backup your project before running these scripts**

## Script Details

### 1. renamefolders.ps1 (Recommended)

This is the most comprehensive script with advanced features including preview mode and logging.

**Features:**
- Renames both folders and files
- Optionally replaces content inside files
- Preview mode with `-WhatIf` parameter
- Logging to CSV file
- Supports multiple file patterns
- Processes directories deepest-first to avoid path conflicts

**Parameters:**
- `Directory` - Root path to process (default: current directory)
- `OldString` - Text to replace (default: "MoShaabn.CleanArch")
- `NewString` - Replacement text (default: "Pinnacle.Pinnacle")
- `ReplaceFileContents` - Switch to enable content replacement
- `FilePatterns` - Array of file patterns to process
- `Recurse` - Process subdirectories
- `WhatIf` - Preview mode only

**Usage Examples:**

```powershell
# Preview what would be changed (recommended first step)
.\renamefolders.ps1 -Directory "C:\YourProject" -OldString "MoShaabn.CleanArch" -NewString "YourCompany.YourProject" -WhatIf -Recurse

# Rename folders and files only
.\renamefolders.ps1 -Directory "C:\YourProject" -OldString "MoShaabn.CleanArch" -NewString "YourCompany.YourProject" -Recurse

# Rename folders, files, AND replace content inside files
.\renamefolders.ps1 -Directory "C:\YourProject" -OldString "MoShaabn.CleanArch" -NewString "YourCompany.YourProject" -ReplaceFileContents -Recurse
```

### 2. renamefiles.ps1 (Simple Version)

A straightforward script that renames files in two passes:
1. Replaces "MoShaabn" with "Pinnacle"
2. Replaces "CleanArch" with "Pinnacle"

**Usage:**
```powershell
# Edit the script to change the directory and string values, then run:
.\renamefiles.ps1
```

**Configuration (edit the script):**
```powershell
$directory = "D:\Work\YourProject"
$oldString = "MoShaabn"
$newString = "YourCompany"
$oldString2 = "CleanArch"
$newString2 = "YourProject"
```

### 3. renamecontent.ps1 (Content Only)

This script only replaces content inside files, not filenames or folder names.

**File Types Processed:**
- `*.cs` - C# source files
- `*.csproj` - Project files
- `*.sln` - Solution files
- `*.json` - JSON configuration files
- `*.xml` - XML files
- `Dockerfile*` - Docker files
- `docker-compose*.yml/yaml` - Docker Compose files

**Usage:**
```powershell
# Edit the script to change the directory and string values, then run:
.\renamecontent.ps1
```

**Configuration (edit the script):**
```powershell
$directory = "D:\Work\YourProject"
# First replacement
$oldString = "MoShaabn.CleanArch"
$newString = "YourCompany.YourProject"
# Second replacement
$oldString = "CleanArch"
$newString = "YourProject"
```

## Recommended Workflow

1. **Backup your project** - Create a complete backup before starting
2. **Use Preview Mode** - Run `renamefolders.ps1` with `-WhatIf` to see what will change
3. **Run in Order** (if using individual scripts):
   - First: `renamefolders.ps1` (without `-ReplaceFileContents`)
   - Second: `renamecontent.ps1`
4. **Verify Results** - Check the generated `rename-log.csv` for details
5. **Test Build** - Ensure your project still builds correctly

## Example: Complete Refactoring

```powershell
# Step 1: Preview the changes
.\renamefolders.ps1 -Directory "C:\MyProject" -OldString "MoShaabn.CleanArch" -NewString "MyCompany.MyProject" -WhatIf -Recurse -ReplaceFileContents

# Step 2: Apply the changes (if preview looks good)
.\renamefolders.ps1 -Directory "C:\MyProject" -OldString "MoShaabn.CleanArch" -NewString "MyCompany.MyProject" -Recurse -ReplaceFileContents

# Step 3: Check the log file
Get-Content .\rename-log.csv
```

## Important Notes

- **Always backup your project first**
- Scripts modify the directory path hard-coded in the simple scripts (`renamefiles.ps1`, `renamecontent.ps1`)
- The `renamefolders.ps1` script is the most flexible and safest option
- Use `-WhatIf` parameter to preview changes before applying them
- Check the generated `rename-log.csv` file for detailed information about what was changed
- Test your project build after running the scripts to ensure everything works correctly

## Troubleshooting

- If you get execution policy errors, run: `Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser`
- If files are locked, close Visual Studio and other IDEs before running the scripts
- If you encounter permission errors, run PowerShell as Administrator
- Always check the console output and log files for any warnings or errors