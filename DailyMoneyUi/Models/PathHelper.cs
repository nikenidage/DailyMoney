using System;

namespace DailyMoneyUi.Models;

public static class PathHelper
{
    static PathHelper()
    {
        if (!System.IO.Directory.Exists(SettingsFolder))
            System.IO.Directory.CreateDirectory(SettingsFolder);
    }
    
    public static string SettingsFolder
    {
        get
        {
#if OSX
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
                   "/Library/Application Support/com.DailyMoney.DailyMoneyUi/";
#else
            return Environment.CurrentDirectory;
#endif
        }
    }
}