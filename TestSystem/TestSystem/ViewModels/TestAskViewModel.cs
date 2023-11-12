using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;
using TestSystem.Models;

namespace TestSystem.ViewModels;

public class TestAskViewModel : ViewModelBase
{
    private readonly TestAsk _ask;
    
    [Required(ErrorMessage = "Enter title test!", AllowEmptyStrings = false)]
    public string Ask
    {
        get => _ask.Ask;
        set
        {
            if(!IsEdit)
                return;
            _ask.Ask = value;
            this.RaisePropertyChanged();
        }
    }
    
    private bool _isEdit = false;

    public bool IsEdit
    {
        get => _isEdit;
        private set => this.RaiseAndSetIfChanged(ref _isEdit, value);
    }
    
    public ObservableCollection<AskAnswerViewModel> Answers { get; } = new();
    
    private AskAnswerViewModel? _rightAnswer = null;
    
    [Required(ErrorMessage = "Enter title test!", AllowEmptyStrings = false)]
    public AskAnswerViewModel? RightAnswer
    {
        get => _rightAnswer;
        set
        {
            if(!IsEdit)
                return;
            _rightAnswer = value;
            if(value != null)
                _ask.SetRightAnswer(value.Id);
            this.RaisePropertyChanged();
        }
    }
    
    private TestAskViewModel(TestAsk ask)
    {
        _ask = ask;
    }
    
    public static TestAskViewModel GetTestAsk(TestAsk ask, bool isNew = false)
    {
        var vm = new TestAskViewModel(ask) { IsEdit = isNew };
        if(!isNew)
            vm.LoadData();
        return new TestAskViewModel(ask){IsEdit = isNew};
    }
    
    private async void LoadData()
    {
        if(_ask.Answers == null)
            return;
        var testAsks = _ask.Answers.Select(s => AskAnswerViewModel.GetAnswer(s)).ToList();
        Answers.AddRange(testAsks);
    }
    
    public static async Task<bool> DeleteTestAsk(TestAskViewModel ask)
    {
        return await TestAsk.DeleteTest(ask._ask);
    }
}