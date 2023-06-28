using System.Text.Json.Serialization;

namespace DailyMoneyUi.Models;

[JsonSerializable(typeof(MoneyCalculateSettings))]
public partial class AotMoneyCalculateSettingsJsonContext : JsonSerializerContext
{
}

[JsonSerializable(typeof(PositionLoader))]
public partial class AotPositionLoaderJsonContext : JsonSerializerContext
{
}