using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using TestSystem.ViewModels;

namespace TestSystem.Views;

public partial class TestWindowView : ReactiveWindow<TestViewModel>
{
    public TestWindowView()
    {
        this.WhenActivated((d) => d(ViewModel!.SaveChangesCommand.Subscribe(Close)));
        this.WhenActivated((d) => d(ViewModel!.CanselCommand.Subscribe(s => Close(null))));
        InitializeComponent();
    }
}