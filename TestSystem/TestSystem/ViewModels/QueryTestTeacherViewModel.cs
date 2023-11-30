using System.Reflection.Metadata;
using System;
using System.Threading.Tasks;
using ReactiveUI;
using TestSystem.Models;

namespace TestSystem.ViewModels;

public class QueryTestTeacherViewModel : ViewModelBase
{
    private readonly QueryTest _query;

    public string QueryString => _query.Query;

    private bool _isValid = false;

    public bool IsValid
    {
        get => _isValid;
        protected set => this.RaiseAndSetIfChanged(ref _isValid, value);
    }

    public QueryTestTeacherViewModel(QueryTest query)
    {
        _query = query;
        
        // this.WhenAnyValue(x => x.QueryString).Subscribe(s =>
        // {
        //     if (!string.IsNullOrWhiteSpace(s))
        //         IsValid = true;
        // });
    }

    public async Task<bool> Delete()
    {
        var result = await _query.DeleteQuery();
        if(!result)
            _query.CanselDeleteTest();
        return result;
    }
    
}