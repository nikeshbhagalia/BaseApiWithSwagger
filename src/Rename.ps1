<#
PowerShell script to rename C# Project step-by-step: 
* Copying project folder to folder with new project name
* Renaming .csproj file and other files with project name
* Changing project name reference in .sln solution file
* Changing RootNamespace and AssemblyName in .csproj file
* Renaming project inside AssemblyInfo.cs

param(
    [parameter(
        HelpMessage="Existing C# Project path to rename.",
        Mandatory=$true, ValueFromPipeline=$true)]
    [ValidateScript({
        if (Test-Path $_.Trim().Trim('"')) { $true }
        else { throw "Path does not exist: $_" }
    })]
    [string]$ProjectFilePath,
    
    [parameter(
        HelpMessage="New project file name, without extension.",
        Mandatory=$true)]
    [string]$NewProjectName,
 
    [parameter(
        HelpMessage="Path to solution file.",
        Mandatory=$true)]
    [ValidateScript({
        if (Test-Path $_.Trim().Trim('"')) { $true }
        else { throw "Path does not exist: $_" }
    })]
    [string]$SolutionFilePath
)#>
$curDir = Get-Location
Write-Host "Current Working Directory: $curDir"

$ProjectFilePath = "C:\Users\nikes\Desktop\Projects\Base Api With Swagger\src\BaseSwagger.Api\BaseSwagger.Api.csproj"
$NewProjectName = "SomethingNew"
$SolutionFilePath = "C:\Users\nikes\Desktop\Projects\Base Api With Swagger\BaseSwagger.sln"


((Get-Content -path $SolutionFilePath -Raw) -replace 'SomethingNew','BaseSwagger') | Set-Content -Path $SolutionFilePath

$ProjectFilePath = $ProjectFilePath.Trim().Trim('"')
$SolutionFilePath = $SolutionFilePath.Trim().Trim('"')

function ProceedOrExit {
    if ($?) { echo "Proceed.." } else { echo "Script FAILED! Exiting.."; exit 1 } 
}

echo "Rename project from '$OldProjectName' to '$NewProjectName'"
echo "=========="

$OldProjectFolder  = Split-Path $ProjectFilePath
echo "1. Set current location to project folder '$OldProjectFolder'"
cd $OldProjectFolder 
ProceedOrExit
        
$NewProjectFolder="..\$NewProjectName"
echo "----------"
echo "2. Copy project folder to '$NewProjectFolder'"
copy . $NewProjectFolder -Recurse #-WhatIf
ProceedOrExit

$NewProjectFolder=Resolve-Path $NewProjectFolder
echo "----------"
echo "3. Set current location to New project folder '$NewProjectFolder'"
cd $NewProjectFolder
ProceedOrExit

echo "----------"
echo "4. Rename .proj and other files inside '$NewProjectFolder'"

$OldProjectName=[IO.Path]::GetFileNameWithoutExtension($ProjectFilePath)
dir -Include "$OldProjectName.*" -Recurse | 
    ren -NewName {$_.Name -replace [regex]("^"+$OldProjectName+"\b"), $NewProjectName} #-WhatIf
ProceedOrExit

$NewProjectFile="$NewProjectName.csproj"
echo "----------"
echo "7. Rename project in tags AssemblyName and RootNamespace inside '$NewProjectFile'"

(Get-Content $NewProjectFile) |
    % { if ($_ -match ("<(?:AssemblyName|RootNamespace)>(" + $OldProjectName +")</")) { $_ -replace $($matches[1]), $NewProjectName } else { $_ }} |
    Set-Content $NewProjectFile
ProceedOrExit

echo "=========="
echo "DONE."
Read-Host -Prompt "Press Enter to exit"