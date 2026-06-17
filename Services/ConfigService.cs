using System.IO;
using System.Text.Json;

namespace Project_03_TestPilot_20260617.Services;

public class Config
{
    public string TargetAppPath { get; set; } = "";
    public string TargetProcessName { get; set; } = "";
    public string LaunchHotKey { get; set; } = "F11";
    public string KillHotKey { get; set; } = "F12";
}

public class ConfigService
{
    private static readonly string ConfigPath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

    public Config Load()
    {
        try
        {
            if (!File.Exists(ConfigPath))
            {
                var defaultConfig = new Config();
                Save(defaultConfig);
                return defaultConfig;
            }

            var json = File.ReadAllText(ConfigPath);
            var config = JsonSerializer.Deserialize<Config>(json);
            if (config == null) throw new JsonException("Deserialized null");

            if (string.IsNullOrEmpty(config.TargetProcessName) && !string.IsNullOrEmpty(config.TargetAppPath))
                config.TargetProcessName = Path.GetFileNameWithoutExtension(config.TargetAppPath);

            return config;
        }
        catch
        {
            var fallback = new Config();
            Save(fallback);
            return fallback;
        }
    }

    public void Save(Config config)
    {
        if (string.IsNullOrEmpty(config.TargetProcessName) && !string.IsNullOrEmpty(config.TargetAppPath))
            config.TargetProcessName = Path.GetFileNameWithoutExtension(config.TargetAppPath);

        var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(ConfigPath, json);
    }
}
