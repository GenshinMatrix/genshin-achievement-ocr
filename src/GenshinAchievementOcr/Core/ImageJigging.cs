using CommunityToolkit.Mvvm.Messaging;
using GenshinAchievementOcr.Models;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace GenshinAchievementOcr.Core;

internal class ImageJigging
{
    public async Task StartReg()
    {
        ImageRecognition.IsRunning = true;
        await Action();
    }

    public void StopReg()
    {
        ImageRecognition.IsRunning = false;
    }

    public async Task Action()
    {
        await Task.Delay(300);
        if (!await ImageRecognition.HasHwnd())
        {
            WeakReferenceMessenger.Default.Send(new RunningMessage() { Indicate = RunningIndicate.Completed });
            NoticeService.AddNotice(Mui("Tips"), Mui("GameNotLaunchedTips"), string.Empty, ToastDuration.Short);
            return;
        }
        SettingsVisitor.Setup();
        if (SettingsVisitor.Container.Resolution!.Width != 1440
         || SettingsVisitor.Container.Resolution!.Height != 900
         || SettingsVisitor.Container.Language!.TextLang != TextLanguage.English)
        {
            WeakReferenceMessenger.Default.Send(new RunningMessage() { Indicate = RunningIndicate.Completed });
            NoticeService.AddNotice(Mui("Tips"), Mui("GameConfigNotMatchedTips", $"{SettingsVisitor.Container.Resolution!.Width}x{SettingsVisitor.Container.Resolution!.Height}", SettingsVisitor.Container.Language!.TextLang.ToString()), string.Empty, ToastDuration.Short);
            return;
        }

        DateTime startDateTime = DateTime.Now;

        await Task.Delay(300);
        string uid = await ImageRecognition.RecUID() ?? "UIAF";

        if (string.IsNullOrWhiteSpace(uid))
        {
            Logger.Warn("[OcrTask] UID not detected.");
        }
        else
        {
            Logger.Warn($"[OcrTask] UID:{uid}");
        }

        List<AchievementMatchScore> scores = await ImageRecognition.RecScroll();

        try
        {
            string ocrFileName = SpecialPathProvider.GetPath($"{uid}.ocr");
            string ocrJson = JsonConvert.SerializeObject(scores, Formatting.Indented);
            File.WriteAllText(ocrFileName, ocrJson);

            string uiafFileName = SpecialPathProvider.GetPath($"{uid}.json");
            UIAFData uiaf = scores.ToUIAF();
            string uiafJson = JsonConvert.SerializeObject(uiaf, Formatting.Indented);
            File.WriteAllText(uiafFileName, uiafJson);

            if (uiaf.List.Count > 0)
            {
                try
                {
                    _ = Process.Start("explorer.exe", new FileInfo(uiafFileName).DirectoryName!);
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
                NoticeService.AddNotice(Mui("Tips"), Mui("JiggingCompletedTips", uiaf.List.Count.ToString(), $"{(DateTime.Now - startDateTime).TotalMinutes:0.0}"), string.Empty, ToastDuration.Short);
            }
            else
            {
                NoticeService.AddNotice(Mui("Tips"), Mui("JiggingNotCompletedTips"), string.Empty, ToastDuration.Short);
            }
            if (ImageRecognition.IsRunning)
            {
                WeakReferenceMessenger.Default.Send(new RunningMessage() { Indicate = RunningIndicate.Completed });
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
    }
}
