using GenshinAchievementOcr.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Windows.Media.Ocr;

namespace GenshinAchievementOcr.Core;

internal static class AchievementRecognition
{
    public static AchievementMatchScore GetScore(this AchievementMatchable matchable, double passScore = 75d, double minScore = 30d, double highScore = 90d)
    {
        AchievementMatchScore score = new(matchable);

        // Matching once
        foreach (AchievementRepositoryMetaItem item in AchievementRepository.WondersOfTheWorldEnglish.Achievements)
        {
            double nameScore = CalcSimilarityStringScore(item.Name, matchable.Name);
            double descScore = CalcSimilarityStringScore(item.Desc, matchable.Desc);

            if (nameScore > score.TryNameScore)
            {
                score.TryNameScore = nameScore;
            }
            if (descScore > score.TryDescScore)
            {
                score.TryDescScore = descScore;
            }
            if (nameScore > passScore && descScore > passScore
            || (nameScore > minScore && descScore > highScore)
            || (nameScore > highScore && descScore > minScore))
            {
                score.NameScore = nameScore;
                score.DescScore = descScore;
                score.Score = (nameScore + descScore) / 2d;
                score.IsMatched = true;
                score.Matched = item;
                break;
            }
        }

        // Matching twice
        if (score.NameScore == 0)
        {
            foreach (AchievementRepositoryMetaItem item in AchievementRepository.WondersOfTheWorldEnglish.Achievements)
            {
                double allScore = CalcSimilarityStringScore(item.Name + item.Desc, matchable.Name + matchable.Desc);

                if (allScore > passScore)
                {
                    score.NameScore = score.TryNameScore;
                    score.DescScore = score.TryDescScore;
                    score.Score = allScore;
                    score.IsMatched = true;
                    score.Matched = item;
                    break;
                }
            }
        }

        // Matching result
        if (matchable.Status?.Length > 0)
        {
            string[] handled = new string[matchable.Status.Length];

            for (int i = default; i < matchable.Status.Length; i++)
            {
                string status = matchable.Status[i];
                double completedScore = CalcSimilarityStringScore(status, "Completed");

                if (completedScore >= minScore)
                {
                    handled[i] = "Status";
                    score.Completed = true;
                    if (score.Matched.Hidden)
                    {
                        score.Current = 1;
                    }
                }

                if (ParseDateTime(status, "yyyy/MM/dd", out DateTime dt))
                {
                    handled[i] = "DateTime";
                    score.DateTimeString = status;
                    score.DateTime = dt;
                }
            }

            if (!score.Matched.Hidden && score.Current == 0 && matchable.Status.Count() == 1)
            {
                string status = matchable.Status.First();
                string[] splitted = status.Split("/");

                if (splitted.Length == 2)
                {
                    if (int.TryParse(splitted[0].PickNumberString(), out int left) && int.TryParse(splitted[1].PickNumberString(), out int right))
                    {
                        score.Current = left;
                        _ = right;
                    }
                }
            }

            if (handled.Contains("Status") && !handled.Contains("DateTime"))
            {
                handled[^1] = "DateTime";
                score.DateTime = new DateTime(9999, 12, 31, 23, 59, 59);
                score.DateTimeString = score.DateTime.ToString("yyyy/MM/dd");
            }

            for (int i = default; i < matchable.Status.Length; i++)
            {
                if (string.IsNullOrEmpty(handled[i]))
                {
                    handled[i] = "Addtional";
                    score.Addtional += matchable.Status[i];
                }
            }

            if (!string.IsNullOrEmpty(score.Addtional))
            {
                score.AddtionalNumberString = score.Addtional?.PickNumberString()!;
                if (int.TryParse(score.AddtionalNumberString, out int total))
                {
                    score.Current = total;
                }
            }
        }

        // Matching series
        if (score.IsMatched)
        {
            int id = score.Matched.Id;

            if (id == 80127 || id == 80128 || id == 80129)
            {
                score.MatchedAttached = new();
                if (score.Current >= 100 && id != 80129)
                {
                    score.MatchedAttached.Add(new(matchable) { Matched = AchievementRepository.GetById(80129), Current = score.Current });
                }
                if (score.Current >= 30 && id != 80128)
                {
                    score.MatchedAttached.Add(new(matchable) { Matched = AchievementRepository.GetById(80128), Current = score.Current });
                }
                if (score.Current >= 1 && id != 80127)
                {
                    score.MatchedAttached.Add(new(matchable) { Matched = AchievementRepository.GetById(80127), Current = score.Current });
                }
            }
            else if (id == 80142 || id == 80143 || id == 80144)
            {
                score.MatchedAttached = new();
                if (score.Current >= 60 && id != 80144)
                {
                    score.MatchedAttached.Add(new(matchable) { Matched = AchievementRepository.GetById(80144), Current = score.Current });
                }
                if (score.Current >= 30 && id != 80143)
                {
                    score.MatchedAttached.Add(new(matchable) { Matched = AchievementRepository.GetById(80143), Current = score.Current });
                }
                if (score.Current >= 10 && id != 80142)
                {
                    score.MatchedAttached.Add(new(matchable) { Matched = AchievementRepository.GetById(80142), Current = score.Current });
                }
            }
            else if (id == 81026 || id == 81027 || id == 81028)
            {
                score.MatchedAttached = new();
                if (score.Current >= 30 && id != 81028)
                {
                    score.MatchedAttached.Add(new(matchable) { Matched = AchievementRepository.GetById(81028), Current = score.Current });
                }
                if (score.Current >= 20 && id != 81027)
                {
                    score.MatchedAttached.Add(new(matchable) { Matched = AchievementRepository.GetById(81027), Current = score.Current });
                }
                if (score.Current >= 10 && id != 81026)
                {
                    score.MatchedAttached.Add(new(matchable) { Matched = AchievementRepository.GetById(81026), Current = score.Current });
                }
            }
            else if (id == 81029 || id == 81030 || id == 81031)
            {
                score.MatchedAttached = new();
                if (score.Current >= 30 && id != 81031)
                {
                    score.MatchedAttached.Add(new(matchable) { Matched = AchievementRepository.GetById(81031), Current = score.Current });
                }
                if (score.Current >= 20 && id != 81030)
                {
                    score.MatchedAttached.Add(new(matchable) { Matched = AchievementRepository.GetById(81030), Current = score.Current });
                }
                if (score.Current >= 10 && id != 81029)
                {
                    score.MatchedAttached.Add(new(matchable) { Matched = AchievementRepository.GetById(81029), Current = score.Current });
                }
            }
            else if (id == 82041 || id == 82042 || id == 81031)
            {
                score.MatchedAttached = new();
                if (score.Current >= 50000 && id != 82043)
                {
                    score.MatchedAttached.Add(new(matchable) { Matched = AchievementRepository.GetById(82043), Current = score.Current });
                }
                if (score.Current >= 20000 && id != 82042)
                {
                    score.MatchedAttached.Add(new(matchable) { Matched = AchievementRepository.GetById(82042), Current = score.Current });
                }
                if (score.Current >= 5000 && id != 82041)
                {
                    score.MatchedAttached.Add(new(matchable) { Matched = AchievementRepository.GetById(82041), Current = score.Current });
                }
            }
        }

        return score;
    }

