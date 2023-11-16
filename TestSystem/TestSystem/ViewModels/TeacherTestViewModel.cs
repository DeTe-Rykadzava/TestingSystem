using ReactiveUI;
using TestSystem.Models;

namespace TestSystem.ViewModels;

public class TeacherTestViewModel : ViewModelBase
{
    private readonly Test _test;

    public string Title
    {
        get => _test.Name;
        set {
            if (_isEdit)
            {
                ShortTitle = GetShortTitle(_test.Name);

            }
        }
    }

    private string _shortTitle = "";

    public string ShortTitle
    {
        get => _shortTitle;
        set => this.RaiseAndSetIfChanged(ref _shortTitle, value);
    }

    private bool _isEdit = false;
    
    public TeacherTestViewModel(Test test)
    {
        _test = test;
    }

    private string GetShortTitle(string title)
    {
        var shortTitle = "";
        if (title.Length > 50)
            shortTitle = title.Substring(0, 50);
        else
            shortTitle = title;
        return shortTitle;
    }
}