using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using DynamicData;
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
        private set => this.RaiseAndSetIfChanged(ref _isEdit, value);
    }
    
    [Required(ErrorMessage = "Enter title test!", AllowEmptyStrings = false)]
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

    public string CreatorFullName => $"{_test.CreatorUser.SecondName} {_test.CreatorUser.FirstName} {_test.CreatorUser.LastName}";

    private bool _isValid = false;

    public bool IsValid
    {
        get => _isValid;
        private set => this.RaiseAndSetIfChanged(ref _isValid, value);
    }
    
    public ObservableCollection<TestAskViewModel> Asks { get; } = new();

    private TestViewModel(Test test)
    {
        _test = test;
        this.WhenAnyValue(x => x.Title)
            .Subscribe(s =>
            {
                if (string.IsNullOrWhiteSpace(s))
                {
                    IsValid = false;
                    return;
                }

                IsValid = true;
            });
        LoadData();
    }

    public static TestViewModel GetTest(Test test, bool isNew = false)
    {
        return new TestViewModel(test){IsEdit = isNew};
    }

    private async void LoadData()
    {
        if(_test.Asks.Count == 0)
            return;
        var testAsks = _test.Asks.Select(s => new TestAskViewModel(s)).ToList();
        Asks.AddRange(testAsks);
    }
}