using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Splat;
using TestSystem.Core;
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
        bool successConDatabase = false;
        try
        {
            TestingSystemDbContext.Instance.Database.EnsureCreated();
            Console.WriteLine("success connection");
            successConDatabase = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime errorDesktop)
            {
                errorDesktop.MainWindow = new OpenErrorWindow
                {
                    DataContext = OpenErrorViewModel.GetInstance()
                };
            }
            else if(ApplicationLifetime is ISingleViewApplicationLifetime errorSingleView)
            {
                errorSingleView.MainView = new OpenErrorView
                {
                    DataContext = OpenErrorViewModel.GetInstance()
                };
            }
        }

        if (successConDatabase)
        {
            // DIGMA
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
        }
        base.OnFrameworkInitializationCompleted();
    }
}
