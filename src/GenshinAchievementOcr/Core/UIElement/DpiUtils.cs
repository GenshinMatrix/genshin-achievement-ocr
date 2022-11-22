using System;
using System.Windows;

namespace GenshinAchievementOcr.Core;

public class DpiUtils
{
    public static DpiScale GetScale()
    {
        IntPtr hdc = PInvoke.GetDC(new(IntPtr.Zero));
        float scaleX = PInvoke.GetDeviceCaps(new HDC(hdc), GET_DEVICE_CAPS_INDEX.LOGPIXELSX) / 96f;
        float scaleY = PInvoke.GetDeviceCaps(new HDC(hdc), GET_DEVICE_CAPS_INDEX.LOGPIXELSY) / 96f;
        PInvoke.ReleaseDC(new(IntPtr.Zero), new(hdc));
        return new(scaleX, scaleY);
    }

    public static double ScaleX => GetScale().DpiScaleX;
    public static double ScaleY => GetScale().DpiScaleY;

    public static double ScaleXReci => 1d / ScaleX;
    public static double ScaleYReci => 1d / ScaleY;
}
