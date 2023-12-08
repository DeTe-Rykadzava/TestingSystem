using System.Reflection.Metadata;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using System.Windows.Input;
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

    private bool _oneAnswer = false;
    public bool OneAnswer
    {
        get => _oneAnswer;
        private set => this.RaiseAndSetIfChanged(ref _oneAnswer, value);
    }

    public ReactiveCommand<TeacherQueryAnswerViewModel, Unit> RemoveAnswer { get; }

    public ICommand AddQueryAnswer { get; }

    public QueryTestTeacherViewModel(QueryTest query)
    {
        _query = query;

        RemoveAnswer = ReactiveCommand.CreateFromTask(async (TeacherQueryAnswerViewModel answer) =>
        {
            if (await answer.Delete())
                Answers.Remove(answer);
        });
        
        AddQueryAnswer = ReactiveCommand.CreateFromTask(async () =>
        {
            var answer = await QueryAnswer.CreateQueryAnswer(_query);
            if(answer != null)
                Answers.Add(answer);
        });
        
        this.WhenAnyValue(x => x.QueryString).Subscribe(s =>
        {
            if (!string.IsNullOrWhiteSpace(s))
                IsValid = true;
            else
                IsValid = false;

        });

        this.WhenAnyValue(x => x.Answers.Count).Subscribe(s =>
        {
            if (s == 1)
                OneAnswer = true;
            else if(s > 1)
                OneAnswer = false;
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
        foreach (var answer in Answers)
        {
            await answer.ResetChanges();
        }
        this.RaisePropertyChanged(nameof(QueryString));
    }

    public async Task<bool> Delete()
    {
        var result = await _query.DeleteQuery();
        if(!result)
            _query.CanselDeleteTest();
        return result;
    }

}