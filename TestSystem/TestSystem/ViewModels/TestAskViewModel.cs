using TestSystem.Models;

namespace TestSystem.ViewModels;

public class TestAskViewModel : ViewModelBase
{
    private readonly TestAsk _ask;

    public TestAskViewModel(TestAsk ask)
    {
        _ask = ask;
    }
}