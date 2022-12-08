using System;
using System.Drawing;
using System.Windows;

namespace GenshinAchievementOcr.Core;

internal static class ImageCapture
{
    public static bool IsFullScreen { get; private set; } = false;
    public static bool IsFullScreenMode(IntPtr hwnd)
    {
        int exStyle = User32.GetWindowLong(hwnd, User32.WindowLongFlags.GWL_EXSTYLE);

        if ((exStyle & NativeMethods.WS_EX_TOPMOST) != 0)
        {
            return IsFullScreen = true;
        }
        return IsFullScreen = false;
    }

    private static int GetCaptionHeight(IntPtr? hwnd = null)
    {
        int captionHeight = default;

        if (hwnd != null && hwnd != IntPtr.Zero)
        {
            if (!IsFullScreenMode(hwnd.Value))
            {
                captionHeight = (int)(SystemParameters.CaptionHeight * DpiUtils.ScaleY);
            }
        }
        return captionHeight;
    }

    public static Bitmap Capture(int x, int y, int w, int h, IntPtr? hwnd = null)
    {
        return ImageExtension.Capture(x, y - GetCaptionHeight(hwnd), w, h, hwnd);
    }
}
