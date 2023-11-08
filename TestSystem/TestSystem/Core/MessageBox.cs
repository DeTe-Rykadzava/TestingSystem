using System.Threading.Tasks;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace TestSystem.Core;

public static class MessageBox
{
    public static async Task ShowMessageBox(string title, string text)
    {
        var box = MessageBoxManager
            .GetMessageBoxStandard(title, text,
                ButtonEnum.Ok);

        var result = await box.ShowAsync();
    }
}