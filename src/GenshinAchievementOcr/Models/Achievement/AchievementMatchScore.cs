using GenshinAchievementOcr.Core;
using System;
using System.Collections.Generic;

namespace GenshinAchievementOcr.Models;

internal class AchievementMatchScore
{
    public AchievementMatchable Source { get; } = null!;
    public AchievementRepositoryMetaItem Matched { get; set; } = null!;
    public List<AchievementMatchScore> MatchedAttached { get; set; } = null!;

    public bool IsMatched { get; set; } = false;
    public double NameScore { get; set; } = 0d;
    public double DescScore { get; set; } = 0d;
    public double Score { get; set; } = 0d;

    public double TryNameScore { get; set; } = 0d;
    public double TryDescScore { get; set; } = 0d;

    public bool? Completed { get; set; } = false;
    public string DateTimeString { get; set; } = null!;
    public DateTime DateTime { get; set; } = default;
    public int Current { get; set; } = default;
    public string Addtional { get; set; } = null!;
    public string AddtionalNumberString { get; set; } = null!;

    public AchievementMatchScore(AchievementMatchable src)
    {
        Source = src;
    }

    public override string ToString()
    {
        return $"[{NameScore}|{DescScore}] {Matched?.Name}|{Matched?.Desc}|{Completed}|{Current}|{Addtional ?? "None"}|{DateTimeString}";
    }
}

internal static class AchievementMatchScoreExtension
{
    public static UIAFData ToUIAF(this IEnumerable<AchievementMatchScore> list)
    {
        UIAFData uiaf = new();

        foreach (AchievementMatchScore score in list)
        {
            if (!(score.Completed ?? false)) continue;
            if (!score.IsMatched) continue;

            UIAFAchievement achievement = new()
            {
                Id = score.Matched.Id,
                Timestamp = score.DateTime.ToTimeStamp(),
                Current = score.Current,
            };

            uiaf.List.Add(achievement);

            if (score.MatchedAttached != null)
            {
                foreach (AchievementMatchScore scoreAttached in score.MatchedAttached)
                {
                    UIAFAchievement achievementAttached = new()
                    {
                        Id = scoreAttached.Matched.Id,
                        Timestamp = score.DateTime.ToTimeStamp(),
                        Current = scoreAttached.Current,
                    };
                    uiaf.List.Add(achievementAttached);
                }

            }
        }
        return uiaf;
    }
}
