using System.Reflection.Metadata;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;
using TestSystem.Models;

namespace TestSystem.ViewModels;

public class QueryTestTeacherViewModel : ViewModelBase
{
    private readonly QueryTest _query;

    public string QueryString
    {
        get => _query.Query;
        set
        {
            _query.Query = value;
            this.RaisePropertyChanged();
        }
    }

    private bool _isValid = false;

    public bool IsValid
    {
        get => _isValid;
        protected set => this.RaiseAndSetIfChanged(ref _isValid, value);
    }

    public ObservableCollection<TeacherQueryAnswerViewModel> Answers { get; } = new();

    public QueryTestTeacherViewModel(QueryTest query)
    {
        _query = query;
        
        this.WhenAnyValue(x => x.QueryString).Subscribe(s =>
        {
            if (!string.IsNullOrWhiteSpace(s))
                IsValid = true;
            else
                IsValid = false;

        });

        RxApp.MainThreadScheduler.Schedule(LoadData);
    }

    private void LoadData()
    {
        var answers = _query.GetQueryAnswers();
        if(!answers.Any())
            return;
        Answers.AddRange(answers);
    }

    public async Task ResetChanges()
    {
        await _query.ResetChanges();
        QueryString = _query.Query;
    }

    public async Task<bool> Delete()
    {
        var result = await _query.DeleteQuery();
        if(!result)
            _query.CanselDeleteTest();
        return result;
    }
    
}