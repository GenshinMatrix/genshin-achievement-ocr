using GenshinAchievementOcr.Core;
using Microsoft.Win32;
using System;

namespace GenshinAchievementOcr.Models;

internal class GenshinRegistry
{
    public static RegistryKey GetRegistryKey()
    {
        try
        {
            using RegistryKey hkcu = Registry.CurrentUser;
            if (hkcu.OpenSubKey(@"SOFTWARE\miHoYo\原神", true) is RegistryKey sk)
            {
                return sk;
            }
            else if (hkcu.OpenSubKey(@"SOFTWARE\miHoYo\Genshin Impact", true) is RegistryKey sk2)
            {
                return sk2;
            }
        }
        catch (Exception e)
        {
            Logger.Warn(e.ToString());
        }
        return null!;
    }
}
