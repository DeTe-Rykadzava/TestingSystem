using TestSystem.Models;

namespace TestSystem.ViewModels;

public class AskTypeViewModel : ViewModelBase
{
    public readonly AskType Type;

    public int TypeId => Type.Id;

    public string TypeName => Type.Type;
    
    public AskTypeViewModel(AskType type)
    {
        Type = type;
    }
}