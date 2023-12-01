using System.Reactive.Concurrency;
using System.Threading.Tasks;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
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

    private bool _queriesValid = false;
    public bool QueriesValid
    {
        get => _queriesValid;
        private set => this.RaiseAndSetIfChanged(ref _queriesValid, value);
    }

    private TaskCompletionSource<bool> _completionSource;

    public ICommand CanselCommand { get; }
    
    public ICommand SaveCommand { get; }

    public ObservableCollection<QueryTypeViewModel> QueryTypes { get; } = new ();

    public ReactiveCommand<QueryTypeViewModel, Unit> AddQuestionCommand { get; }

    public ReactiveCommand<QueryTestTeacherViewModel, Unit> RemoveQueryCommand { get; }

    private int _countRightQueries = 0;
    public int CountRightQueries
    {
        get => _countRightQueries;
        private set => this.RaiseAndSetIfChanged(ref _countRightQueries, value);
    }

    public ObservableCollection<QueryTestTeacherViewModel> Queries { get; } = new();

    public TeacherTestViewModel(Test test, bool isNew = false)
    {
        _test = test;
        _completionSource = new TaskCompletionSource<bool>();
        CanselCommand = ReactiveCommand.CreateFromTask( async () =>
        {
            if(!isNew)
                await ResetChanges();
            _completionSource.SetResult(false);
            foreach (var query in Queries)
            {
                await query.ResetChanges();
            }
            Queries.Clear();
            Queries.CollectionChanged -= QueriesOnCollectionChanged;
        });
        var canSave = this.WhenAnyValue( 
                x => x.QueriesValid,x => x.Title, 
                (queries, title) =>
                    queries && !string.IsNullOrWhiteSpace(title)
                )
            .DistinctUntilChanged();
        SaveCommand = ReactiveCommand.CreateFromTask( async () =>
        {
            if (!await SaveChanges())
            {
                await MessageBox.ShowMessageBox("Error","Cannot save changes");
                if (isNew)
                    await DeleteTest();
                return;
            }
            if (isNew)
                isNew = !isNew;
            _completionSource.SetResult(true);
            Queries.Clear();
            Queries.CollectionChanged -= QueriesOnCollectionChanged;
        }, canSave);
        
        AddQuestionCommand = ReactiveCommand.CreateFromTask(async (QueryTypeViewModel questionType) =>
        {
            try
            {
                var query = await _test.AddQuery(questionType);
                if (query != null)
                    Queries.Add(query);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await MessageBox.ShowMessageBox("Query add error","Произошла ошибка при добавлении вопроса к тесту, обратитесь к разработчику.");
            }
        });
        
        RemoveQueryCommand = ReactiveCommand.CreateFromTask(async (QueryTestTeacherViewModel query) =>
        {
            if (await MessageBoxManager.GetMessageBoxStandard("Delete", "Вы уверены?", ButtonEnum.YesNo).ShowAsync() == ButtonResult.No)
            {
                return;
            }

            if (!await query.Delete())
            {
                await MessageBox.ShowMessageBox("Error", "Не удалось удалить вопрос");
                return;
            }

            Queries.Remove(query);
        });

        Queries.CollectionChanged += QueriesOnCollectionChanged;
        this.WhenAnyValue(x => x.Title).Subscribe(async s => { ShortTitle = await GetShortTitle(s); });
        RxApp.MainThreadScheduler.Schedule(async s => { ShortTitle = await GetShortTitle(_test.Name); });
    }

    private void QueriesOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (QueryTestTeacherViewModel query in e.NewItems)
                query.WhenAnyValue(x => x.IsValid).Subscribe(s =>
                {
                    if (s)
                        CountRightQueries += 1;
                    else
                        if(CountRightQueries > 0)
                            CountRightQueries -= 1;
                    Console.WriteLine($"{query.QueryString} is valid - " + s);
                });
    }

    
    
    private void LoadData()
    {
        Task.Run(LoadQueryTypes);

        Task.Run(LoadQueries);
    }

    private void LoadQueries()
    {
        var queries = _test.GetTestQueries();
        if (!queries.Any()) return;
        if (Queries.Any()) return;
        RxApp.MainThreadScheduler.Schedule(() =>
        {
            Queries.AddRange(queries); 
            this.WhenAnyValue(x => x.CountRightQueries).Subscribe(s =>
            {
                if (s < Queries.Count)
                    QueriesValid = false;
                else if(s == Queries.Count)
                    QueriesValid = true;
                Console.WriteLine($"test {_test.Name} queries is valid - " + QueriesValid);
            });
        });
    }

    private async void LoadQueryTypes()
    {
        var types = await QueryType.GetQueryTypes();
        if (!types.Any()) return;
        if (!QueryTypes.Any())
        {
            QueryTypes.AddRange(types);
            return;
        }
        if (types.Count > QueryTypes.Count)
        {
            foreach (var type in types)
            {
                var findedType = QueryTypes.FirstOrDefault(x => x == type);
                if(findedType != null)
                    continue;
                QueryTypes.Add(type);
            }
            return;
        }

        if (types.Count < QueryTypes.Count)
        {
            foreach (var type in QueryTypes)
            {
                var findedType = types.FirstOrDefault(x => x == type);
                if(findedType == null)
                    QueryTypes.Remove(type);
            }
            return;
        }
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
        Task.Run(LoadData);
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