using GenshinAchievementOcr.Core;
using System;
using System.Windows.Forms;

namespace GenshinAchievementOcr.Models;

internal static class SettingsVisitor
{
    internal static SettingsContainer container = new();
    public static SettingsContainer Container => container;

    static SettingsVisitor()
    {
        Setup();
    }

    public static void Setup()
    {
        container.FromReg();
        Logger.Info($"[SystemInfo] OSVersion='{Environment.OSVersion.VersionString}'|PrimaryScreen={Screen.PrimaryScreen.Bounds.Width}x{Screen.PrimaryScreen.Bounds.Height}|DPI={DpiUtils.ScaleX * 100d}%|MouseSpeed={NativeMethods.GetMouseSpeed()}");
        Logger.Info($"[GenshinConfig] Resolution={Container.Resolution?.Width}x{Container.Resolution?.Height}|FullScreen={Container.Resolution?.FullScreen}");
        Logger.Info($"[GenshinConfig] TextLanguage={Container.Language?.TextLang}|VoiceLanguage={Container.Language?.VoiceLang}");
    }
}
