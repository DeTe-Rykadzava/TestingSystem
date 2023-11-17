using System.Collections.ObjectModel;
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
        //     var newTest = await Test.CreateNewBlackTest();
        //     newTest.BeginEdit();
        //     Tests.Add(newTest);
        //     var result = await ShowTestInteraction.Handle(newTest);
        //     if(result == null)
        //         await newTest.ResetChanges();
        //     if (!(await newTest.SaveChanges()))
        //     {
        //         await MessageBox.ShowMessageBox("Error","Не удалось сохранить новые данные, попробуйте отредактировать");
        //     }
        });

        // var canEdit = this.WhenAnyValue(x => x.SelectedTest, x => x.Tests, (selectedTest, tests) => selectedTest != null && tests.Any())
        //     .DistinctUntilChanged();
        
        EditTestCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            // SelectedTest!.BeginEdit();
            // var res = await ShowTestInteraction.Handle(SelectedTest!);
            // if (res == null)
            // {
            //     await SelectedTest.ResetChanges();
            //     return;
            // }
            // else
            // {
            //     SelectedTest.EndEdit();
            //     await res.SaveChanges();
            // }
            //
            // SelectedTest = null;
        });
        
        DeleteTestCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            // if (await MessageBoxManager.GetMessageBoxStandard("Delete", "Вы уверены?", ButtonEnum.YesNo).ShowAsync() == ButtonResult.No)
            // {
            //     return;
            // }
            //
            // if (!await TestViewModel.DeleteTest(SelectedTest!))
            // {
            //     await MessageBox.ShowMessageBox("Error","Не удалось удалить тест");
            //     return;
            // }
            //
            // Tests.Remove(SelectedTest!);
            // SelectedTest = null;

        });
    }

    private async void LoadData()
    {
        var tests = await Test.GetAllUserTests();
        Tests.AddRange(tests);
    }
}