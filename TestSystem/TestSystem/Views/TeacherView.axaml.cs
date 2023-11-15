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
    }
    
}