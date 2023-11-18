using System.Threading.Tasks;
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
            _test.Name = value;
            ShortTitle = GetShortTitle(_test.Name);
            this.RaisePropertyChanged();
        }
    }

    private string _shortTitle = "";

    public string ShortTitle
    {
        get => _shortTitle;
        set => this.RaiseAndSetIfChanged(ref _shortTitle, value);
    }
    
    public TeacherTestViewModel(Test test)
    {
        _test = test;
        ShortTitle = GetShortTitle(_test.Name);
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

    public async Task<bool> DeleteTest()
    {
        return await Test.DeleteTest(_test);
    }
    
    public void CanselDeleteTest()
    {
        Test.CanselDeleteTest(_test);
    }

    public async Task ResetChanges()
    {
        
    }

    public async Task<bool> SaveChanges()
    {
        return true;
    }
}