using System;
using System.IO;
using System.Text.Json;
class SettingsManager
{
    public static string jsonFilePath = "./settings.json";
    public static string countdownText;
    public static string countdownOverText;
    public static string countdownFormat;
    public static string filePath;
    public static Boolean autoStartCountdown;
    public static DateTime autoCountdownDateTime;
    public static DayOfWeek autoCountdownDay;
    public static string autoCountdownTime;

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
                countdownFormat = settings.CountdownFormat;
                filePath = settings.FilePath;
                autoStartCountdown = settings.AutoStartCountdown;
                autoCountdownDateTime = settings.AutoCountdownDateTime;
                autoCountdownDay = settings.AutoCountdownDay;
                autoCountdownTime = settings.AutoCountdownTime;

            }
            else
            {
                Console.WriteLine("No settings found");
            }
        } catch (FileNotFoundException)
        {
            Console.WriteLine("File not found");
        }
    }

    public static void SaveSettings()
    {
        var settings = new AppSettings
        {
            CountdownText = countdownText,
            CountdownOverText = countdownOverText,
            CountdownFormat = countdownFormat,
            FilePath = filePath,
            AutoStartCountdown = autoStartCountdown,
            AutoCountdownDateTime = autoCountdownDateTime,
            AutoCountdownDay = autoCountdownDay,
            AutoCountdownTime = autoCountdownTime,

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
            CountdownFormat = countdownFormat,
            FilePath = filePath,
            AutoStartCountdown = autoStartCountdown,
            AutoCountdownDateTime = autoCountdownDateTime,
            AutoCountdownDay = autoCountdownDay,
            AutoCountdownTime = autoCountdownTime,
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

    public static string GetCountdownFormat()
    {
        return countdownFormat;
    }
    public static string GetFilePath()
    {
        return filePath;
    }

    public static Boolean GetAutoStartCountdown()
    {
        return autoStartCountdown;
    }

    public static DateTime GetAutoCountdownDateTime()
    {
        return autoCountdownDateTime;
    }

    public static DayOfWeek GetAutoCountdownDay()
    {
        return autoCountdownDay;
    }

    public static string GetAutoCountdownTime()
    {
        return autoCountdownTime;
    }
    public static void SetCountdownText(string newText)
    {
        countdownText = newText;
    }

    public static void SetCountdownOverText(string newText)
    {
        countdownOverText = newText;
    }

    public static void SetCountdownFormat(string newFormat)
    {
        countdownFormat = newFormat;
    }
    public static void SetFilePath(string newFilePath)
    {
        filePath = newFilePath;
    }

    public static void SetAutoStartCountdown(Boolean newValue)
    {
        autoStartCountdown = newValue;
        SaveSettings();
    }

    public static void SetAutoCountdownDateTime(DateTime newAutoCountdownDateTime)
    {
        autoCountdownDateTime = newAutoCountdownDateTime;

        SaveSettings();
    }

    public static void SetAutoCountdownDay(DayOfWeek newAutoCountdownDay)
    {
        autoCountdownDay = newAutoCountdownDay;
    }

    public static void SetAutoCountdownTime(string newAutoCountdownTime)
    {
        autoCountdownTime = newAutoCountdownTime;
    }
}