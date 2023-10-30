using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ReactiveUI;
using TestSystem.Models;

namespace TestSystem.ViewModels;

public class UserViewModel : ViewModelBase
{
    private User _user;

    private bool _canEdit = false;
    public bool CanEdit
    {
        get => _canEdit; 
        private set => this.RaiseAndSetIfChanged(ref _canEdit, value);
    }
    
    private bool _isNewUser = false;
    public bool IsNewUser
    {
        get => _isNewUser; 
        private set => this.RaiseAndSetIfChanged(ref _isNewUser, value);
    }

    public int UserId  => _user.Id; 

    [Required(ErrorMessage = "Enter your first name!", AllowEmptyStrings = false)]
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
    
    [Required(ErrorMessage = "Enter your second name!", AllowEmptyStrings = false)]
    public string SecondName
    {
        get => _user.SecondName;
        set
        {
            if(!CanEdit)
                return;
            _user.SecondName = value;
            this.RaisePropertyChanged();
        }
    }
    
    public string? LastName
    {
        get => _user.LastName;
        set
        {
            if(!CanEdit)
                return;
            _user.LastName = value;
            this.RaisePropertyChanged();
        }
    }
    
    [Required(ErrorMessage = "Enter login!", AllowEmptyStrings = false)]
    public string UserLogin
    {
        get => _user.Login;
        set
        {
            if(!CanEdit)
                return;
            _user.Login = value;
            this.RaisePropertyChanged();
        }
    }
    
    [Required(ErrorMessage = "Enter password!", AllowEmptyStrings = false)]
    public string UserPassword
    {
        get => _user.Password;
        set
        {
            if(!CanEdit)
                return;
            _user.Password = value;
            this.RaisePropertyChanged();
        }
    }

    private bool _isValid = false;

    public bool IsValid
    {
        get => _isValid;
        private set => this.RaiseAndSetIfChanged(ref _isValid, value);
    }

    public RoleViewModel Role
    {
        get;
        private set;
    }
    
    public UserViewModel(User user)
    {
        _user = user;
        Role = Models.Role.GetRoleByName(user.UserRole.RoleName).Result!;
        _isValid = true;
        this.WhenAnyValue(x => x.FirstName, x => x.SecondName,
                          x => x.UserLogin, x => x.UserPassword)
            .Subscribe(s =>
            {
                if (string.IsNullOrWhiteSpace(s.Item1) || string.IsNullOrWhiteSpace(s.Item2) ||
                    string.IsNullOrWhiteSpace(s.Item3) || string.IsNullOrWhiteSpace(s.Item4))
                {
                    IsValid = false;
                }
            });
    }

    private UserViewModel()
    {
        _isValid = false;
        _user = new User();
        CanEdit = true;
        IsNewUser = true;
        Role = Models.Role.GetRoleByName("student").Result!;
        this.WhenAnyValue(x => x.FirstName, x => x.SecondName,
                x => x.UserLogin, x => x.UserPassword)
            .Subscribe(s =>
            {
                if (string.IsNullOrWhiteSpace(s.Item1) || string.IsNullOrWhiteSpace(s.Item2) ||
                    string.IsNullOrWhiteSpace(s.Item3) || string.IsNullOrWhiteSpace(s.Item4))
                {
                    IsValid = false;
                }
            });
    }

    public static UserViewModel CreateNewUser()
    {
        var vm = new UserViewModel();
        return vm;
    }
}