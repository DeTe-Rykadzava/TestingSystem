using System;
using System.Threading.Tasks;
using Splat;
using TestSystem.ViewModels;

namespace TestSystem.Models;

public class QueryAnswer
{
    public int Id { get; set; }
    public string Answer { get; set; } = null!;

    public bool IsCorrect { get; set; }

    private QueryAnswer()
    {
        
    }

    private QueryAnswer(string answerValue)
    {
        Answer = answerValue;
    }

    public static async Task<TeacherQueryAnswerViewModel?> CreateQueryAnswer(QueryTest query)
    {
        try
        {
            var newAnswer = new QueryAnswer
            {
                Answer = $"new query answer {DateTime.Now}"
            };
            await Locator.GetLocator().GetService<TestingSystemDbContext>().QueryAnswer.AddAsync(newAnswer);
            await Locator.GetLocator().GetService<TestingSystemDbContext>().SaveChangesAsync();
            query.Answers.Add(newAnswer);
            return new TeacherQueryAnswerViewModel(newAnswer);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message + "\n\n"+ e.InnerException);
            return null;
        }
    }
    
    public async Task<bool> DeleteAnswer()
    {
        try
        {
            Locator.GetLocator().GetService<TestingSystemDbContext>().QueryAnswer.Remove(this);
            await Locator.GetLocator().GetService<TestingSystemDbContext>().SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task ResetChanges()
    {
        var originalValues = (QueryAnswer)Locator.GetLocator().GetService<TestingSystemDbContext>().Entry(this).OriginalValues.ToObject();
        Answer = originalValues.Answer;
    }
}