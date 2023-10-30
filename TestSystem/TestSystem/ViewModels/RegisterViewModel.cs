using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using TestSystem.Core;
using TestSystem.Models;

namespace TestSystem.ViewModels;

public class RegisterViewModel : ViewModelBase
{
    public UserViewModel NewUser
    {
        get;
        private set;
    }

    public ICommand RegisterCommand { get; }

    public RegisterViewModel(MainViewModel root)
    {
        NewUser = UserViewModel.CreateNewUser();

        var canRegister = this.WhenAnyValue(x => x.NewUser.IsValid).DistinctUntilChanged();
        
        RegisterCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var user = await User.RegisterNewUser(NewUser);
            if (user == null)
            {
                await MessageBox.ShowMessageBox("Registration error", "Пользователь увы не создан");
                return;
            }
            await MessageBox.ShowMessageBox("Registration success", "Успешно =)");
        }, canRegister);
    }
}