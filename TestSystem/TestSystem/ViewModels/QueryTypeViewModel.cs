using TestSystem.Models;

namespace TestSystem.ViewModels;

public class QueryTypeViewModel : ViewModelBase
{
    private readonly QueryType _type;

    public int Id => _type.Id;

    public string TypeName => _type.Type;

    public QueryTypeViewModel(QueryType type)
    {
        _type = type;
    }
}