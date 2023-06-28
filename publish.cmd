set version=%1
del DailyMoneyUi\packages.lock.json
dotnet publish -r win-x64 -c Release /p:RestoreLockedMode=true /p:AssemblyVersion=%version%