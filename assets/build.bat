@echo off

.nuget\NuGet.exe install .nuget\packages.config -OutputDirectory packages

powershell.exe -NoProfile -ExecutionPolicy unrestricted -Command "& {Import-Module '.\packages\psake.4.4.1\tools\psake.psm1'; invoke-psake '.\assets\psake-project.ps1' %*; if ($LastExitCode -and $LastExitCode -ne 0) {write-host "ERROR CODE: $LastExitCode" -fore RED; exit $lastexitcode} }"
