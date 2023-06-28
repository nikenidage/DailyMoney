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
            var result = GetMoneyNumber();
            Text = string.Format(MoneyCalculateSettings.Instance.TextTemplate, result.ToString("F"));
        };
        timer.Start();
    }

    private double GetMoneyNumber()
    {
        var now = DateTime.Now;
        var settings = MoneyCalculateSettings.Instance;
        if (now.TimeOfDay < settings.StartTime)
            return 0;
        if (now.TimeOfDay > settings.EndTime)
            return settings.SalaryTotalSeconds * settings.SalaryPerSecond;

        var totalSeconds = 0D;
        if (settings.IsHaveLunchTime && settings.LunchStartTime.HasValue && settings.LunchEndTime.HasValue &&
            now.TimeOfDay > settings.LunchStartTime.Value &&
            now.TimeOfDay < settings.LunchEndTime.Value)
            totalSeconds = (settings.LunchStartTime.Value - settings.StartTime).TotalSeconds;
        else{
            totalSeconds = (now.TimeOfDay - settings.StartTime).TotalSeconds;
            if (settings.IsHaveLunchTime && settings.LunchStartTime.HasValue && settings.LunchEndTime.HasValue)
                totalSeconds -= (settings.LunchEndTime.Value - settings.LunchStartTime.Value).TotalSeconds;
        }
        return totalSeconds * settings.SalaryPerSecond;
    }
}