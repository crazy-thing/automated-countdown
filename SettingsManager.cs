using System;
using System.IO;
using System.Text.Json;
class SettingsManager
{
    public static string jsonFilePath = "./settings.json";
    public static string countdownText;
    public static string countdownOverText;
    public static string filePath;

    public static void LoadSettings()
    {
        try
        {
            string json = File.ReadAllText(jsonFilePath);
            var settings = JsonSerializer.Deserialize<AppSettings>(json);

            if (settings != null)
            {
                countdownText = settings.CountdownText;
                countdownOverText = settings.CountdownOverText;
                filePath = settings.FilePath;
            }
            else
            {
                Console.WriteLine("No settings found");
            }
        } catch (FileNotFoundException)
        {
            Console.WriteLine("File not found");
        }

        Countdown.ReloadSettings();
    }

    public static void SaveSettings()
    {
        var settings = new AppSettings
        {
            CountdownText = countdownText,
            CountdownOverText = countdownOverText,
            FilePath = filePath
        };

        string settingsJson = JsonSerializer.Serialize(settings);
        File.WriteAllText(jsonFilePath, settingsJson);
        Console.WriteLine("Settings saved");
        LoadSettings();
    }

    public static AppSettings GetCountdownSettings()
    {
        return new AppSettings{
            CountdownText = countdownText,
            CountdownOverText = countdownOverText,
            FilePath = filePath,
        };

    }

    public static string GetCountdownText()
    {
        return countdownText;
    }

    public static string GetCountdownOverText()
    {
        return countdownOverText;
    }

    public static string GetFilePath()
    {
        return filePath;
    }

    public static void SetCountdownText(string newText)
    {
        countdownText = newText;
    }

    public static void SetCountdownOverText(string newText)
    {
        countdownOverText = newText;
    }

    public static void SetFilePath(string newFilePath)
    {
        filePath = newFilePath;
    }


}