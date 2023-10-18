using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using TestingSystem.Models;

namespace TestingSystem.ViewModels;

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
        privet
    }
}