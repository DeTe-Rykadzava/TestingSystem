using System.Reactive.Concurrency;
using System.Threading.Tasks;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Windows.Input;
using TestSystem.Core;
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
            this.RaisePropertyChanged();
        }
    }

    private string _shortTitle = "";

    public string ShortTitle
    {
        get => _shortTitle;
        set => this.RaiseAndSetIfChanged(ref _shortTitle, value);
    }

    private TaskCompletionSource<bool> _completionSource;

    public ICommand CanselCommand { get; }
    
    public ICommand SaveCommand { get; }

    public TeacherTestViewModel(Test test, bool isNew = false)
    {
        _test = test;
        _completionSource = new TaskCompletionSource<bool>();
        CanselCommand = ReactiveCommand.CreateFromTask( async () =>
        {
            if(!isNew)
                await ResetChanges();
            _completionSource.SetResult(false);
        });
        var canSave = this.WhenAnyValue(x => x.Title, (title) => !string.IsNullOrWhiteSpace(title))
            .DistinctUntilChanged();
        SaveCommand = ReactiveCommand.CreateFromTask( async () =>
        {
            if (!await SaveChanges())
            {
                await MessageBox.ShowMessageBox("Error","Cannot save changes");
                await DeleteTest();
                return;
            }
            if (isNew)
                isNew = !isNew;
            _completionSource.SetResult(true);
        }, canSave);
        this.WhenAnyValue(x => x.Title).Subscribe(async s => { ShortTitle = await GetShortTitle(s); });
        RxApp.MainThreadScheduler.Schedule(async s => { ShortTitle = await GetShortTitle(_test.Name); });
    }

    private async Task<string> GetShortTitle(string title)
    {
        var shortTitle = title.Length > 50 ? title[..50] : title;
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

    public async Task<bool> EditTest()
    {
        if (_completionSource.Task.IsCompleted || _completionSource.Task.IsCanceled)
            _completionSource = new TaskCompletionSource<bool>();
        return await _completionSource.Task;
    }

    private async Task ResetChanges()
    {
        await _test.ResetChanges();
        ShortTitle = await GetShortTitle(_test.Name);
    }

    private async Task<bool> SaveChanges()
    {
        return await _test.SaveChanges();
    }
}