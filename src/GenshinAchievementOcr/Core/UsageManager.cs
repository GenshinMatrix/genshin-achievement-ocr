using GenshinAchievementOcr.Views;
using System.Threading.Tasks;

namespace GenshinAchievementOcr.Core;

internal static class UsageManager
{
    public static async Task ShowUsage()
    {
        await DialogWindow.ShowMessageContent(Mui("Usage"), new UsageContent());
    }
}
