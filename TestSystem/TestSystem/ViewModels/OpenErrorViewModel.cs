using System;
using System.Windows.Input;
using ReactiveUI;

namespace TestSystem.ViewModels;

public class OpenErrorViewModel : ViewModelBase
{
    public string ErrorText { get; } = "We can't connect to the database, please check the Ethernet connection or to the database server!";
    
    public ICommand CloseAppCommand { get; }

    private static OpenErrorViewModel? _instance = null;

    public static OpenErrorViewModel GetInstance()
    {
        return _instance ??= new OpenErrorViewModel();
    }

    private OpenErrorViewModel()
    {
        CloseAppCommand = ReactiveCommand.Create(() =>
        {
            Environment.Exit(1);
        });
    }
}