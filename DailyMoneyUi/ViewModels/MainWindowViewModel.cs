using Avalonia.Threading;
using DailyMoneyUi.Models;
using ReactiveUI;
using System;
using System.IO;
using System.Text.Json;
using System.Timers;

namespace DailyMoneyUi.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public string Text
    {
        get => _text;
        set => this.RaiseAndSetIfChanged(ref _text, value);
    }

    private DispatcherTimer timer;
    private string _text = "";

    public MainWindowViewModel()
    {
        timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(0.1)
        };
        timer.Tick += (sender, e) =>
        {
            var now = DateTime.Now;
            var number = ((now - new DateTime(now.Year,now.Month,now.Day)).TotalSeconds * MoneyCalculateSettings.Instance.SalaryPerSecond).ToString("F");
            Text = string.Format(MoneyCalculateSettings.Instance.TextTemplate, number);
        };
        timer.Start();

    }
}
