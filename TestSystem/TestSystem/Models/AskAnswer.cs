using System;
using System.Threading.Tasks;
using Splat;
using TestSystem.ViewModels;

namespace TestSystem.Models;

public class AskAnswer
{
    public int Id { get; set; }
    public string AnswerValue { get; set; } = null!;

    private AskAnswer(string answerValue)
    {
        AnswerValue = answerValue;
    }
    
    
    public static async Task<AskAnswerViewModel> CreateNewTestAsk()
    {
        var askAnswer = new AskAnswer($"new answer {DateTime.Now}");
        await Locator.GetLocator().GetService<TestingSystemDbContext>()!.AskAnswer.AddAsync(askAnswer);
        await Locator.GetLocator().GetService<TestingSystemDbContext>()!.SaveChangesAsync();
        return AskAnswerViewModel.GetAnswer(askAnswer, true);
    }
}