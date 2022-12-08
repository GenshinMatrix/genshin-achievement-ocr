using System;
using System.Threading.Tasks;

namespace GenshinAchievementOcr.Core;

public class GenshinWindow
{
    protected IntPtr hwnd;
    public IntPtr Hwnd => hwnd;

    public GenshinWindow()
    {
    }

    public async Task<bool> HasHwnd()
    {
        return await LaunchCtrl.IsRunning(async p =>
        {
            hwnd = p?.MainWindowHandle ?? IntPtr.Zero;
        });
    }

    public void MouseLeftButtonDown()
    {
        NativeMethods.Focus(hwnd);
        User32.PostMessage(new(hwnd), NativeMethods.WM_LBUTTONDOWN, IntPtr.Zero, (IntPtr)((0 << 16) | 0));
    }

    public void MouseLeftButtonUp()
    {
        NativeMethods.Focus(hwnd);
        User32.PostMessage(new(hwnd), NativeMethods.WM_LBUTTONUP, IntPtr.Zero, (IntPtr)((0 << 16) | 0));
    }

    public async Task MouseClick(int x, int y)
    {
        NativeMethods.Focus(hwnd);
        User32.PostMessage(new(hwnd), NativeMethods.WM_LBUTTONDOWN, IntPtr.Zero, (IntPtr)((y << 16) | x));
        await Task.Delay(80);
        User32.PostMessage(new(hwnd), NativeMethods.WM_LBUTTONUP, IntPtr.Zero, (IntPtr)((y << 16) | x));
    }

    public void MouseWheelUp(int delta = 120)
    {
        NativeMethods.Focus(hwnd);
        User32.mouse_event(User32.MOUSEEVENTF.MOUSEEVENTF_WHEEL, 0, 0, delta, IntPtr.Zero);
    }

    public void MouseWheelDown(int delta = -120)
    {
        NativeMethods.Focus(hwnd);
        User32.mouse_event(User32.MOUSEEVENTF.MOUSEEVENTF_WHEEL, 0, 0, delta, IntPtr.Zero);
    }

    public void MouseWheelDownPage()
    {
        NativeMethods.Focus(hwnd);
        for (int i = default; i < 6; i++)
        {
            MouseWheelDown(-1);
        }
    }

    public void MouseMove(int x, int y)
    {
        NativeMethods.Focus(hwnd);
        _ = User32.GetWindowRect(hwnd, out RECT lpRect);
        _ = User32.SetCursorPos(lpRect.X + x, lpRect.Y + y);
    }
}
