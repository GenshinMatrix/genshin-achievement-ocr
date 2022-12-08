using GenshinAchievementOcr.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Vanara.PInvoke;
using Windows.Media.Ocr;

namespace GenshinAchievementOcr.Core;

internal static class ImageRecognition
{
    private static GenshinWindow window = new();
    public static bool IsRunning { get; internal set; } = false;

    public static bool OutputCollect => Settings.ExportCapturedImages;
    public static bool OutputDebug => Settings.ExportDebugImages;
    public static OcrEngine OcrEngine { get; private set; } = null!;

    private static int count = 0;
    private const int x = 538; // 1440x900
    private const int y = 122; // 1440x900
    private const int spliteHeight = 183; // 1440x900
    private const int achieveWidth = 520; // 1440x900

    static ImageRecognition()
    {
        OcrEngine = OcrEngine.TryCreateFromUserProfileLanguages();
    }

    public static async Task<bool> HasHwnd()
    {
        return await window.HasHwnd();
    }

    public static bool GetFrameHeight(this Bitmap self, out int y1, out int y2)
    {
        BitmapData data = self.LockBits(new Rectangle(0, 0, self.Width, self.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

        int yprev = 0;
        (int h, int y1, int y2) max = (0, 0, 0);

        unsafe
        {
            byte* ptr = (byte*)data.Scan0;

            for (int y = 0; y < data.Height; y++)
            {
                for (int x = 0; x < data.Width; x++)
                {
                    byte b = *ptr;
                    byte g = *(ptr + 1);
                    byte r = *(ptr + 2);
                    ptr += 3;

                    Logger.Ignore($"[{y}] {r} {g} {b}");

                    if (y == 0)
                    {
                        yprev = 0;
                    }
                    else
                    {
                        if (r >= 225 - 10 && r <= 225 + 10
                         && g >= 215 - 10 && g <= 215 + 10
                         && b >= 200 - 10 && b <= 200 + 10)
                        {
                            if (y - yprev > max.h)
                            {
                                max.y1 = yprev;
                                max.y2 = y;
                                max.h = max.y2 - max.y1;
                            }
                            yprev = y;
                        }
                    }
                }
                ptr += data.Stride - data.Width * 3;
            }
        }
        self.UnlockBits(data);

        if (max.h > 0)
        {
            y1 = max.y1;
            y2 = max.y2;

            if (y2 - y1 <= 30d)
            {
                return false;
            }
            return true;
        }
        y1 = y2 = 0;
        return false;
    }

    public static async Task<string> RecUID()
    {
        if (!IsRunning || !await HasHwnd())
        {
            return null!;
        }
        NativeMethods.Focus(window.Hwnd);
        await Task.Delay(1000);
        using Bitmap frame = ImageCapture.Capture(1270, 900, 134, 20, window.Hwnd); // 1440x900

        if (OutputDebug) frame.SaveDebugImage("uid");

        using Bitmap frame2 = frame.ScaleToSize(frame.Width * 2, frame.Height * 2);
        using MemoryStream steam2 = new();
        frame2.Save(steam2, ImageFormat.Jpeg);
        OcrResult result = await OcrUtils.RecognizeAsync(steam2, OcrEngine);
        string uid = result.Lines.Count > 0 ? result.Lines.First().Text.ClearKanji().PickNumberString() : null!;
        return uid;
    }

    public static async Task<List<AchievementMatchScore>> RecScroll(bool isFixedMode = false)
    {
        List<AchievementMatchScore> scores = new();
        int countDuplicated = 0;
        int offsetY = 0;

        await Task.Run(async () =>
        {
            if (!isFixedMode) count = 0;

            do
            {
                if (!IsRunning || !await HasHwnd())
                {
                    break;
                }
                if (!isFixedMode)
                {
                    window.MouseMove(x, y);
                }

                AchievementMatchable matchable = new();

                int y1, y2;

                try
                {
                    using Bitmap frame = ImageCapture.Capture(x + 20, y + offsetY, 1, spliteHeight, window.Hwnd); // 1440x900

                    if (OutputDebug) frame.SaveDebugImage($"frame{count}");
                    if (!frame.GetFrameHeight(out y1, out y2))
                    {
                        Logger.Ignore($"[GetFrameHeight|Exit] count={count}, y1={y1}, y2={y2}");
                        break;
                    }
                    if (OutputCollect)
                    {
                        using Bitmap frame2 = ImageCapture.Capture(x + 20, y + offsetY + y1, 800, y2 - y1, window.Hwnd); // 1440x900
                        frame2.SaveCollectImage($"collect{count}");
                    }
                    Logger.Ignore($"[GetFrameHeight] count={count}, y1={y1}, y2={y2}");
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.ToString());
                    break;
                }

                try
                {
                    using Bitmap achieve = ImageCapture.Capture(x + 110, y + offsetY + y1, achieveWidth, y2 - y1, window.Hwnd); // 1440x900
                    using Bitmap achieve2 = achieve.ScaleToSize(achieve.Width * 2, achieve.Height * 2);
                    using MemoryStream steam2 = new();

                    if (OutputDebug) achieve2.SaveDebugImage($"ocr{count}");
                    achieve2.Save(steam2, ImageFormat.Jpeg);
                    OcrResult result = await OcrUtils.RecognizeAsync(steam2, OcrEngine);
                    (string title, string description) = result.ParseToAchievement();

                    matchable.Name = title;
                    matchable.Desc = description;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.ToString());
                    break;
                }

                try
                {
                    using Bitmap status = ImageCapture.Capture(x + 110 + 520 + 90, y + offsetY + y1, 90, y2 - y1, window.Hwnd); // 1440x900
                    using Bitmap status2 = status.ScaleToSize(status.Width * 2, status.Height * 2);
                    using MemoryStream steam2 = new();

                    if (OutputDebug) status2.SaveDebugImage($"ocrr{count}");
                    status2.Save(steam2, ImageFormat.Jpeg);
                    OcrResult result = await OcrUtils.RecognizeAsync(steam2, OcrEngine);
                    matchable.Status = result.ParseToAchievementStatus();

                    if (matchable.Status.Length <= 0)
                    {
                        using Bitmap status4 = status.ScaleToSize(status.Width * 4, status.Height * 4);
                        using MemoryStream steam4 = new();

                        if (OutputDebug) status4.SaveDebugImage($"ocrrr{count}");
                        status4.Save(steam4, ImageFormat.Jpeg);
                        OcrResult result4 = await OcrUtils.RecognizeAsync(steam4, OcrEngine);
                        matchable.Status = result4.ParseToAchievementStatus();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.ToString());
                    break;
                }
                count++;
                AchievementMatchScore score = matchable.GetScore();
                if (score.IsMatched && scores.Count > 0 && scores.Last().Matched?.Id == score.Matched?.Id)
                {
                    countDuplicated++;
                }
                else
                {
                    Logger.Info(score!.ToString());
                    scores.Add(score!);
                    countDuplicated = 0;
                }
                if (countDuplicated >= 3)
                {
                    break;
                }
                if (!isFixedMode)
                {
                    window.MouseMove(x, y);
                    window.MouseWheelDownPage();
                }
                else
                {
                    offsetY += spliteHeight / 2;

                    _ = User32.GetWindowRect(new(window.Hwnd), out RECT rect);
                    if (offsetY > rect.bottom)
                    {
                        break;
                    }
                }
                await Task.Delay(50);
            }
            while (true);
        });

        if (!isFixedMode && countDuplicated >= 3)
        {
            List<AchievementMatchScore> @fixed = await RecFixed();
            scores.AddRange(@fixed);
        }
        return scores;
    }

    public static async Task<List<AchievementMatchScore>> RecFixed()
    {
        List<AchievementMatchScore> scores = await RecScroll(true);
        return scores;
    }
}
