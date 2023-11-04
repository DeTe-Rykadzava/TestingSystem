using System.Threading.Tasks;
using ReactiveUI;

namespace TestSystem.ViewModels;

public class MainViewModel : ViewModelBase
{
    private ViewModelBase _currentContentViewModel;

    public ViewModelBase CurrentContentViewModel
    {
        get => _currentContentViewModel;
        private set => this.RaiseAndSetIfChanged(ref _currentContentViewModel, value);
    }

    public MainViewModel()
    {
        _currentContentViewModel = LoginViewModel.GetInstance(this);
    }

    public async Task SetView(ViewModelBase vm)
    {
        CurrentContentViewModel = vm;
    }
}