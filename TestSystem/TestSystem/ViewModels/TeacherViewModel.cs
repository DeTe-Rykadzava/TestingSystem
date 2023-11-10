using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using TestSystem.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reactive.Linq;
using System.Windows.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using TestSystem.Core;
using TestSystem.Models;
using TestSystem.ViewModels;


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

    public Interaction<TestViewModel, TestViewModel?> ShowTestInteraction { get; }

    public ICommand CreateTestCommand { get; }

    public ICommand EditTestCommand { get; }

    public ICommand DeleteTestCommand { get; }

    private TeacherViewModel()
    {
        Task.Run(LoadData);
        ShowTestInteraction = new Interaction<TestViewModel, TestViewModel?>();
        _user = User.GetCurrentUser()!;
        
        CreateTestCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var result = await ShowTestInteraction.Handle(Test.CreateNewBlackTest());
            if(result == null)
                return;
            Tests.Add(result);
            var success = await TestViewModel.SaveTestInBase(result);
            if (success)
            {
                await MessageBox.ShowMessageBox("Success", "Успешно");
                return;
            }
            await MessageBox.ShowMessageBox("Error", "Не удалось сохранить");
        });
        
        // var canEdit = this.WhenAnyValue(x => x.SelectedTest, x => Tests,
        //         (test, tests) => test != null && tests.Count > 0)
        //     .DistinctUntilChanged();
        
        EditTestCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            SelectedTest!.BeginEdit();
            var res = await ShowTestInteraction.Handle(SelectedTest!);
            if (res == null)
            {
                SelectedTest.ResetChanges();
                return;
            }
            SelectedTest.SaveChanges();
        });
        
        DeleteTestCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (await MessageBoxManager.GetMessageBoxStandard("Delete", "Вы уверены?", ButtonEnum.YesNo).ShowAsync() == ButtonResult.No)
            {
                return;
            }

            if (!await TestViewModel.DeleteTest(SelectedTest!))
            {
                MessageBox.ShowMessageBox("Error","Не удалось удалить тест");
                return;
            }

            Tests.Remove(SelectedTest!);
            SelectedTest = null;

        });
    }

    private async void LoadData()
    {
        var tests = await Test.GetAllUserTests();
        Tests.AddRange(tests);
    }
}