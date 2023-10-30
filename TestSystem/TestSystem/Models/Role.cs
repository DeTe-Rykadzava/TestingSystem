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
        await TestingSystemDbContext.Instance.Role.AddAsync(adminRole);
        
        // teacher
        var teacherRole = new Role("Teacher");
        await TestingSystemDbContext.Instance.Role.AddAsync(teacherRole);
        
        // student
        var studentRole = new Role("student");
        await TestingSystemDbContext.Instance.Role.AddAsync(studentRole);
        
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