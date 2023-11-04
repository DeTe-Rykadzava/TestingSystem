using System;
using System.ComponentModel.DataAnnotations;
using System.Reactive.Linq;
using System.Windows.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using TestSystem.Core;
using TestSystem.Models;
using TestSystem.ViewModels;

namespace TestSystem.ViewModels;

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

    private static LoginViewModel? _instance = null;
    
    private LoginViewModel(MainViewModel root)
    {
        var canLogin = this.WhenAnyValue(x => x.Login, x => x.Password,
            (login, password) =>
                !string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password)).DistinctUntilChanged();

        LoginCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var user = await User.GetUserByLogAndPas(Login, Password);

            if (user == null)
            {
                await MessageBox.ShowMessageBox("Login error", "Пользователь не найден");
                return;
            }
            await MessageBox.ShowMessageBox("Login success", "Успешно =)");

        }, canLogin);

        RegisterCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await root.SetView(RegisterViewModel.GetInstance(root));
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