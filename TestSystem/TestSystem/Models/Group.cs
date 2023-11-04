using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Splat;
using TestSystem.ViewModels;

namespace TestSystem.Models;

public class Group
{
    public int Id { get; set; }

    public string GroupName { get; set; } = null!;
    
    public static async Task<IEnumerable<GroupViewModel>> GetGroups()
    {
        if (Locator.GetLocator().GetService<TestingSystemDbContext>() == null)
        {
            return Array.Empty<GroupViewModel>();
        }
        
        var group = await Locator.GetLocator().GetService<TestingSystemDbContext>()!.Group.ToListAsync();

        return group.Select(s => new GroupViewModel(s)).ToList();
    }
}