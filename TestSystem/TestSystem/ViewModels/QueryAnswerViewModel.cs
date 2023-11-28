using TestSystem.Models;

namespace TestSystem.ViewModels;

public class QueryAnswerViewModel : ViewModelBase
{
    private readonly QueryAnswer _answer;

    public int Id => _answer.Id;
    
    public QueryAnswerViewModel(QueryAnswer answer)
    {
        _answer = answer;
    }
}