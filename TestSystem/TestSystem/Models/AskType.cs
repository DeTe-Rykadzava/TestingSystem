using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Splat;
using TestSystem.ViewModels;

namespace TestSystem.Models;

public class AskType
{
    public int Id { get; set; }
    public string Type { get; set; } = null!;

    private AskType()
    {
    }

    private AskType(string type)
    {
        Type = type;
    }

    public static async Task<AskTypeViewModel?> GetTypeByName(string typeName)
    {
        if (Locator.GetLocator().GetService<TestingSystemDbContext>() == null)
        {
            return null;
        }

        var type = (await GetAskTypes()).FirstOrDefault(x => x.TypeName == typeName);
        return type;
    }
    
    private static async Task<IEnumerable<AskTypeViewModel>> GetAskTypes()
    {
        if (Locator.GetLocator().GetService<TestingSystemDbContext>() == null)
        {
            return Array.Empty<AskTypeViewModel>();
        }
        
        var roles = await Locator.GetLocator().GetService<TestingSystemDbContext>()!.AskType.ToListAsync();
        if (!roles.Any())
        {
            roles = await CreateBaseAskTypes();
        }

        return roles.Select(s => new AskTypeViewModel(s)).ToList();
    }

    private static async Task<List<AskType>> CreateBaseAskTypes()
    {
        var types = new List<AskType>();
        
        // select
        var adminRole = new AskType("Select");
        types.Add(adminRole);
        
        await TestingSystemDbContext.Instance.AskType.AddRangeAsync(types);
        
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