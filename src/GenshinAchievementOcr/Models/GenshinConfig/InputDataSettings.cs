using Newtonsoft.Json;

namespace GenshinAchievementOcr.Models;

internal class InputDataSettings
{
    [JsonProperty("data")] public InputDataConfig? Data;
    [JsonIgnore] public double MouseSensitivity => Data!.MouseSensitivity;
    [JsonIgnore] public MouseFocusSenseIndex MouseFocusSenseIndex => (MouseFocusSenseIndex)Data!.MouseFocusSenseIndex;
    [JsonIgnore] public MouseFocusSenseIndex MouseFocusSenseIndexY => (MouseFocusSenseIndex)Data!.MouseFocusSenseIndexY;

    public InputDataSettings(GenshinConfig data)
    {
        Load(data);
    }

    public void Load(GenshinConfig data)
    {
        try
        {
            Data = JsonConvert.DeserializeObject<InputDataConfig>(data.InputData);
        }
        catch
        {
        }
    }
}

internal class InputDataConfig
{
    [JsonProperty("mouseSensitivity")] public double MouseSensitivity { get; set; }
    [JsonProperty("mouseFocusSenseIndex")] public int MouseFocusSenseIndex { get; set; }
    [JsonProperty("mouseFocusSenseIndexY")] public int MouseFocusSenseIndexY { get; set; }
}

public enum MouseFocusSenseIndex
{
    Level1,
    Level2,
    Level3,
    Level4,
    Level5,
}
