using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;

namespace GenshinAchievementOcr.Core;

internal static class ImageExtension
{
    public static ImageSource ToImageSource(this Bitmap bitmap)
    {
        IntPtr hBitmap = bitmap.GetHbitmap();
        ImageSource imageSource = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        _ = PInvoke.DeleteObject(new(hBitmap));
        return imageSource;
    }

    public static BitmapSource ToBitmapSource(this Bitmap bitmap)
    {
        try
        {
            return Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
        catch
        {
        }
        return null!;
    }

    public static Bitmap Capture(int x, int y, int w, int h, IntPtr? hwnd = null)
    {
        try
        {
            Bitmap copied = new(w, h);
            using Graphics g = Graphics.FromImage(copied);
            IntPtr hdcDest = g.GetHdc();
            IntPtr hdcSrc = PInvoke.GetDC(new(hwnd ?? PInvoke.GetDesktopWindow()));
            _ = PInvoke.StretchBlt(new HDC(hdcDest), 0, 0, w, h, new(hdcSrc), x, y, w, h, ROP_CODE.SRCCOPY);
            g.ReleaseHdc();
            _ = PInvoke.DeleteDC(new(hdcDest));
            _ = PInvoke.DeleteDC(new(hdcSrc));
            return copied;
        }
        catch
        {
        }
        return null!;
    }

    public static Bitmap Sharpen(this Bitmap self)
    {
        try
        {
            Bitmap newBitmap = new(self.Width, self.Height);
            int[] Laplacian = { -1, -1, -1, -1, 9, -1, -1, -1, -1 }; // 拉普拉斯模板
            for (int x = 1; x < self.Width - 1; x++)
                for (int y = 1; y < self.Height - 1; y++)
                {
                    int r = 0, g = 0, b = 0;
                    int Index = 0;
                    for (int col = -1; col <= 1; col++)
                        for (int row = -1; row <= 1; row++)
                        {
                            Color pixel = self.GetPixel(x + row, y + col); r += pixel.R * Laplacian[Index];
                            g += pixel.G * Laplacian[Index];
                            b += pixel.B * Laplacian[Index];
                            Index++;
                        }
                    r = r > 255 ? 255 : r;
                    r = r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g;
                    g = g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b;
                    b = b < 0 ? 0 : b;
                    newBitmap.SetPixel(x - 1, y - 1, Color.FromArgb(r, g, b));
                }
            return newBitmap;
        }
        catch (Exception ex)
        {
            Logger.Error(ex.ToString());
        }
        return self;
    }
}
