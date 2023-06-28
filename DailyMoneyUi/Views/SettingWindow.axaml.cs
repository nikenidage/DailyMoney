using Avalonia.Controls;
using DailyMoneyUi.ViewModels;

namespace DailyMoneyUi.Views;

public partial class SettingWindow : Window
{
    public SettingWindow()
    {
        InitializeComponent();

        DataContext = new SettingWindowViewModel();
    }
}