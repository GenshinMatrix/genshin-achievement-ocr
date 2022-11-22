using System;

namespace GenshinAchievementOcr.Core;

internal static class TimeStampHelper
{
    private static readonly DateTime Epoch = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
    public static DateTime ToDateTime(this long timeStamp) => Epoch.AddSeconds(timeStamp);
    public static long ToTimeStamp(this DateTime dateTime) => (long)dateTime.Subtract(Epoch).TotalSeconds;
}
