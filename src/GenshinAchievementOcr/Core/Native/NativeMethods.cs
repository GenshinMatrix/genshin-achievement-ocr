using System;
using System.Runtime.InteropServices;
using Vanara.InteropServices;
using static Vanara.PInvoke.Kernel32;

namespace GenshinAchievementOcr.Core;

internal class NativeMethods
{
    public const int WM_NCHITTEST = 0x0084;
    public const int WM_SYSCOMMAND = 0x0112;
    public const int WM_HOTKEY = 0x0312;
    public const int WM_LBUTTONDOWN = 0x201;
    public const int WM_LBUTTONUP = 0x202;
    public const int WM_RBUTTONDBLCLK = 0x0206;
    public const int WM_MBUTTONDOWN = 0x0207;
    public const int WM_STYLECHANGING = 0x007C;
    public const int WM_SETCURSOR = 0x0020;
    public const int WM_NCLBUTTONDBLCLK = 0x00A3;

    public const int SC_RESTORE = 0xF120;
    public const int LWA_ALPHA = 2;

    public const int WS_EX_TOPMOST = 0x0008;
    public const int WS_EX_TRANSPARENT = 0x20;
    public const int WS_EX_LAYERED = 0x80000;
    public const int WS_EX_TOOLWINDOW = 0x00000080;

    public const int WS_MINIMIZEBOX = 0x00020000;
    public const int WS_MAXIMIZEBOX = 0x00010000;

    public const int HWND_TOPMOST = -1;
    public const int HWND_NOTOPMOST = -2;

    public static void Focus(IntPtr hwnd)
    {
        _ = User32.SendMessage(new(hwnd), WM_SYSCOMMAND, (IntPtr)SC_RESTORE, 0);
        _ = User32.SetForegroundWindow(new(hwnd));
        while (User32.IsIconic(new(hwnd)))
        {
            continue;
        }
    }

    public static int GetMouseSpeed()
    {
        _ = User32.SystemParametersInfo(User32.SPI.SPI_GETMOUSESPEED, out uint mouseSpeed);
        return (int)mouseSpeed;
    }

    public static void SetWindowRECT(IntPtr hwnd, RECT rect, bool topMost = false)
    {
        _ = User32.SetWindowPos(new(hwnd), new(topMost ? (IntPtr)HWND_TOPMOST : (IntPtr)HWND_NOTOPMOST), rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, 0);
    }

    public static void SetToolWindow(IntPtr hwnd)
    {
        int style = User32.GetWindowLong(hwnd, User32.WindowLongFlags.GWL_EXSTYLE);

        style |= WS_EX_TOOLWINDOW;
        _ = User32.SetWindowLong(hwnd, User32.WindowLongFlags.GWL_EXSTYLE, style);
    }

    public static void SetLayeredWindow(IntPtr hwnd, bool isLayered = true)
    {
        int style = User32.GetWindowLong(hwnd, User32.WindowLongFlags.GWL_EXSTYLE);

        if (isLayered)
        {
            style |= WS_EX_TRANSPARENT;
            style |= WS_EX_LAYERED;
        }
        else
        {
            style &= ~WS_EX_TRANSPARENT;
            style &= ~WS_EX_LAYERED;
        }
        _ = User32.SetWindowLong(hwnd, User32.WindowLongFlags.GWL_EXSTYLE, style);
    }
}
