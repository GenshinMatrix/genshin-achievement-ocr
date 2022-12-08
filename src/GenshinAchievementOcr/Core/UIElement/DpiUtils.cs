using System;
using System.Windows;

namespace GenshinAchievementOcr.Core;

public class DpiUtils
{
    public static DpiScale GetScale()
    {
        Gdi32.SafeHDC hdc = User32.GetDC(IntPtr.Zero);
        float scaleX = Gdi32.GetDeviceCaps(hdc, Gdi32.DeviceCap.LOGPIXELSX) / 96f;
        float scaleY = Gdi32.GetDeviceCaps(hdc, Gdi32.DeviceCap.LOGPIXELSY) / 96f;
        User32.ReleaseDC(new(IntPtr.Zero), hdc);
        return new(scaleX, scaleY);
    }

    public static double ScaleX => GetScale().DpiScaleX;
    public static double ScaleY => GetScale().DpiScaleY;

    public static double ScaleXReci => 1d / ScaleX;
    public static double ScaleYReci => 1d / ScaleY;
}
