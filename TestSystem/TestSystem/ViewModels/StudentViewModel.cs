namespace TestSystem.ViewModels;

public class StudentViewModel : ViewModelBase
{
    private static StudentViewModel? _instance;

    public static StudentViewModel GetInstance()
    {
        return _instance ??= new StudentViewModel();
    }
}