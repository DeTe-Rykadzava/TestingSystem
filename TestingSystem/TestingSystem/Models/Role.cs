using System;
using System.Collections.Generic;
using System.Linq;
using TestingSystem.ViewModels;

namespace TestingSystem.Models;

public class Role
{
    public int Id { get; set; }
    public string RoleName { get; set; } = null!;

    private Role(string roleName)
    {
        RoleName = roleName;
    }

    public static IEnumerable<RoleViewModel> GetRoles()
    {
        var roles = TestingSystemDbContext.Instance.Role.ToList();
        if (!roles.Any())
        {
            roles = CreateBaseRoles();
        }

        foreach (var role in roles)
        {
            yield return new RoleViewModel(role);
        }
    }

    private static List<Role> CreateBaseRoles()
    {
        var roles = new List<Role>();
        
        // admin
        var adminRole = new Role("Admin");
        TestingSystemDbContext.Instance.Role.AddAsync(adminRole);
        
        // teacher
        var teacherRole = new Role("Teacher");
        TestingSystemDbContext.Instance.Role.AddAsync(teacherRole);
        
        // student
        var studentRole = new Role("student");
        TestingSystemDbContext.Instance.Role.AddAsync(studentRole);
        
        try
        {
            TestingSystemDbContext.Instance.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return roles;

    }
}