namespace TestSystem.ViewModels;

public class TeacherViewModel : ViewModelBase
{
    private static TeacherViewModel? _instance;

    public static TeacherViewModel GetInstance()
    {
        return _instance ??= new TeacherViewModel();
    }
}