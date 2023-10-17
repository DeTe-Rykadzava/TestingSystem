using System;
using System.IO;
using System.Text.Json.Nodes;

namespace TestingSystem.Core;

public class Settings
{
    private static JsonNode? GetAppSettingsJson()
    {
        var directory = Directory.GetCurrentDirectory();
        var pathToSettings = Path.Combine(directory, "appSetting.json");
        Console.WriteLine("Path to settings: " + pathToSettings);
        var settingsJsonString = File.ReadAllText(pathToSettings);
        var settings = JsonObject.Parse(settingsJsonString);
        return settings;
    }

    public static string GetConnectionString()
    {
        var settings = GetAppSettingsJson();
        if (settings != null)
            return settings["ConnectionString"]!["DefaultConnection"]!.GetValue<string>();
        throw new Exception("Cannot get connection string");
    }
}