using Microsoft.Win32;
using System;

namespace GenshinAchievementOcr.Models;

internal class ResolutionSettings
{
    protected string? heightName;
    protected int height;
    public int Height => height;

    protected string? widthName;
    protected int width;
    public int Width => width;

    protected string? fullscreenName;
    protected int fullscreen;
    public bool FullScreen => fullscreen == 1;

    public ResolutionSettings()
    {
        using RegistryKey hk = GenshinRegistry.GetRegistryKey();
        string[] names = hk.GetValueNames();

        foreach (string name in names)
        {
            if (name.Contains("Width"))
            {
                widthName = name;
            }
            if (name.Contains("Height"))
            {
                heightName = name;
            }
            if (name.Contains("Fullscreen"))
            {
                fullscreenName = name;
            }
        }
        Read();
    }

    private void Read()
    {
        using RegistryKey hk = GenshinRegistry.GetRegistryKey();

        height = Convert.ToInt32(hk.GetValue(heightName));
        width = Convert.ToInt32(hk.GetValue(widthName));
        fullscreen = Convert.ToInt32(hk.GetValue(fullscreenName));
    }
}
