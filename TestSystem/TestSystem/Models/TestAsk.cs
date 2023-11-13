using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Splat;
using TestSystem.ViewModels;

namespace TestSystem.Models;

public class TestAsk
{
    public int Id { get; set; }
    public string Ask { get; set; } = null!;
    public AskType Type { get; set; } = null!;
    public List<AskAnswer> Answers { get; set; } = null!;
    public AskAnswer RightAnswer { get; set; } = null!;
    public List<TestAsk>? SubAsk { get; set; } = null;
    
    private TestAsk()
    {
        
    }

    public List<AskAnswerViewModel> GetAskAnswers()
    {
        if (Answers == null)
            return new List<AskAnswerViewModel>();
        var answers = Answers.Select(s => AskAnswerViewModel.GetAnswer(s)).ToList();
        return answers;
    }
    
    public async void SetRightAnswer(int rightAnswerId)
    {
        var answer = await Locator.GetLocator().GetService<TestingSystemDbContext>().AskAnswer
            .FirstOrDefaultAsync(x => x.Id == rightAnswerId);
        if (answer != null)
            RightAnswer = answer;
    }

    public static async Task<TestAskViewModel> CreateNewTestAsk(Test test)
    {
        var newTestAsk = new TestAsk
        {
            Ask = $"New ask {DateTime.Now}",
            Type = (await AskType.GetTypeByName("Select"))!.Type,
            Answers = new List<AskAnswer>()
        };
        (await AskAnswer.CreateNewTestAsk()).SetAnswer(out AskAnswer answer);
        newTestAsk.Answers.Add(answer);
        newTestAsk.RightAnswer = answer;
        if (test.Asks == null)
            test.Asks = new List<TestAsk>();
        test.Asks.Add(newTestAsk);
        await Locator.GetLocator().GetService<TestingSystemDbContext>()!.TestAsk.AddAsync(newTestAsk);
        Locator.GetLocator().GetService<TestingSystemDbContext>()!.Entry(test).State = EntityState.Modified;
        await Locator.GetLocator().GetService<TestingSystemDbContext>()!.SaveChangesAsync();
        return TestAskViewModel.GetTestAsk(newTestAsk, true);
    }

    public async Task<bool> SaveChanges()
    {
        try
        {
            await Locator.GetLocator().GetService<TestingSystemDbContext>().SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
    
    public static async Task<bool> DeleteAsk(TestAsk ask)
    {
        try
        {
            Locator.GetLocator().GetService<TestingSystemDbContext>().TestAsk.Remove(ask);
            await Locator.GetLocator().GetService<TestingSystemDbContext>().SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}