using System.ComponentModel.DataAnnotations;
using ReactiveUI;
using TestSystem.Models;

namespace TestSystem.ViewModels;

public class TestViewModel : ViewModelBase
{
    private readonly Test _test;

    private bool _isEdit = false;

    public bool IsEdit
    {
        get => _isEdit;
        set => this.RaiseAndSetIfChanged(ref _isEdit, value);
    }
    
    [Required(ErrorMessage = "Enter your first name!", AllowEmptyStrings = false)]
    public string Title
    {
        get => _test.Name;
        set
        {
            if(!IsEdit)
                return;
            _test.Name = value;
            this.RaisePropertyChanged();
        }
    }

    public TestViewModel(Test test)
    {
        _test = test;
    }
    
    
    
}