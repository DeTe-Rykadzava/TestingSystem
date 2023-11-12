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

    public string ShortTitle { get; private set; }

    [Required(ErrorMessage = "Enter title test!", AllowEmptyStrings = false)]
    public string Title
    {
        get => _test.Name;
        set
        {
            if(!IsEdit)
                return;
            _test.Name = value;
            if (_test.Name.Length > 15)
                ShortTitle = _test.Name.Substring(0, 15);
            else
                ShortTitle = _test.Name;
            if (_test.Name.Length > ShortTitle.Length)
                ShortTitle += "...";
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
        
        if (_test.Name.Length > 45)
            ShortTitle = _test.Name.Substring(0, 45);
        else
            ShortTitle = _test.Name;
        if (_test.Name.Length > ShortTitle.Length)
            ShortTitle += "...";
        
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
    }

    public async Task<bool> SaveChanges()
    {
        IsEdit = false;
        return await _test.SaveChanges();
    }

    public async Task ResetChanges()
    {
        var defaultTest = await Test.GetTestById(_test.Id);
        if(defaultTest == null)
            return;
        _test = defaultTest._test;
        this.RaisePropertyChanged();
    }

    public static TestViewModel GetTest(Test test, bool isNew = false)
    {
        var vm = new TestViewModel(test) { IsEdit = isNew };
        if(!isNew)
            vm.LoadData();
        return new TestViewModel(test){IsEdit = isNew};
    }

    private async void LoadData()
    {
        if(_test.Asks == null)
            return;
        var testAsks = _test.Asks.Select(s => TestAskViewModel.GetTestAsk(s)).ToList();
        Asks.AddRange(testAsks);
    }

    public static async Task<bool> DeleteTest(TestViewModel test)
    {
        return await Test.DeleteTest(test._test);
    }
}