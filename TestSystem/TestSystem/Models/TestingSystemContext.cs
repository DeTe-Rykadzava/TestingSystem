using System;
using System.IO;
using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TestSystem.Core;

namespace TestSystem.Models;

public class TestingSystemDbContext : DbContext
{
    private static TestingSystemDbContext? _instance = null;

    public static TestingSystemDbContext Instance
    {
        get
        {
            if (_instance != null)
                return _instance;
            return _instance = GetNewInstance();
        }
    }

    public DbSet<User> User { get; set; } = null!;
    
    public DbSet<Role> Role { get; set; } = null!;
    
    public DbSet<Group> Group { get; set; } = null!;

    public DbSet<Test> Test { get; set; } = null!;
    
    public DbSet<AskType> AskType { get; set; } = null!;
    
    public DbSet<AskAnswer> AskAnswer { get; set; } = null!;
    
    public DbSet<TestAsk> TestAsk { get; set; } = null!;
    
    public DbSet<TestUser> TestUser { get; set; } = null!;

    public TestingSystemDbContext(DbContextOptions<TestingSystemDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = Settings.GetConnectionString();
        // Console.WriteLine(connectionString);
        
        optionsBuilder.UseNpgsql(connectionString);
    }
    
    private static TestingSystemDbContext GetNewInstance()
    {
        var optionsBuilder = new DbContextOptionsBuilder<TestingSystemDbContext>();
        var context = new TestingSystemDbContext(optionsBuilder.Options);
        return context;
    }
    
}