using DailyMoneyUi.ViewModels;
using ReactiveUI;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DailyMoneyUi.Models;
public class MoneyCalculateSettings : ViewModelBase
{
    private string _backgroundColor = "";
    private string _textColor = "";
    private int _height = 100;
    private int _width = 300;
    private string _textTemplate = "我今天已经赚了 {0} RMB 👏";

    public static MoneyCalculateSettings Instance { get; set; }

    static MoneyCalculateSettings()
    {
        Load();
    }

    public static void Load()
    {
        var path = Path.Combine(PathHelper.SettingsFolder, "settings.json");
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            Instance = JsonSerializer.Deserialize<MoneyCalculateSettings>(json, AotMoneyCalculateSettingsJsonContext.Default.MoneyCalculateSettings) ?? new();
        }
        else
        {
            Instance = new();
        }
    }

    public static void Save()
    {
        var path = Path.Combine(PathHelper.SettingsFolder, "settings.json");
        var json = JsonSerializer.Serialize(Instance, AotMoneyCalculateSettingsJsonContext.Default.MoneyCalculateSettings);
        File.WriteAllText(path, json);
        
        StartWithSystemHelper.SetStartWithSystem(Instance.IsStartWithSystem);
    }

    public TimeSpan StartTime { get; set; } = new(8, 0, 0);
    public TimeSpan EndTime { get; set; } = new(17, 0, 0);
    public TimeSpan? LunchStartTime { get; set; }
    public TimeSpan? LunchEndTime { get; set; }
    public bool IsHaveLunchTime { get; set; }
    public int Salary { get; set; } = 20000;
    public float WorkDays { get; set; } = 21.5F;
    public int Width
    {
        get => _width;
        set => this.RaiseAndSetIfChanged(ref _width, value);
    }

    public int Height
    {
        get => _height;
        set => this.RaiseAndSetIfChanged(ref _height, value);
    }

    public string TextTemplate
    {
        get => _textTemplate;
        set => this.RaiseAndSetIfChanged(ref _textTemplate, value);
    }

    public bool IsStartWithSystem { get; set; }
    public string BackgroundColor
    {
        get => _backgroundColor;
        set => this.RaiseAndSetIfChanged(ref _backgroundColor, value);
    }

    public string TextColor
    {
        get => _textColor;
        set => this.RaiseAndSetIfChanged(ref _textColor, value);
    }

    [JsonIgnore]
    public double SalaryPerSecond
    {
        get
        {
            var secondsOfDays = (EndTime - StartTime).TotalSeconds;
            if (IsHaveLunchTime && LunchStartTime.HasValue && LunchEndTime.HasValue)
                secondsOfDays -= (LunchEndTime.Value - LunchStartTime.Value).TotalSeconds;
            if (WorkDays == 0 || secondsOfDays == 0)
                return 0;
            return Salary / WorkDays / secondsOfDays;
        }
    }
}
