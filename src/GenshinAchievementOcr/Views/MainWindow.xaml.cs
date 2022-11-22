using GenshinAchievementOcr.ViewModels;
using System;
using System.Windows;

namespace GenshinAchievementOcr.Views;

public partial class MainWindow : Window
{
    public MainViewModel ViewModel { get; }

    public MainWindow()
    {
        DataContext = ViewModel = new(this);
        InitializeComponent();
    }

    private void MainWindowClosed(object? sender, EventArgs e)
    {
        Application.Current.Shutdown();
    }
}
