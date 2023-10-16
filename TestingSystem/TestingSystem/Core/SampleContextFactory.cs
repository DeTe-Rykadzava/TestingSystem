using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TestingSystem.Models;

namespace TestingSystem.Core;

public class SampleContextFactory : IDesignTimeDbContextFactory<TestingSystemDbContext>
{
    public TestingSystemDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TestingSystemDbContext>();

        var path = Directory.GetCurrentDirectory();
        
        ConfigurationBuilder builder = new ConfigurationBuilder();
        builder.SetBasePath(path);
        builder.AddJsonFile("appSetting.json");
        IConfigurationRoot config = builder.Build();

        string connectionString = config.GetConnectionString("DefaultConnection");
        optionsBuilder.UseNpgsql(connectionString);
        return new TestingSystemDbContext(optionsBuilder.Options);
    }
}