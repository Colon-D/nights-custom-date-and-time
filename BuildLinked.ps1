# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "$env:RELOADEDIIMODS/nights.test.sandbox/*" -Force -Recurse
dotnet publish "./nights.test.sandbox.csproj" -c Release -o "$env:RELOADEDIIMODS/nights.test.sandbox" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location