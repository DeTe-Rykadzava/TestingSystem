using TestSystem.Models;

namespace TestSystem.ViewModels;

public class TeacherViewModel : ViewModelBase
{
    private readonly UserViewModel _user;

    public string UserName
    {
        get => $"{_user.SecondName} {_user.FirstName} {_user.LastName}";
    }
    
    private static TeacherViewModel? _instance;

    public static TeacherViewModel GetInstance()
    {
        return _instance ??= new TeacherViewModel();
    }

    private TeacherViewModel()
    {
        _user = User.GetCurrentUser()!;
    }
}