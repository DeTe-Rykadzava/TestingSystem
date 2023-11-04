using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Splat;
using TestSystem.ViewModels;

namespace TestSystem.Models;

public class Role
{
    public int Id { get; set; }
    public string RoleName { get; set; }

    private Role(string roleName)
    {
        RoleName = roleName;
    }

    public static async Task<RoleViewModel?> GetRoleByName(string roleName)
    {
        if (Locator.GetLocator().GetService<TestingSystemDbContext>() == null)
        {
            return null;
        }

        var role = (await GetRoles()).FirstOrDefault(x => x.RoleName == roleName);
        return role;
    }
    
    private static async Task<IEnumerable<RoleViewModel>> GetRoles()
    {
        if (Locator.GetLocator().GetService<TestingSystemDbContext>() == null)
        {
            return Array.Empty<RoleViewModel>();
        }
        
        var roles = Locator.GetLocator().GetService<TestingSystemDbContext>()!.Role.ToList();
        if (!roles.Any())
        {
            roles = await CreateBaseRoles();
        }

        return roles.Select(s => new RoleViewModel(s)).ToList();
    }

    private static async Task<List<Role>> CreateBaseRoles()
    {
        var roles = new List<Role>();
        
        // admin
        var adminRole = new Role("Admin");
        roles.Add(adminRole);
        // teacher
        var teacherRole = new Role("Teacher");
        roles.Add(teacherRole);
        // student
        var studentRole = new Role("student");
        roles.Add(studentRole);

        await TestingSystemDbContext.Instance.Role.AddRangeAsync(roles);
        
        try
        {
            await TestingSystemDbContext.Instance.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return roles;
    }
}