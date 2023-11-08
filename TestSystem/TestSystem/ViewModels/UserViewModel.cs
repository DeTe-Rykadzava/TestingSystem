using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DynamicData;
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
    
    private GroupViewModel? _currentGroup = null;
    
    [Required(ErrorMessage = "Select your group!")]
    public GroupViewModel? CurrentGroup
    {
        get => _currentGroup;
        set => this.RaiseAndSetIfChanged(ref _currentGroup, value);
    }
    
    public ObservableCollection<GroupViewModel> Groups { get; } = new ();

    public UserViewModel(User user, bool isNewBlankUser)
    {
        _user = user;
        if (isNewBlankUser)
        {
            _isValid = false;
            CanEdit = true;
            IsNewUser = true;
            SetData();
        }
        else
        {
            Role = new RoleViewModel(_user.UserRole);
            _isValid = true;
            CurrentGroup = _user.Group == null ? null : new GroupViewModel(_user.Group);
        }
        this.WhenAnyValue(x => x.FirstName, x => x.SecondName,
                x => x.UserLogin, x => x.UserPassword ,
                x => x.CurrentGroup)
            .Subscribe(s =>
            {
                if (string.IsNullOrWhiteSpace(s.Item1) || string.IsNullOrWhiteSpace(s.Item2) ||
                    string.IsNullOrWhiteSpace(s.Item3) || string.IsNullOrWhiteSpace(s.Item4) ||
                    s.Item3 == s.Item4 || s.Item4.Length < 8 || (Role?.RoleName == "Student" && s.Item5 == null))
                {
                    IsValid = false;
                    return;
                }

                IsValid = true;
            });
    }

    private async void SetData()
    {
        await SetRole();
        await SetGroups();
    }

    private async Task SetGroups()
    {
        Groups.AddRange(await Group.GetGroups());
    }

    private async Task SetRole()
    {
        Role = (await Models.Role.GetRoleByName("Student"))!;
    }

    public static async Task<UserViewModel> CreateNewUser(User newBlankUser)
    {
        var vm = new UserViewModel(newBlankUser, true);
        return vm;
    }

    public void DoRegister(User registeredUser)
    {
        _user = registeredUser;
        CanEdit = false;
        IsNewUser = false;
    }
}