using System.Threading.Tasks;
using TestSystem.Models;

namespace TestSystem.ViewModels;

public class TeacherQueryAnswerViewModel : ViewModelBase
{
    private readonly QueryAnswer _answer;

    public int Id => _answer.Id;

    public string Answer => _answer.Answer;

    public TeacherQueryAnswerViewModel(QueryAnswer answer)
    {
        _answer = answer;
    }

    public async Task<bool> Delete()
    {
        return await _answer.DeleteAnswer();
    }
}