using System;
using System.IO;
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
        var path = Directory.GetCurrentDirectory();
        path = path.Replace("\\", "/"); // Replace backslashes with forward slashes
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(path);
        builder.AddJsonFile("appSetting.json");
        IConfiguration config = builder.Build();
        Console.WriteLine(config.GetConnectionString("DefaultConnection"));
        
        optionsBuilder.UseNpgsql(config.GetConnectionString("DefaultConnection"));
    }
}