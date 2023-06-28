#!/bin/bash 
dir=${0%/*} 
if [ -d "$dir" ]; then 
  cd "$dir" 
fi 
rm -f DailyMoneyUi/packages.lock.json 
dotnet publish -r osx-x64 -c Release /p:RestoreLockedMode=true /p:AssemblyVersion=$1 -t:BundleApp
rm -rf DailyMoneyUi/bin/Release/net7.0/osx-x64/publish/Assets/
rm -rf DailyMoneyUi/bin/Release/net7.0/osx-x64/publish/DailyMoneyUi.app/Contents/MacOS/Assets/