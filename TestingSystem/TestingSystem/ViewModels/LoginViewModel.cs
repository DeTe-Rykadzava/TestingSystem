using System;
using System.ComponentModel.DataAnnotations;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;

namespace TestingSystem.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private string _login = string.Empty;
    
    [Required(ErrorMessage = "Enter your login!", AllowEmptyStrings = false)]
    public string Login
    {
        get => _login;
        set => this.RaiseAndSetIfChanged(ref _login, value);
    }
    
    private string _password = string.Empty;
    
    [Required(ErrorMessage = "Enter your password!", AllowEmptyStrings = false)]
    public string Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    public ICommand LoginCommand { get; }

    public ICommand RegisterCommand { get; }

    private readonly MainViewModel _root;

    private static LoginViewModel? _instance = null;
    
    private LoginViewModel(MainViewModel root)
    {
        _root = root;

        var canLogin = this.WhenAnyValue(x => x.Login, x => x.Password,
            (login, password) =>
                !string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password)).DistinctUntilChanged();

        LoginCommand = ReactiveCommand.Create(() =>
        {
            
        });

        RegisterCommand = ReactiveCommand.Create(() =>
        {
            _root.SetView(new RegisterViewModel());
        });
    }

    private void Reset()
    {
        Login = string.Empty;
        Password = string.Empty;
    }

    public static LoginViewModel GetInstance(MainViewModel root)
    {
        if (_instance == null)
            _instance = new LoginViewModel(root);
        _instance.Reset();
        return _instance;
    }
}