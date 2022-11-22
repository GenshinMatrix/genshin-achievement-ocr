using System;

namespace GenshinAchievementOcr.Models;

internal class ExceptionMessage
{
    public Exception Exception { get; set; } = null!;
    public string Message { get; set; } = null!;

    public override string ToString()
    {
        return Message ?? Exception?.ToString()!;
    }
}
