﻿using System.Collections.ObjectModel;
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
            ShortTitle = _test.Name.Substring(0, 15);
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
    
    public ReactiveCommand<Unit, TestViewModel> SaveChangesCommand { get; }

    public ReactiveCommand<Unit, Unit> CanselCommand { get; }

    public ObservableCollection<TestAskViewModel> Asks { get; } = new();

    private TestViewModel(Test test)
    {
        _test = test;
        Title = "test test";
        
        // var canSave = this.WhenAnyValue(x => x.Title, (Title) => !string.IsNullOrWhiteSpace(Title))
        //     .DistinctUntilChanged();
        
        SaveChangesCommand = ReactiveCommand.Create( () => this);

        CanselCommand = ReactiveCommand.Create(() => {  });
        
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

    public void BeginEdit()
    {
        IsEdit = true;
    }

    public void SaveChanges()
    {
        IsEdit = false;
    }

    public async void ResetChanges()
    {
        var defaultTest = await Test.GetTestById(_test.Id);
        if(defaultTest == null)
            return;
        _test = defaultTest._test;
        this.RaisePropertyChanged();
    }

    public static TestViewModel GetTest(Test test, bool isNew = false)
    {
        return new TestViewModel(test){IsEdit = isNew};
    }

    private async void LoadData()
    {
        if(_test.Asks == null)
            return;
        var testAsks = _test.Asks.Select(s => new TestAskViewModel(s)).ToList();
        Asks.AddRange(testAsks);
    }

    public static async Task<bool> DeleteTest(TestViewModel test)
    {
        return await Test.DeleteTest(test._test);
    }

    public static async Task<bool> SaveTestInBase(TestViewModel test)
    {
        return await Test.SaveNewTest(test._test);
    }
}