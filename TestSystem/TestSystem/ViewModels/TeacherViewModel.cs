using System.Collections.ObjectModel;
using System.Reactive;
using DynamicData;
using ReactiveUI;
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

    private TestViewModel? _selectedTest = null;

    public TestViewModel? SelectedTest
    {
        get => _selectedTest;
        set => this.RaiseAndSetIfChanged(ref _selectedTest, value);
    }
    
    public ObservableCollection<TestViewModel> Tests { get; } = new();

    public Interaction<TestViewModel, TestViewModel?> CreateNewTest { get; }

    private TeacherViewModel()
    {
        _user = User.GetCurrentUser()!;
        CreateNewTest = new Interaction<TestViewModel, TestViewModel?>();
        LoadData();
    }

    private async void LoadData()
    {
        var tests = await Test.GetAllUserTests();
        Tests.AddRange(tests);
    }
}