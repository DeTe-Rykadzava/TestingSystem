using System;
using System.Threading.Tasks;
using ReactiveUI;
using TestingSystem.Models;

namespace TestingSystem.ViewModels;

public class UserViewModel : ViewModelBase
{
    private readonly User _user;

    private bool _canEdit = false;
    public bool CanEdit
    {
        get => _canEdit; 
        private set => this.RaiseAndSetIfChanged(ref _canEdit, value);
    }

    public string FirstName
    {
        get => _user.FirstName;
        set
        {
            if(!CanEdit)
                return;
            _user.FirstName = value;
            this.RaisePropertyChanged();
        }
    }

    public UserViewModel(User user)
    {
        _user = user;
    }

    private UserViewModel()
    {
        CanEdit = true;
    }

    public async Task DoRegister()
    {
        try
        {
            await User.RegisterNewUser(this);
            CanEdit = false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public static UserViewModel RegisterNewUser()
    {
        var vm = new UserViewModel();
        return vm;
    }
}