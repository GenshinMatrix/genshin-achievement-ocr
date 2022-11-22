using GenshinAchievementOcr.Core;
using System.Reflection;

namespace GenshinAchievementOcr.Models;

[Obfuscation]
public class Settings
{
    public static SettingsDefinition<string> ShortcutKey { get; } = new(nameof(ShortcutKey), "F11");
    public static SettingsDefinition<string> Language { get; } = new(nameof(Language), string.Empty);
    public static SettingsDefinition<bool> ExportCapturedImages { get; } = new(nameof(ExportCapturedImages), false);
    public static SettingsDefinition<bool> ExportDebugImages { get; } = new(nameof(ExportDebugImages), false);
}
