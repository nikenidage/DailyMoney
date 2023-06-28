using System;
using System.IO;
using System.Text.Json;
using System.Xml.Linq;

namespace DailyMoneyUi.Models;

public class PositionLoader
{
    public int X { get; set; }
    public int Y { get; set; }
    public static PositionLoader Instance { get; set; }

    static PositionLoader()
    {
        var path = Path.Combine(PathHelper.SettingsFolder, "position.json");
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            Instance = JsonSerializer.Deserialize<PositionLoader>(json, AotPositionLoaderJsonContext.Default.PositionLoader) ?? new();
        }
    }

    public static void Save(int x, int y)
    {
        if (Instance == null) Instance = new();
        Instance.X = x;
        Instance.Y = y;
        var path = Path.Combine(PathHelper.SettingsFolder, "position.json");
        var json = JsonSerializer.Serialize(Instance, AotPositionLoaderJsonContext.Default.PositionLoader);
        File.WriteAllText(path,json);
    }
}