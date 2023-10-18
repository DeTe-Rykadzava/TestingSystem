using System;
using TestingSystem.Models;

namespace TestingSystem.ViewModels;

public class RoleViewModel : ViewModelBase
{
    private readonly Role _role;

    public int RoleId => _role.Id;

    public string RoleName => _role.RoleName;

    public RoleType RoleType
    {
        get
        {
            if (!RoleType.TryParse(_role.RoleName, out RoleType type))
            {
                throw new Exception("It is impossible to identify the user's role");
            }

            return type;
        }
    }

    public RoleViewModel(Role role)
    {
        _role = role;
    }
}