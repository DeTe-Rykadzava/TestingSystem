using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using TestSystem.Core;
using TestSystem.Models;

namespace TestSystem.ViewModels;

public class RegisterViewModel : ViewModelBase
{
    private UserViewModel? _newUser = null;
    public UserViewModel? NewUser
    {
        get => _newUser;
        private set
        {
            RxApp.MainThreadScheduler.Schedule(() => { this.RaiseAndSetIfChanged(ref _newUser, value);});
        }
    }

    public ICommand RegisterCommand { get; }

    public ICommand ShowLoginCommand { get; }

    
    private static RegisterViewModel? _instance;
    
    private RegisterViewModel(MainViewModel root)
    {
        Task.Run(SetData);
        
        var canRegister = this.WhenAnyValue( x => x.NewUser.IsValid)
            .DistinctUntilChanged();
        
        RegisterCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var user = await User.RegisterNewUser(NewUser!);
            if (user == null)
            {
                await MessageBox.ShowMessageBox("Registration error", "Пользователь, увы, не создан");
                return;
            }
            await MessageBox.ShowMessageBox("Registration success", "Успешно =)");
        }, canRegister);

        ShowLoginCommand = ReactiveCommand.CreateFromTask(async () => { await root.SetView(LoginViewModel.GetInstance(root));});
    }
    
    private void Reset()
    {
        NewUser = null;
        Task.Run(SetData);
    }

    public static RegisterViewModel GetInstance(MainViewModel root)
    {
        if (_instance == null)
            _instance = new RegisterViewModel(root);
        else
            _instance.Reset();
        return _instance;
    }
    
    private async void SetData()
    { 
        NewUser = await UserViewModel.CreateNewUser();
    }
}