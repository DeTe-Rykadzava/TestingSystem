using TestSystem.Models;

namespace TestSystem.ViewModels;

public class QueryTypeViewModel : ViewModelBase
{
    private readonly QueryType _type;

    public string TypeName => _type.Type;

    public QueryTypeViewModel(QueryType type)
    {
        _type = type;
    }
}