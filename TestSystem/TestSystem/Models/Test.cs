using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Splat;
using TestSystem.ViewModels;

namespace TestSystem.Models;

public class Test
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public User CreatorUser { get; set; } = null!;
    public List<TestAsk> Asks { get; set; } = null!;

    private Test()
    {
        
    }
    
    private Test(User creatorUser)
    {
        CreatorUser = creatorUser;
    }

    public static async Task<TestViewModel> CreateNewBlackTest()
    {
        var userId = User.GetCurrentUser()!.UserId;
        var user = await Locator.GetLocator().GetService<TestingSystemDbContext>()!.User.FirstOrDefaultAsync(x => x.Id == userId)!;
        var newTest = new Test(user!);
        newTest.Name = $"new Test {DateTime.Now}";
        await Locator.GetLocator().GetService<TestingSystemDbContext>()!.Test.AddAsync(newTest);
        await Locator.GetLocator().GetService<TestingSystemDbContext>()!.SaveChangesAsync();
        return TestViewModel.GetTest(newTest, true);
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

    public static async Task<TestViewModel?> GetTestById(int testId)
    {
        var userId = User.GetCurrentUser().UserId;
        var test = await Locator.GetLocator().GetService<TestingSystemDbContext>().Test
            .Include(i => i.CreatorUser)
            .Include(i => i.Asks)
            .FirstOrDefaultAsync(x => x.CreatorUser.Id == userId && x.Id == testId);
        return test == null ? null : TestViewModel.GetTest(test, false);
    }

    public static async Task<List<TestViewModel>> GetAllUserTests()
    {
        var userId = User.GetCurrentUser().UserId;
        var tests = Locator.GetLocator().GetService<TestingSystemDbContext>().Test
            .Include(i => i.CreatorUser)
            .Include(i => i.Asks)
            .Where(x => x.CreatorUser.Id == userId)
            .AsEnumerable()
            .Select( s => TestViewModel.GetTest(s, false))
            .ToList();
        return tests;
    }

    public static async Task<bool> DeleteTest(Test test)
    {
        try
        {
            Locator.GetLocator().GetService<TestingSystemDbContext>().Test.Remove(test);
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