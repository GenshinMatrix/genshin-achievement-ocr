namespace GenshinAchievementOcr.Models;

internal class RunningMessage
{
    public RunningIndicate Indicate { get; set; } = RunningIndicate.None;
    public string Message { get; set; } = null!;
}

internal enum RunningIndicate
{
    None,
    Completed,
}
