using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Splat;
using TestSystem.ViewModels;

namespace TestSystem.Models;

public class QueryTest
{
    public int Id { get; set; }
    public string Ask { get; set; } = null!;
    
    public QueryType Type { get; set; } = null!;
    
    public List<QueryAnswer> Answers { get; set; } = null!;
    
    private QueryTest()
    {
        
    }

    // public List<AskAnswerViewModel> GetAskAnswers()
    // {
    //     if (Answers == null)
    //         return new List<AskAnswerViewModel>();
    //     var answers = Answers.Select(s => AskAnswerViewModel.GetAnswer(s)).ToList();
    //     return answers;
    // }
    //
    // public async void SetRightAnswer(int rightAnswerId)
    // {
    //     var answer = await Locator.GetLocator().GetService<TestingSystemDbContext>().AskAnswer
    //         .FirstOrDefaultAsync(x => x.Id == rightAnswerId);
    //     if (answer != null)
    //         RightAnswer = answer;
    // }
    //
    // public static async Task<TestAskViewModel> CreateNewTestAsk(Test test)
    // {
    //     var newTestAsk = new QueryTest
    //     {
    //         Ask = $"New ask {DateTime.Now}",
    //         Type = (await QueryType.GetTypeByName("Select"))!.Type,
    //         Answers = new List<QueryAnswer>()
    //     };
    //     (await QueryAnswer.CreateNewTestAsk()).SetAnswer(out QueryAnswer answer);
    //     newTestAsk.Answers.Add(answer);
    //     newTestAsk.RightAnswer = answer;
    //     if (test.Asks == null)
    //         test.Asks = new List<QueryTest>();
    //     test.Asks.Add(newTestAsk);
    //     await Locator.GetLocator().GetService<TestingSystemDbContext>()!.TestAsk.AddAsync(newTestAsk);
    //     Locator.GetLocator().GetService<TestingSystemDbContext>()!.Entry(test).State = EntityState.Modified;
    //     await Locator.GetLocator().GetService<TestingSystemDbContext>()!.SaveChangesAsync();
    //     return TestAskViewModel.GetTestAsk(newTestAsk, true);
    // }
    //
    // public async Task<bool> SaveChanges()
    // {
    //     try
    //     {
    //         await Locator.GetLocator().GetService<TestingSystemDbContext>().SaveChangesAsync();
    //         return true;
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         return false;
    //     }
    // }
    //
    // public static async Task<bool> DeleteAsk(QueryTest ask)
    // {
    //     try
    //     {
    //         Locator.GetLocator().GetService<TestingSystemDbContext>().TestAsk.Remove(ask);
    //         await Locator.GetLocator().GetService<TestingSystemDbContext>().SaveChangesAsync();
    //         return true;
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         return false;
    //     }
    // }
}