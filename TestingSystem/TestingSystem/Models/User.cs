using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestingSystem.ViewModels;

namespace TestingSystem.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string SecondName { get; set; } = null!;
    public string? LastName { get; set; }
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Role UserRole { get; set; } = null!;

    public static async Task<UserViewModel?> GetUserByLogAndPas(string login, string password)
    {
        try
        {
            var user = await TestingSystemDbContext.Instance.User.FirstOrDefaultAsync(x =>
                x.Login == login && x.Password == password);
            if (user == null)
                return null;
            var vm = new UserViewModel(user);
            return vm;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public static async Task<UserViewModel> RegisterNewUser(UserViewModel userVM)
    {
        // processing userVM ... 
        
        return userVM;
    }
}