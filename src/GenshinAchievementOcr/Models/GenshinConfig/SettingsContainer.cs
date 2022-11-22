using GenshinAchievementOcr.Core;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Text;

namespace GenshinAchievementOcr.Models;

internal class SettingsContainer
{
    protected GenshinConfig? data;
    public LanguageSettings? Language;
    public ResolutionSettings? Resolution;
    public InputDataSettings? InputData;

    public SettingsContainer()
    {
    }

    public void Parse(string raw)
    {
        try
        {
            data = JsonConvert.DeserializeObject<GenshinConfig>(raw);
            Language = new LanguageSettings(data);
            Resolution = new ResolutionSettings();
            InputData = new InputDataSettings(data);
        }
        catch
        {
        }
    }

    public void FromReg()
    {
        Parse(RegistryContainer.Load());
    }
}

internal class RegistryContainer
{
    public static string Load()
    {
        try
        {
            using RegistryKey hk = GenshinRegistry.GetRegistryKey();
            string valueName = SearchName(hk);

            return Encoding.UTF8.GetString((byte[])hk.GetValue(valueName)!);
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
        return string.Empty;
    }

    public static void Unload(string raw)
    {
        try
        {
            using RegistryKey hk = GenshinRegistry.GetRegistryKey();
            byte[] bytes = Encoding.UTF8.GetBytes(raw);
            string valueName = SearchName(hk);
            
            hk.SetValue(valueName, bytes);
            hk.Flush();
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
    }

    private static string SearchName(RegistryKey key)
    {
        string valueName = string.Empty;
        string[] names = key.GetValueNames();

        foreach (string name in names)
        {
            if (name.Contains("GENERAL_DATA"))
            {
                valueName = name;
                break;
            }
        }
        if (valueName == string.Empty)
        {
            throw new ArgumentException(valueName);
        }
        return valueName;
    }
}
