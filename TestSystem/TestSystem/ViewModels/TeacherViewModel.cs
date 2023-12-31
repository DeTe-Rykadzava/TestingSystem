﻿using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using TestSystem.Core;
using TestSystem.Models;
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

    private ViewModelBase? _testVm = null;

    public ViewModelBase? TestVm
    {
        get => _testVm;
        set => this.RaiseAndSetIfChanged(ref _testVm, value);
    }

    private TeacherTestViewModel? _selectedTest = null;
    
    public TeacherTestViewModel? SelectedTest
    {
        get => _selectedTest;
        set => this.RaiseAndSetIfChanged(ref _selectedTest, value);
    }
    
    public ObservableCollection<TeacherTestViewModel> Tests { get; } = new();
    
    public ICommand CreateTestCommand { get; }

    public ICommand EditTestCommand { get; }

    public ICommand DeleteTestCommand { get; }

    private TeacherViewModel()
    {
        Task.Run(LoadData);
        _user = User.GetCurrentUser()!;
        
        CreateTestCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var newTest = await Test.CreateNewBlackTest();
            TestVm = newTest;
            if (!await newTest.EditTest())
                await newTest.DeleteTest();
            else
                Tests.Add(newTest);
            TestVm = null;
        });

        var canEdit = this.WhenAnyValue(x => x.SelectedTest, x => x.Tests, (selectedTest, tests) => selectedTest != null && tests.Any())
            .DistinctUntilChanged();
        
        EditTestCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            TestVm = SelectedTest!;
            await SelectedTest!.EditTest();
            SelectedTest = null;
            TestVm = null;
        }, canEdit);
        
        DeleteTestCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (await MessageBoxManager.GetMessageBoxStandard("Delete", "Вы уверены?", ButtonEnum.YesNo).ShowAsync() == ButtonResult.No)
            {
                return;
            }
            
            if (!await SelectedTest!.DeleteTest())
            {
                SelectedTest.CanselDeleteTest();
                await MessageBox.ShowMessageBox("Error","Не удалось удалить тест");
                return;
            }
            
            Tests.Remove(SelectedTest!);
            SelectedTest = null;
        }, canEdit);
    }

    private async void LoadData()
    {
        var tests = await Test.GetAllUserTests();
        Tests.AddRange(tests);
    }
}