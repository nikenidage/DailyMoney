using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform;
using DailyMoneyUi.Models;
using DailyMoneyUi.Views;
using ReactiveUI;

namespace DailyMoneyUi;

public class CommonCommand
{
    public static List<MenuItem> MenuItems;
    public static CommonCommand Instance;
    public static List<NativeMenuItem> NativeMenuItems;

    static CommonCommand()
    {
        Instance = new();
        MenuItems = new List<MenuItem>()
        {
            new MenuItem() { Header = "设置", Command = ReactiveCommand.Create(Instance.ShowSettingWindow) },
            new MenuItem() { Header = "我要提建议 ", Command = ReactiveCommand.Create(Instance.Suggest) },
            new MenuItem() { Header = "退出", Command = ReactiveCommand.Create(Instance.ExitApplication) }
        };
        NativeMenuItems =
            MenuItems.ConvertAll<NativeMenuItem>(i => new NativeMenuItem()
            { Header = i.Header.ToString(), Command = i.Command });
    }

    private void Suggest()
    {
        OpenBrowser("https://jihulab.com/nikenidage/DailyMoney/-/issues");
    }

    private SettingWindow settingWindow;


    public static void OpenBrowser(string url)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            // If no associated application/json MimeType is found xdg-open opens retrun error
            // but it tries to open it anyway using the console editor (nano, vim, other..)
            ShellExec($"xdg-open {url}", waitForExit: false);
        }
        else
        {
            using Process process = Process.Start(new ProcessStartInfo
            {
                FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? url : "open",
                Arguments = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? $"{url}" : "",
                CreateNoWindow = true,
                UseShellExecute = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            });
        }
    }

    private static void ShellExec(string cmd, bool waitForExit = true)
    {
        var escapedArgs = cmd.Replace("\"", "\\\"");

        using var process = Process.Start(
            new ProcessStartInfo
            {
                FileName = "/bin/sh",
                Arguments = $"-c \"{escapedArgs}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            }
        );
        if (waitForExit)
        {
            process.WaitForExit();
        }
    }
    private void ShowSettingWindow()
    {
        //设置默认背景色和文字颜色
        if (string.IsNullOrWhiteSpace(MoneyCalculateSettings.Instance.BackgroundColor) &&
            string.IsNullOrWhiteSpace(MoneyCalculateSettings.Instance.TextColor))
        {
            var settings = AvaloniaLocator.Current.GetRequiredService<IPlatformSettings>();
            var isDark = settings.GetColorValues().ThemeVariant == PlatformThemeVariant.Dark;
            MoneyCalculateSettings.Instance.BackgroundColor = isDark ? "Black" : "White";
            MoneyCalculateSettings.Instance.TextColor = isDark ? "White" : "Black";
        }

        if (settingWindow == null)
        {
            settingWindow = new SettingWindow();
            settingWindow.Closing += (sender, args) =>
            {
                settingWindow.Hide();
                args.Cancel = true;
            };
        }

        if (!settingWindow.IsVisible)
            settingWindow.Show();
    }

    private void ExitApplication()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
        {
            lifetime.Shutdown();
        }
    }
}
