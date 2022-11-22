using GenshinAchievementOcr.Core;
using Newtonsoft.Json;
using System.Linq;
using System.Text;

namespace GenshinAchievementOcr.Models;

internal static class AchievementRepository
{
    public static AchievementRepositoryMeta Meta { get; }

    static AchievementRepository()
    {
        byte[] wonders_of_the_world = ResourceUtils.GetBytes($"pack://application:,,,/{Pack.Name};component/Resources/Achievements/english/wonders_of_the_world.json");
        Meta = JsonConvert.DeserializeObject<AchievementRepositoryMeta>(Encoding.UTF8.GetString(wonders_of_the_world));
    }

    public static AchievementRepositoryMetaItem GetById(int id)
    {
        return Meta.Achievements.Where(item => item.Id == id).ToArray().First();
    }
}

internal class AchievementRepositoryMeta
{
    [JsonProperty("_id")] public int _Id { get; set; } = default;
    [JsonProperty("id")] public string Id { get; set; } = null!;
    [JsonProperty("name")] public string Name { get; set; } = null!;
    [JsonProperty("order")] public int Order { get; set; } = default;
    [JsonProperty("achievements")] public AchievementRepositoryMetaItem[] Achievements { get; set; } = null!;
}

internal class AchievementRepositoryMetaItem
{
    [JsonProperty("id")] public int Id { get; set; } = default;
    [JsonProperty("name")] public string Name { get; set; } = null!;
    [JsonProperty("desc")] public string Desc { get; set; } = null!;
    [JsonProperty("reward")] public int Reward { get; set; } = default;
    [JsonProperty("hidden")] public bool Hidden { get; set; } = default;
    [JsonProperty("order")] public int Order { get; set; } = default;
}
