using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using DailyMoneyUi.Models;
using NetSparkleUpdater;
using NetSparkleUpdater.Enums;
using NetSparkleUpdater.SignatureVerifiers;

namespace DailyMoneyUi.Views;

public partial class MainWindow : Window
{
    private SparkleUpdater _sparkle;
    public MainWindow()
    {
        InitializeComponent();

        if (PositionLoader.Instance != null)
        {
            Position = new PixelPoint(PositionLoader.Instance.X, PositionLoader.Instance.Y);
        }

        this.ContextMenu = new();
        foreach (var menuItem in CommonCommand.MenuItems)
        {
            ContextMenu.Items.Add(menuItem);
        }

        // Init Tray
        App.TrayIcon.IsVisible = true;
        App.TrayIcon.ToolTipText = "DailyMoney";
        App.TrayIcon.Icon = Icon;

        App.TrayIcon.Menu = new NativeMenu();
        CommonCommand.NativeMenuItems.ForEach(i => App.TrayIcon.Menu.Items.Add(i));
        
        this.Topmost = true;
        this.SystemDecorations = SystemDecorations.None;
#if OSX
        var appcastUrl = "https://jihulab.com/api/v4/projects/117941/packages/generic/release/xml/appcast-macos.xml";
#elif Windows
        var appcastUrl = "https://jihulab.com/api/v4/projects/117941/packages/generic/release/xml/appcast-windows.xml";
#else
        var appcastUrl = "https://jihulab.com/api/v4/projects/117941/packages/generic/release/xml/appcast-linux.xml";
#endif
        // set icon in project properties!
        _sparkle = new CustomSparkleUpdater(appcastUrl, new Ed25519Checker(SecurityMode.Unsafe, ""))
        {
            UIFactory = new NetSparkleUpdater.UI.Avalonia.UIFactory(Icon),
            // Avalonia version doesn't support separate threads: https://github.com/AvaloniaUI/Avalonia/issues/3434#issuecomment-573446972
            ShowsUIOnMainThread = true,
            LogWriter = new LogWriter(true),
            RelaunchAfterUpdate = true
            //UseNotificationToast = false // Avalonia version doesn't yet support notification toast messages
        };
        // TLS 1.2 required by GitHub (https://developer.github.com/changes/2018-02-01-weak-crypto-removal-notice/)
        _sparkle.SecurityProtocolType = System.Net.SecurityProtocolType.Tls12;
        _sparkle.StartLoop(true, true, TimeSpan.FromDays(10));
    }

    private bool _mouseDownForWindowMoving = false;
    private PointerPoint _originalPoint;

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        if (WindowState == WindowState.Maximized || WindowState == WindowState.FullScreen) return;
        var lastPointer = e.GetCurrentPoint(this);
        var lastProperties = lastPointer.Properties;
        if (lastProperties.IsRightButtonPressed) return; ;

        if (ContextMenu != null && ContextMenu.IsOpen)
            ContextMenu.Close();

        _mouseDownForWindowMoving = true;
        _originalPoint = e.GetCurrentPoint(this);

        base.OnPointerPressed(e);
    }
    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        _mouseDownForWindowMoving = false;
        base.OnPointerReleased(e);
    }
    protected override void OnPointerMoved(PointerEventArgs e)
    {
        if (!_mouseDownForWindowMoving) return;

        PointerPoint currentPoint = e.GetCurrentPoint(this);
        Position = new PixelPoint(Position.X + (int)(currentPoint.Position.X - _originalPoint.Position.X),
            Position.Y + (int)(currentPoint.Position.Y - _originalPoint.Position.Y));
        
        //save position
        PositionLoader.Save(Position.X, Position.Y);

        base.OnPointerMoved(e);
    }
}