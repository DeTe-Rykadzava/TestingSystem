using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using TestSystem.ViewModels;

namespace TestSystem.Views;

public partial class TeacherTestWindowView : ReactiveWindow<TestViewModel>
{
    public TeacherTestWindowView()
    {
        this.WhenActivated((d) => d(ViewModel!.SaveChangesCommand.Subscribe(Close)));
        this.WhenActivated((d) => d(ViewModel!.CanselCommand.Subscribe(s => Close(null))));
        InitializeComponent();
    }
}