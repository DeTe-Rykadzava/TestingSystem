using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestSystem.ViewModels;

namespace TestSystem.Models;

public class Role
{
    public int Id { get; set; }
    public string RoleName { get; set; } = null!;

    private Role(string roleName)
    {
        RoleName = roleName;
    }

    public static async Task<RoleViewModel?> GetRoleByName(string roleName)
    {
        var role = await TestingSystemDbContext.Instance.Role.FirstOrDefaultAsync(x => x.RoleName == roleName);

        return role == null ? null : new RoleViewModel(role);
    }
    
    public static async IAsyncEnumerable<RoleViewModel> GetRoles()
    {
        var roles = TestingSystemDbContext.Instance.Role.ToList();
        if (!roles.Any())
        {
            roles = await CreateBaseRoles();
        }

        foreach (var role in roles)
        {
            yield return new RoleViewModel(role);
        }
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