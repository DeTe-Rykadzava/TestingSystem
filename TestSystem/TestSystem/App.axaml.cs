using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Splat;
using TestSystem.Models;
using TestSystem.ViewModels;
using TestSystem.Views;

namespace TestSystem;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        TestingSystemDbContext.Instance.Database.EnsureCreated();
        // DI
        Locator.GetLocator().Register<TestingSystemDbContext>(() => TestingSystemDbContext.Instance);
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = MainViewModel.GetInstance()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = MainViewModel.GetInstance()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}