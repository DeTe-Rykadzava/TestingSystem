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

    private int _askNumber = 0;

    public int AskNumber
    {
        get => _askNumber;
        set => this.RaiseAndSetIfChanged(ref _askNumber, value);
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

    public void BeginEdit()
    {
        IsEdit = true;
    }
    
    public void EndEdit()
    {
        IsEdit = false;
    }

    private TestAskViewModel(TestAsk ask)
    {
        _ask = ask;
    }
    
    public static TestAskViewModel GetTestAsk(TestAsk ask, bool isNew = false)
    {
        var vm = new TestAskViewModel(ask) { IsEdit = isNew };
        vm.LoadData();
        return vm;
    }
    
    private async void LoadData()
    {
        var testAsks = _ask.GetAskAnswers();
        Answers.AddRange(testAsks);
    }
    
    public static async Task<bool> DeleteTestAsk(TestAskViewModel ask)
    {
        return await TestAsk.DeleteAsk(ask._ask);
    }
}