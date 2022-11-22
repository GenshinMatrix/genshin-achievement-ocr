using Newtonsoft.Json;

namespace GenshinAchievementOcr.Models;

public class GenshinConfig
{
    public const string DeviceLanguageTypeKey = "deviceLanguageType";
    [JsonProperty(DeviceLanguageTypeKey)] public int DeviceLanguageType { get; set; }
    [JsonProperty("deviceVoiceLanguageType")] public int DeviceVoiceLanguageType { get; set; }
    [JsonProperty("inputData")] public string? InputData { get; set; }
}
