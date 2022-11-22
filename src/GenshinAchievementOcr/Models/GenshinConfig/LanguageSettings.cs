namespace GenshinAchievementOcr.Models;

internal class LanguageSettings
{
    protected TextLanguage textLang;
    public TextLanguage TextLang => textLang;

    protected VoiceLanguage voiceLang;
    public VoiceLanguage VoiceLang => voiceLang;

    public LanguageSettings(GenshinConfig data)
    {
        Load(data);
    }

    public void Load(GenshinConfig data)
    {
        textLang = (TextLanguage)data.DeviceLanguageType;
        voiceLang = (VoiceLanguage)data.DeviceVoiceLanguageType;
    }
}

public enum VoiceLanguage
{
    Chinese,
    English,
    Japanese,
    Korean
}

public enum TextLanguage
{
    None,
    English,
    SimplifiedChinese,
    TraditionalChinese,
    French,
    German,
    Spanish,
    Portugese,
    Russian,
    Japanese,
    Korean,
    Thai,
    Vietnamese,
    Indonesian
}
