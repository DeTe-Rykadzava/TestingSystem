using System.Threading.Tasks;
using ReactiveUI;
using TestSystem.Models;

namespace TestSystem.ViewModels;

public class TeacherQueryAnswerViewModel : ViewModelBase
{
    private readonly QueryAnswer _answer;

    public int Id => _answer.Id;

    public string Answer
    {
        get => _answer.Answer;
        set
        {
            _answer.Answer = value;
            this.RaisePropertyChanged();
        }
    }

    public bool IsCorrect
    {
        get => _answer.IsCorrect;
        set
        {
            _answer.IsCorrect = value;
            this.RaisePropertyChanged();
        }
        
    }

    public TeacherQueryAnswerViewModel(QueryAnswer answer)
    {
        _answer = answer;
    }

    public async Task<bool> Delete()
    {
        return await _answer.DeleteAnswer();
    }

    public async Task ResetChanges()
    {
        await _answer.ResetChanges();
    }
}