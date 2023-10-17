﻿using System;
using System.IO;
using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TestingSystem.Core;

namespace TestingSystem.Models;

public class TestingSystemDbContext : DbContext
{
    private static TestingSystemDbContext? _instance = null;

    public static TestingSystemDbContext Instance
    {
        get
        {
            if (_instance != null)
                return _instance;
            return GetNewInstance();
        }
    }

    public DbSet<User> User { get; set; } = null!;
    public DbSet<Role> Role { get; set; } = null!;

    public TestingSystemDbContext(DbContextOptions<TestingSystemDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = Settings.GetConnectionString();
        Console.WriteLine(connectionString);
        
        optionsBuilder.UseNpgsql(connectionString);
    }
    
    private static TestingSystemDbContext GetNewInstance()
    {
        var optionsBuilder = new DbContextOptionsBuilder<TestingSystemDbContext>();
        var context = new TestingSystemDbContext(optionsBuilder.Options);
        return context;
    }
    
}