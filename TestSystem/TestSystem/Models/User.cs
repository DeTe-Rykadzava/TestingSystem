using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Splat;
using TestSystem.ViewModels;

namespace TestSystem.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string SecondName { get; set; } = null!;
    public string? LastName { get; set; }
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Role UserRole { get; set; } = null!;
    
    public Group? Group { get; set; }
    

    private static UserViewModel? _currentUser;
    public static UserViewModel? GetCurrentUser()
    {
        return _currentUser;
    }

    public static async Task<UserViewModel?> GetUserByLogAndPas(string login, string password)
    {
        try
        {
            var user = await Locator.GetLocator().GetService<TestingSystemDbContext>()!.User.FirstOrDefaultAsync(x =>
                x.Login == login);
            if (user == null)
                return null;
            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                return null;
            var vm = new UserViewModel(user);
            _currentUser = vm;
            return vm;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public static async Task<UserViewModel?> RegisterNewUser(UserViewModel user)
    {
        // processing user ... 

        var newUser = new User()
        {
            FirstName = user.FirstName,
            SecondName = user.SecondName,
            LastName = user.LastName,
            Login = user.UserLogin,
            Password = BCrypt.Net.BCrypt.HashPassword(user.UserPassword),
            UserRole = user.Role.Role
        };

        try
        {
            if (Locator.GetLocator().GetService<TestingSystemDbContext>() == null)
            {
                return null;
            }
            
            await Locator.GetLocator().GetService<TestingSystemDbContext>()!.User.AddAsync(newUser);
            await Locator.GetLocator().GetService<TestingSystemDbContext>()!.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
        
        _currentUser = user;
        return _currentUser;
    }
}