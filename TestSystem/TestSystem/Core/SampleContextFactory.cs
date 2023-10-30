using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TestSystem.Models;

namespace TestSystem.Core;

public class SampleContextFactory : IDesignTimeDbContextFactory<TestingSystemDbContext>
{
    public TestingSystemDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TestingSystemDbContext>();

        string connectionString = Settings.GetConnectionString();
        optionsBuilder.UseNpgsql(connectionString);
        return new TestingSystemDbContext(optionsBuilder.Options);
    }
}