using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using ReactiveUI;
using TestSystem.Models;

namespace TestSystem.ViewModels;

public class TestViewModel : ViewModelBase
{
    private Test _test;

    private bool _isEdit = false;

    public bool IsEdit
    {
        get => _isEdit;
        private set => this.RaiseAndSetIfChanged(ref _isEdit, value);
    }
    
    private string _shortTitle = "";
    
    public string ShortTitle
    {
        get => _shortTitle;
        private set => this.RaiseAndSetIfChanged(ref _shortTitle, value);
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
            SetShortTitle();
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

    public ICommand AddNewAskCommand { get; }

    public ReactiveCommand<Unit, TestViewModel> SaveChangesCommand { get; }

    public ReactiveCommand<Unit, Unit> CanselCommand { get; }

    public ObservableCollection<TestAskViewModel> Asks { get; } = new();

    private TestViewModel(Test test)
    {
        _test = test;
        
        SetShortTitle();
        
        var canSave = this.WhenAnyValue(x => x.Title, (title) => !string.IsNullOrWhiteSpace(title))
            .DistinctUntilChanged();
        
        SaveChangesCommand = ReactiveCommand.Create( () => this, canSave);

        CanselCommand = ReactiveCommand.Create(() => {  });

        AddNewAskCommand = ReactiveCommand.CreateFromTask(async () => { Asks.Add(await TestAsk.CreateNewTestAsk(_test)); });
        
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
    }

    public void BeginEdit()
    {
        IsEdit = true;
        foreach (var ask in Asks)
        {
            ask.BeginEdit();
        }
    }

    public void EndEdit()
    {
        IsEdit = false;
        foreach (var ask in Asks)
        {
            ask.EndEdit();
        }
    }

    public async Task<bool> SaveChanges()
    {
        IsEdit = false;
        return await _test.SaveChanges();
    }

    public async Task ResetChanges()
    {
        _test.ResetChanges();
        SetShortTitle();
        this.RaisePropertyChanged();
    }

    private void SetShortTitle()
    {
        if (_test.Name.Length > 50)
            ShortTitle = _test.Name.Substring(0, 50);
        else
            ShortTitle = _test.Name;
        if (_test.Name.Length > ShortTitle.Length)
            ShortTitle += "...";
    }

    public static TestViewModel GetTest(Test test, bool isNew = false)
    {
        var vm = new TestViewModel(test) { IsEdit = isNew };
        vm.LoadData();
        return vm;
    }

    private void LoadData()
    {
        var testAsks = _test.GetTestAsks();
        Asks.AddRange(testAsks);
        // setting ask number
        Task.Run(() => {
            foreach (var ask in Asks)
            {
                ask.AskNumber = Asks.IndexOf(ask) + 1;
            }
        });
    }

    public static async Task<bool> DeleteTest(TestViewModel test)
    {
        return await Test.DeleteTest(test._test);
    }
}