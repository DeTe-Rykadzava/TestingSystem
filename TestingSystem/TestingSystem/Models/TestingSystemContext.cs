using System;
using System.IO;
using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TestingSystem.Models;

public class TestingSystemDbContext : DbContext
{
    public DbSet<User> User { get; set; } = null!;
    public DbSet<Role> Role { get; set; } = null!;

    public TestingSystemDbContext(DbContextOptions<TestingSystemDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = GetConnectionString();
        Console.WriteLine(connectionString);
        
        optionsBuilder.UseNpgsql(connectionString);
    }

    private string GetConnectionString()
    {
        var directory = Directory.GetCurrentDirectory();
        var pathToSettings = Path.Combine(directory, "appSetting.json");
        Console.WriteLine("Path to settings: " + pathToSettings);
        var settingsJsonString = File.ReadAllText(pathToSettings);
        var settings = JsonObject.Parse(settingsJsonString);
        if (settings != null)
            return settings["ConnectionString"]!["DefaultConnection"]!.GetValue<string>();
        return "";
    }
}