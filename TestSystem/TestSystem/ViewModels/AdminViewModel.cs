namespace TestSystem.ViewModels;

public class AdminViewModel : ViewModelBase
{
    private static AdminViewModel? _instance;

    public static AdminViewModel GetInstance()
    {
        return _instance ??= new AdminViewModel();
    }
}