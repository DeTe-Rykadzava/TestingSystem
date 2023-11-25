using System.Reflection.Metadata;
using TestSystem.Models;

namespace TestSystem.ViewModels;

public class QueryTestTeacherViewModel : ViewModelBase
{
    private readonly QueryTest _query;

    public string QueryString => _query.Query;

    public QueryTestTeacherViewModel(QueryTest query)
    {
        _query = query;
    }
}