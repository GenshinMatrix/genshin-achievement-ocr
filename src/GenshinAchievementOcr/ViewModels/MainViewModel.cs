using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GenshinAchievementOcr.Core;
using GenshinAchievementOcr.Models;
using GenshinAchievementOcr.Views;
using SharpVectors.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GenshinAchievementOcr.ViewModels;

public class MainViewModel : ObservableRecipient, IRecipient<RunningMessage>
{
    public MainWindow Source { get; set; } = null!;
    internal ImageJigging jigging = new();

    public double Left => SystemParameters.WorkArea.Right;
    public double Top => SystemParameters.WorkArea.Bottom;

    public bool ExportCapturedImages
    {
        get => Settings.ExportCapturedImages;
        set
        {
            Settings.ExportCapturedImages.Set(value);
            SettingsManager.Save();
        }
    }

    public bool ExportDebugImages
    {
        get => Settings.ExportDebugImages;
        set
        {
            Settings.ExportDebugImages.Set(value);
            SettingsManager.Save();
        }
    }

    public ICommand StartCommand { get; }

    public ICommand ConfigOpenCommand => new RelayCommand(() =>
    {
        try
        {
            _ = Process.Start(new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                Arguments = $"/c \"{SettingsManager.Path}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
            });
        }
        catch
        {
        }
    });

    public ICommand ConfigOpenWithNotepadCommand => new RelayCommand(async () =>
    {
        try
        {
            _ = Process.Start(new ProcessStartInfo()
            {
                FileName = "notepad.exe",
                Arguments = $"\"{SettingsManager.Path}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
            });
        }
        catch
        {
        }
    });

    public ICommand ConfigOpenWithCommand => new RelayCommand(async () =>
    {
        try
        {
            _ = Process.Start(new ProcessStartInfo()
            {
                FileName = "openwith.exe",
                Arguments = $"\"{SettingsManager.Path}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
            });
        }
        catch
        {
        }
    });

    public ICommand ConfigReloadCommand => new RelayCommand(() =>
    {
        SettingsManager.Reinit();
        SetupLanguage();
    });

    public ICommand TopMostCommand => new RelayCommand<Window>(app =>
    {
        app!.Topmost = !app.Topmost;
        if (app.FindName("TextBlockTopMost") is TextBlock topMostIcon)
        {
            topMostIcon.Text = app.Topmost ? FluentSymbol.Unpin : FluentSymbol.Pin;
        }
    });
    public ICommand RestorePosCommand => new RelayCommand<Window>(app =>
    {
        app!.Left = Left - app.Width;
        app!.Top = Top - app.Height;
    });
    public ICommand RestartCommand => NotifyIconViewModel.RestartCommand;
    public ICommand ExitCommand => NotifyIconViewModel.ExitCommand;
    public ICommand UsageCommand => NotifyIconViewModel.UsageCommand;
    public ICommand GitHubCommand => NotifyIconViewModel.GitHubCommand;

    public MainViewModel(MainWindow source)
    {
        Source = source;

        WeakReferenceMessenger.Default.RegisterAll(this);
        StartCommand = new RelayCommand<Button>(async button =>
        {
            TextBlock startIcon = (Window.GetWindow(button).FindName("TextBlockStartIcon") as TextBlock)!;
            TextBlock start = (Window.GetWindow(button).FindName("TextBlockStart") as TextBlock)!;
            SvgViewbox mainIcon = (Window.GetWindow(button).FindName("SvgViewBoxMainIcon") as SvgViewbox)!;

            Brush brush = null!;
            button!.IsEnabled = false;
            if (startIcon.Text.Equals(FluentSymbol.Start))
            {
                brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EEDBE8F6"));
                start.Text = Mui("ButtonStop");
                startIcon.Text = FluentSymbol.Stop;
                mainIcon.SetColor("#E8B036");
                StartReg();
            }
            else
            {
                brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EEFFFFFF"));
                start.Text = Mui("ButtonStart");
                startIcon.Text = FluentSymbol.Start;
                mainIcon.SetColor("Black");
                StopReg();
            }

            Border border = (Window.GetWindow(button).FindName("Border") as Border)!;
            StoryboardUtils.BeginBrushStoryboard(border, new Dictionary<DependencyProperty, Brush>()
            {
                [Border.BackgroundProperty] = brush,
            });
            await Task.Delay(20);
            button!.IsEnabled = true;
        });
        RegisterHotKey();
    }

    private void RegisterHotKey()
    {
        try
        {
            HotkeyHolder.RegisterHotKey(Settings.ShortcutKey, (s, e) =>
            {
                StartCommand?.Execute(Source.FindName("ButtonStart"));
            });
        }
        catch (Exception e)
        {
            Logger.Exception(e);
        }
    }

    public void StartReg()
    {
        _ = Task.Run(async () => await jigging.StartReg());
    }

    public void StopReg()
    {
        jigging.StopReg();
    }

    void IRecipient<RunningMessage>.Receive(RunningMessage message)
    {
        _ = Source.Dispatcher.BeginInvoke(() =>
        {
            if (message?.Indicate == RunningIndicate.Completed)
                StartCommand?.Execute(Source.FindName("ButtonStart"));
        });
    }
}
