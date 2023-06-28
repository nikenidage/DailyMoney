using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DailyMoneyUi.ViewModels;
using DailyMoneyUi.Views;

namespace DailyMoneyUi;

public partial class App : Application
{

    public static TrayIcon TrayIcon { get; } = new TrayIcon();

    public App()
    {
        DataContext = new ApplicationViewModel();
        
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
                ShowInTaskbar = false
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}