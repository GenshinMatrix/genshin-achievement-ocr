using System.Windows.Media;

namespace GenshinAchievementOcr.Core;

public static class BrushConverterX
{
    public static Brush ToBrush(this string colorString)
    {
        return (new BrushConverter().ConvertFromString(colorString) as Brush)!;
    }

    public static Color ToColor(this string colorString)
    {
        return (Color)ColorConverter.ConvertFromString(colorString);
    }

    public static Color ToColor(this Brush brush)
    {
        return ((SolidColorBrush)brush).Color;
    }
}
