using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Splat;
using TestSystem.ViewModels;

namespace TestSystem.Models;

public class QueryType
{
    public int Id { get; set; }
    public string Type { get; set; } = null!;

    private QueryType()
    {
    }

    private QueryType(string typeName)
    {
        Type = typeName;
    }
    
    // public static async Task<AskTypeViewModel?> GetTypeByName(string typeName)
    // {
    //     if (Locator.GetLocator().GetService<TestingSystemDbContext>() == null)
    //     {
    //         return null;
    //     }
    //
    //     var type = (await GetAskTypes()).FirstOrDefault(x => x.TypeName == typeName);
    //     return type;
    // }
    
    public static async Task<List<QueryTypeViewModel>> GetQueryTypes()
    {
        if (Locator.GetLocator().GetService<TestingSystemDbContext>() == null)
        {
            return new List<QueryTypeViewModel>();
        }
        
        var roles = await Locator.GetLocator().GetService<TestingSystemDbContext>()!.QueryType.ToListAsync();
        if (!roles.Any())
        {
            roles = await CreateBaseAskTypes();
        }
    
        return roles.Select(s => new QueryTypeViewModel(s)).ToList();
    }
    
    private static async Task<List<QueryType>> CreateBaseAskTypes()
    {
        var types = new List<QueryType>();
        
        // select
        var selectOneType = new QueryType("one answer");
        types.Add(selectOneType);
        
        var selectManyType = new QueryType("many answer");
        types.Add(selectManyType);
        
        await TestingSystemDbContext.Instance.QueryType.AddRangeAsync(types);
        
        try
        {
            await TestingSystemDbContext.Instance.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return types;
    }
}