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
        PInvoke.PostMessage(new(hwnd), NativeMethods.WM_LBUTTONDOWN, 0, (0 << 16) | 0);
    }

    public void MouseLeftButtonUp()
    {
        NativeMethods.Focus(hwnd);
        PInvoke.PostMessage(new(hwnd), NativeMethods.WM_LBUTTONUP, 0, (0 << 16) | 0);
    }

    public async Task MouseClick(int x, int y)
    {
        NativeMethods.Focus(hwnd);
        PInvoke.PostMessage(new(hwnd), NativeMethods.WM_LBUTTONDOWN, 0, (y << 16) | x);
        await Task.Delay(80);
        PInvoke.PostMessage(new(hwnd), NativeMethods.WM_LBUTTONUP, 0, (y << 16) | x);
    }

    public void MouseWheelUp(int delta = 120)
    {
        NativeMethods.Focus(hwnd);
        PInvoke.mouse_event(MOUSE_EVENT_FLAGS.MOUSEEVENTF_WHEEL, 0, 0, (uint)delta, 0);
    }

    public void MouseWheelDown(int delta = -120)
    {
        NativeMethods.Focus(hwnd);
        NativeMethods.mouse_event(MOUSE_EVENT_FLAGS.MOUSEEVENTF_WHEEL, 0, 0, delta, 0);
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
        _ = PInvoke.GetWindowRect(new(hwnd), out RECT lpRect);
        _ = PInvoke.SetCursorPos(lpRect.X + x, lpRect.Y + y);
    }
}
