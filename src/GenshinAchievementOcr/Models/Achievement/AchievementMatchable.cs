using System;

namespace GenshinAchievementOcr.Models;

internal class AchievementMatchable
{
    public string Name { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
    public string[] Status { get; set; } = Array.Empty<string>();

    public override string ToString()
    {
        return $"{Name}|{Desc}|{string.Join("|", Status)}";
    }
}
