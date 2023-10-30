using System;
using TestSystem.Models;

namespace TestSystem.ViewModels;

public class RoleViewModel : ViewModelBase
{
    public readonly Role Role;

    public int RoleId => Role.Id;

    public string RoleName => Role.RoleName;

    public RoleType RoleType
    {
        get
        {
            if (!Enum.TryParse(Role.RoleName, out RoleType type))
            {
                throw new Exception("It is impossible to identify the user's role");
            }

            return type;
        }
    }

    public RoleViewModel(Role role)
    {
        Role = role;
    }
}