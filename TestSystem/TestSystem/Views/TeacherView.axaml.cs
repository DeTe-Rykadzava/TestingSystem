using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Splat;
using TestSystem.ViewModels;

namespace TestSystem.Views;

public partial class TeacherView : ReactiveUserControl<TeacherViewModel>
{
    public TeacherView()
    {
        InitializeComponent();
        this.WhenActivated((d) => ViewModel!.ShowTestInteraction.RegisterHandler(ShowTestWindowHandler));
    }

    private async Task ShowTestWindowHandler(InteractionContext<TestViewModel, TestViewModel?> obj)
    {
        var dialog = new TeacherTestWindowView() { DataContext = obj.Input };
        var mainW = Locator.GetLocator().GetService<MainWindow>();
        
        var result = await dialog.ShowDialog<TestViewModel?>(mainW);
        obj.SetOutput(result);
    }
}