    public static bool ParseDateTime(string status, string format, out DateTime dateTime)
    {
        try
        {
            DateTimeFormatInfo dtFormat = new()
            {
                ShortDatePattern = format,
            };
            status = status.Replace(" ", string.Empty);
            return DateTime.TryParseExact(status, format, null!, DateTimeStyles.None, out dateTime);
        }
        catch (Exception e)
        {
            _ = e;
            dateTime = new DateTime(9999, 12, 31, 23, 59, 59);
        }
        return false;
    }


    [Obsolete]
    public static string ParseToAchievement(this OcrLine line, string split = null!)
    {
        StringBuilder sb = new();
        for (int i = default; i < line.Words.Count; i++)
        {
            OcrWord word = line.Words[i];

            if (i != default && i != line.Words.Count - 1)
            {
                sb.Append(split ?? string.Empty);
            }
            sb.Append(word.Text);
        }
        return sb.ToString();
    }

    public static (string, string) ParseToAchievement(this OcrResult result)
    {
        StringBuilder title = new();
        StringBuilder description = new();
        bool firstLine = true;
        foreach (OcrLine line in result.Lines)
        {
            if (firstLine)
            {
                firstLine = false;
                title.Append(line.Text.ClearKanji());
            }
            else
            {
                description.Append(line.Text.ClearKanji());
            }
        }
        return (title.ToString(), description.ToString());
    }

    public static string ClearKanji(this string text)
    {
        text = text
            .Replace("，", ".")
            .Replace("。", ".")
            .Replace("？", "?")
            .Replace("！", "!")
            .Replace("：", ":")
            .Replace("；", ";")
            .Replace("／", "/")
            .Replace("（", "(")
            .Replace("）", ")")
            .Replace("()", "0");
        text = new Regex(@"[\u4e00-\u9fa5]").Replace(text, " ");
        return text;
    }

    public static string PickNumberString(this string text)
    {
        StringBuilder sb = new();

        foreach (char ch in text)
        {
            if (ch >= '0' && ch <= '9')
            {
                sb.Append(ch);
            }
        }
        return sb.ToString();
    }

    public static string[] ParseToAchievementStatus(this OcrResult result)
    {
        List<string> status = new();

        foreach (OcrLine line in result.Lines)
        {
            status.Add(line.Text.ClearKanji());
        }
        return status.ToArray();
    }

    /// <summary>
    /// https://blog.csdn.net/zcr_59186/article/details/123009253
    /// </summary>
    public static double CalcSimilarityStringScore(string textX, string textY, bool isCase = false)
    {
        if (textX.Length <= 0 || textY.Length <= 0)
        {
            return 0d;
        }
        if (!isCase)
        {
            textX = textX.ToLower();
            textY = textY.ToLower();
        }
        int[,] dp = new int[Math.Max(textX.Length, textY.Length) + 1, Math.Max(textX.Length, textY.Length) + 1];
        for (int x = 0; x < textX.Length; x++)
        {
            for (int y = 0; y < textY.Length; y++)
            {
                if (textX[x] == textY[y])
                {
                    dp[x + 1, y + 1] = dp[x, y] + 1;
                }
                else
                {
                    dp[x + 1, y + 1] = Math.Max(dp[x, y + 1], dp[x + 1, y]);
                }
            }
        }
        return Math.Round((double)dp[textX.Length, textY.Length] / Math.Max(textX.Length, textY.Length) * 100d, 2);
    }
}
