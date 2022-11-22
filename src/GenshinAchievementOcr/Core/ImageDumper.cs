using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace GenshinAchievementOcr.Core;

internal static class ImageDumper
{
    public static void SaveDebugImage(this Bitmap bitmap, string fileName, ImageFormat? format = null!)
    {
        SaveImage(bitmap, @$".\debug\{fileName}", format);
    }

    public static void SaveCollectImage(this Bitmap bitmap, string fileName, ImageFormat? format = null!)
    {
        SaveImage(bitmap, fileName, format);
    }

    private static void SaveImage(this Bitmap bitmap, string fileName, ImageFormat? format = null!)
    {
        try
        {
            ImageFormat iformat = format ?? ImageFormat.Jpeg;
            string ext = iformat.ToString() switch
            {
                nameof(ImageFormat.Bmp) => ".bmp",
                nameof(ImageFormat.Png) => ".png",
                nameof(ImageFormat.Tiff) => ".tiff",
                nameof(ImageFormat.Jpeg) => ".jpg",
                _ => throw new FileFormatException(nameof(format)),
            };

            string path = SpecialPathProvider.GetPath(@$".\ocr\{fileName}{ext}");

            if (File.Exists(path)) File.Delete(path);
            bitmap.Save(path, format ?? ImageFormat.Jpeg);
        }
        catch (Exception e)
        {
            Logger.Warn(e);
        }
    }
}
