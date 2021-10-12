<#
PowerShell script to rename C# Project step-by-step: 
* Copying project folder to folder with new project name
* Renaming .csproj file and other files with project name
* Changing project name reference in .sln solution file
* Changing RootNamespace and AssemblyName in .csproj file
* Renaming project inside AssemblyInfo.cs
#>
param(
    [parameter(
        HelpMessage="New project file name, without extension.",
        Mandatory=$true)]
    [string]$NewProjectName
)

$initialDir = Get-Location

$SolutionFolder = "$($initialDir)\.."
$SolutionFile = Get-ChildItem -Path $SolutionFolder -Filter *.sln |Select -First 1
$SolutionFilePath = "$($SolutionFolder)\$SolutionFile"
$SolutionName = [IO.Path]::GetFileNameWithoutExtension($SolutionFilePath)

((Get-Content -path $SolutionFilePath -Raw) -replace $SolutionName, $NewProjectName) | Set-Content -Path $SolutionFilePath
Rename-Item $SolutionFilePath "$NewProjectName.sln"

$FilesToModify = dir .\ -recurse | where {$_.extension -in ".cs",".csproj",".json"}
foreach ($file in $FilesToModify)
{
    (Get-Content $file.PSPath) |
    Foreach-Object { $_ -replace $SolutionName, $NewProjectName } |
    Set-Content $file.PSPath
}

$ProjectFolderNames = Get-ChildItem -Directory
Foreach ($ProjectFolderName IN $ProjectFolderNames)
{
	$ProjectFilePath = "$($initialDir)\$ProjectFolderName\$ProjectFolderName.csproj"
	$ProjectFolderPath = Split-Path $ProjectFilePath
	$Suffix = "$ProjectFolderName".Substring("$ProjectFolderName".IndexOf("."))
	$NewProjectNameWithSuffix = "$NewProjectName$Suffix"
	$NewProjectFolderPath = "$($initialDir)\$NewProjectNameWithSuffix"
	
	Rename-Item $ProjectFolderPath $NewProjectFolderPath
	cd $NewProjectFolderPath
	$OldProjectName = [IO.Path]::GetFileNameWithoutExtension($ProjectFilePath)
	dir -Include "$OldProjectName.*" -Recurse | 
		ren -NewName {$_.Name -replace [regex]("^"+$OldProjectName+"\b"), $NewProjectNameWithSuffix} #-WhatIf
	
	$NewProjectFile = "$NewProjectNameWithSuffix.csproj"
	
	(Get-Content $NewProjectFile) |
		% { if ($_ -match ("<(?:AssemblyName|RootNamespace)>(" + $OldProjectName +")</")) { $_ -replace $($matches[1]), $NewProjectNameWithSuffix } else { $_ }} |
		Set-Content $NewProjectFile
}
Read-Host -Prompt "Press Enter to exit"