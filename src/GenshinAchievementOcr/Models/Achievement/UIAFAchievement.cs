using GenshinAchievementOcr.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GenshinAchievementOcr.Models;

/// <summary>
/// https://github.com/UIGF-org/UIGF-org.github.io/blob/f7edd354e4578b730c6a98c6db50b8fd00a94b37/docs/standards/UIAF.md
/// </summary>
public class UIAFData
{
    [JsonProperty("info")] public UIAFInfo Info { get; set; } = new();
    [JsonProperty("list")] public List<UIAFAchievement> List { get; set; } = new();
}

public class UIAFAchievement
{
    [JsonProperty("id")] public int Id { get; set; } = default;
    [JsonProperty("timestamp")] public long Timestamp { get; set; } = 253402271999;
    [JsonProperty("current")] public int Current { get; set; } = default;
}

public class UIAFInfo
{
    [JsonProperty("export_app")] public string ExportApp { get; set; } = Pack.Alias;
    [JsonProperty("export_app_version")] public string ExportAppVersion { get; set; } = Pack.Version;
    [JsonProperty("export_timestamp")] public long ExportTimestamp { get; set; } = DateTime.Now.ToTimeStamp();
    [JsonProperty("uiaf_version")] public string UIAFVersion { get; set; } = "v1.0";
}
