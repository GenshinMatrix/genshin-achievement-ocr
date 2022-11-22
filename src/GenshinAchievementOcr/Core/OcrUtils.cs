using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage;
using Windows.Storage.Streams;
using BitmapDecoder = Windows.Graphics.Imaging.BitmapDecoder;

namespace GenshinAchievementOcr.Core;

public static class OcrUtils
{
    public static async Task<OcrResult> RecognizeAsync(string path, OcrEngine engine)
    {
        StorageFile storageFile = await StorageFile.GetFileFromPathAsync(Path.GetFullPath(path));
        using IRandomAccessStream randomAccessStream = await storageFile.OpenReadAsync();
        BitmapDecoder decoder = await BitmapDecoder.CreateAsync(randomAccessStream);
        using SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
        OcrResult ocrResult = await engine.RecognizeAsync(softwareBitmap);
        return ocrResult;
    }

    public static async Task<OcrResult> RecognizeAsync(Stream stream, OcrEngine engine)
    {
        using IRandomAccessStream randomAccessStream = stream.AsRandomAccessStream();
        BitmapDecoder decoder = await BitmapDecoder.CreateAsync(randomAccessStream);
        using SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
        OcrResult ocrResult = await engine.RecognizeAsync(softwareBitmap);
        return ocrResult;
    }

    public static string ParseToString(this OcrResult result)
    {
        StringBuilder sb = new();
        foreach (OcrLine line in result.Lines)
        {
            foreach (var word in line.Words)
            {
                sb.Append(word.Text);
            }
        }
        return sb.ToString();
    }
}
