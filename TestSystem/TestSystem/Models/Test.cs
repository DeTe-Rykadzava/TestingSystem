﻿using System;
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
    public List<QueryTest> Asks { get; set; } = null!;

    private Test()
    {
        
    }
    
    private Test(User creatorUser)
    {
        CreatorUser = creatorUser;
    }
    
    // public List<TestAskViewModel> GetTestAsks()
    // {
    //     if (Asks == null)
    //         return new List<TestAskViewModel>();
    //     var asks = Asks.Select(s => TestAskViewModel.GetTestAsk(s)).ToList();
    //     return asks;
    // }

    public async Task<QueryTestTeacherViewModel?> AddQuery(QueryTypeViewModel type)
    {
        return null;
    }

    public static async Task<List<TeacherTestViewModel>> GetAllUserTests()
    {
        var userId = User.GetCurrentUser().UserId;
        var tests = await Locator.GetLocator().GetService<TestingSystemDbContext>().Test
            .Include(i => i.CreatorUser)
            .Include(i => i.Asks)
            .Where(x => x.CreatorUser.Id == userId)
            .ToListAsync();
        var valTests = tests.Select(s => new TeacherTestViewModel(s)).ToList();
        return valTests;
    }
    public static async Task<TeacherTestViewModel> CreateNewBlackTest()
    {
        var userId = User.GetCurrentUser()!.UserId;
        var user = await Locator.GetLocator().GetService<TestingSystemDbContext>()!.User.FirstOrDefaultAsync(x => x.Id == userId)!;
        var newTest = new Test(user!);
        newTest.Name = $"New test {DateTime.Now}";
        await Locator.GetLocator().GetService<TestingSystemDbContext>()!.Test.AddAsync(newTest);
        await Locator.GetLocator().GetService<TestingSystemDbContext>()!.SaveChangesAsync();
        return new TeacherTestViewModel(newTest, true);
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

    public static void CanselDeleteTest(Test test)
    {
        Locator.GetLocator().GetService<TestingSystemDbContext>().Entry(test).State = EntityState.Unchanged;
    }

    public async Task ResetChanges()
    {
        var originalTest = (Test)Locator.GetLocator().GetService<TestingSystemDbContext>().Entry(this).OriginalValues.ToObject();
        Name = originalTest.Name;
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
    //
    // public void ResetChanges()
    // {
    //     var originalEntity = (Test)Locator.GetLocator().GetService<TestingSystemDbContext>().Entry(this).OriginalValues.ToObject();
    //     this.Name = originalEntity.Name;
    // }
    //
    // public static async Task<TestViewModel?> GetTestById(int testId)
    // {
    //     var userId = User.GetCurrentUser().UserId;
    //     var test = await Locator.GetLocator().GetService<TestingSystemDbContext>().Test
    //         .Include(i => i.CreatorUser)
    //         .Include(i => i.Asks)
    //         .FirstOrDefaultAsync(x => x.CreatorUser.Id == userId && x.Id == testId);
    //     return test == null ? null : TestViewModel.GetTest(test, false);
    // }
}