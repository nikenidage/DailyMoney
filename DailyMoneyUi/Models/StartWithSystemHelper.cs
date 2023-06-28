using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace DailyMoneyUi.Models;

public class StartWithSystemHelper
{

    public static void SetStartWithSystem(bool setOrRemove)
    {
#if OSX
        SetStartupWithMac(setOrRemove);
#elif Windows
        SetStartupWithWindows(setOrRemove);
#endif
    }

    private static void SetStartupWithMac(bool setOrRemove)
    {
        if (setOrRemove)
        {
            SetLaunchAgent();
        }
        else
        {
            RemoveLaunchAgent();
        }
    }

    internal static void SetStartupWithWindows(bool setOrRemove)
    {
        using var reg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
        if (setOrRemove)
        {
            reg.SetValue("DailyMoney", Process.GetCurrentProcess().MainModule.FileName);
        }
        else
        {
            reg.DeleteValue("DailyMoney", false);
        }
    }
    
    // <summary>
    /// Sets launch agent to launch app on login.
    /// </summary>
    internal static void SetLaunchAgent()
    {
        string ap = GetAgentPath();
        var exePath = Environment.ProcessPath;
        string agentXmlContent =
            $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
    <key>Label</key>
    <string>com.DailyMoney.DailyMoneyUi.plist</string>
    <key>LimitLoadToSessionType</key>
    <string>Aqua</string>
    <key>Program</key>
    <string>{exePath}</string>
    <key>RunAtLoad</key>
    <true/>
</dict>
</plist>
";
        Console.WriteLine("Writing agent to: " + ap);
        File.WriteAllText(ap, agentXmlContent);
    }

    /// <summary>
    /// Removes launch agent to not start app on login.
    /// </summary>
    internal static void RemoveLaunchAgent()
    {
        string ap = GetAgentPath();
        if (File.Exists(ap))
        {
            Console.WriteLine("Deleting agent at: " + ap);
            File.Delete(ap);
        }
    }
    
    /// <summary>
    /// Gets path of launch agent. Creates the folder if it does not exist.
    /// </summary>
    private static string GetAgentPath()
    {
        string agentFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Library/LaunchAgents/";

        if (!Directory.Exists(agentFolder))
        {
            Directory.CreateDirectory(agentFolder);
        }

        return agentFolder + "com.DailyMoney.DailyMoneyUi.plist";
    }
}