using GenshinAchievementOcr.Core;
using System;

namespace GenshinAchievementOcr;

internal static class Pack
{
    public static string Name => "GenshinAchievementOcr";
    public static string Alias => "genshin-achievement-ocr";
    public static string Url => "https://github.com/genshin-matrix/genshin-achievement-ocr/releases";
    public static string Version => AssemblyUtils.GetAssemblyVersion(typeof(App).Assembly, prefix: "v", subfix: "rc1");

    public static string SupportedOSVersion => "Windows 10.0.18362.0";
    public static string CurrentOSVersion => $"Windows {Environment.OSVersion.Version.Major}.{Environment.OSVersion.Version.MajorRevision}.{Environment.OSVersion.Version.Build}.{Environment.OSVersion.Version.Minor}";
    public static bool IsSupported => Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 10 && Environment.OSVersion.Version.Build >= 18362;
}
