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
    public string Query { get; set; } = null!;
    
    public QueryType Type { get; set; } = null!;
    
    public List<QueryAnswer> Answers { get; set; } = null!;
    
    private QueryTest()
    {
        
    }

    public List<TeacherQueryAnswerViewModel> GetQueryAnswers()
    {
        if (Answers == null)
            return new List<TeacherQueryAnswerViewModel>();
        var answers = Answers.Select(s => new TeacherQueryAnswerViewModel(s)).ToList();
        return answers;
    }
    
    public static async Task<QueryTestTeacherViewModel?> CreateNewQuery(Test test, QueryTypeViewModel queryType )
    {
        var type = await Locator.GetLocator().GetService<TestingSystemDbContext>()!.QueryType
            .FirstOrDefaultAsync(x => x.Id == queryType.Id);
        if (type == null)
            return null;
        var newQuery = new QueryTest
        {
            Query = $"New ask {DateTime.Now}",
            Type = type,
            Answers = new()
        };
        await QueryAnswer.CreateQueryAnswer(newQuery);
        test.Queries ??= new List<QueryTest>();
        test.Queries.Add(newQuery);
        await Locator.GetLocator().GetService<TestingSystemDbContext>()!.QueryTest.AddAsync(newQuery);
        Locator.GetLocator().GetService<TestingSystemDbContext>()!.Entry(test).State = EntityState.Modified;
        await Locator.GetLocator().GetService<TestingSystemDbContext>()!.SaveChangesAsync();
        switch (type.Type)
        {
            case "one answer":
                return new QueryTestTeacherOneAnswerViewModel(newQuery);
                break;
            case "many answer":
                return new QueryTestTeacherManyAnswerViewModel(newQuery);
                break;
            default:
                throw new Exception("cannot determine query type");
        }
    }
    
    public async Task<bool> DeleteQuery()
    {
        try
        {
            Locator.GetLocator().GetService<TestingSystemDbContext>().QueryTest.Remove(this);
            await Locator.GetLocator().GetService<TestingSystemDbContext>().SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
    
    public void CanselDeleteTest()
    {
        Locator.GetLocator().GetService<TestingSystemDbContext>().Entry(this).State = EntityState.Unchanged;
    }
    
    public async Task ResetChanges()
    {
        var originalQuery = (QueryTest)Locator.GetLocator().GetService<TestingSystemDbContext>().Entry(this).OriginalValues.ToObject();
        Query = originalQuery.Query;
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
}