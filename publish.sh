#!/bin/bash
rm -f DailyMoneyUi/packages.lock.json
dotnet publish -r linux-x64 -c Release /p:RestoreLockedMode=true /p:AssemblyVersion=$1
cd DailyMoneyUi/bin/Release/net7.0/linux-x64/publish 
cp DailyMoneyUi DailyMoneyUi.bin 
