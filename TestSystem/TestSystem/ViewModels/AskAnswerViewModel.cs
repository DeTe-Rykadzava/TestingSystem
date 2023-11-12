using ReactiveUI;
using TestSystem.Models;

namespace TestSystem.ViewModels;

public class AskAnswerViewModel : ViewModelBase
{
    private readonly AskAnswer _answer;

    public int Id => _answer.Id;
    
    private bool _isEdit = false;

    public bool IsEdit
    {
        get => _isEdit;
        private set => this.RaiseAndSetIfChanged(ref _isEdit, value);
    }
    
    private AskAnswerViewModel(AskAnswer answer)
    {
        _answer = answer;
    }
    
    public static AskAnswerViewModel GetAnswer(AskAnswer answer, bool isNew = false)
    {
        var vm = new AskAnswerViewModel(answer) { IsEdit = isNew };
        return new AskAnswerViewModel(answer) { IsEdit = isNew };
    }

    public void SetAnswer(out AskAnswer answer)
    {
        answer = _answer;
    }
}