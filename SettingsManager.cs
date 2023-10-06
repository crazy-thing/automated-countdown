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
    public static Boolean autoStartBibleVersesLoop;
    public static int bibleVersesLoopInterval;
    public static string bibleVersesFilePath;
    public static string bibleVersesTranslation;
    public static string bibleVersesGenre;

    public static void LoadSettings()
    {
        try
        {
            if (!File.Exists(jsonFilePath))
            {
                var defaultSettings = new AppSettings();
                string settingsJson = JsonSerializer.Serialize(defaultSettings);
                File.WriteAllText(jsonFilePath, settingsJson);
            }

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
                autoStartBibleVersesLoop = settings.AutoStartBibleVersesLoop;
                bibleVersesLoopInterval = settings.BibleVersesLoopInterval;
                bibleVersesFilePath = settings.BibleVersesFilePath;
                bibleVersesTranslation = settings.BibleVersesTranslation;
                bibleVersesGenre = settings.BibleVersesGenre;

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
            AutoStartBibleVersesLoop = autoStartBibleVersesLoop,
            BibleVersesLoopInterval = bibleVersesLoopInterval,
            BibleVersesFilePath = bibleVersesFilePath,
            BibleVersesTranslation = bibleVersesTranslation,
            BibleVersesGenre = bibleVersesGenre,

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
            AutoStartBibleVersesLoop = autoStartBibleVersesLoop,
            BibleVersesLoopInterval = bibleVersesLoopInterval,
            BibleVersesFilePath = bibleVersesFilePath,
            BibleVersesTranslation = bibleVersesTranslation,
            BibleVersesGenre = bibleVersesGenre,
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
    public static Boolean GetAutoStartBibleVersesLoop()
    {
        return autoStartBibleVersesLoop;
    }
    public static int GetBibleVersesLoopInterval()
    {
        return bibleVersesLoopInterval;
    }
    public static string GetBibleVersesFilePath()
    {
        return bibleVersesFilePath;
    }
    public static string GetBibleVersesTranslation()
    {
        return bibleVersesTranslation;
    }
    public static string GetBibleVersesGenre()
    {
        return bibleVersesGenre;
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
    public static void SetAutoStartBibleVersesLoop(Boolean newValue)
    {
        autoStartBibleVersesLoop = newValue;
        SaveSettings();
    }
    public static void SetBibleVersesLoopInterval(int newInterval)
    {
        bibleVersesLoopInterval = newInterval;
    }
    public static void SetBibleVersesFilePath(string newFilePath)
    {
        bibleVersesFilePath = newFilePath;
    }
    public static void SetBibleVersesTranslation(string newTranslation)
    {
        bibleVersesTranslation = newTranslation;
    }
    public static void SetBibleVersesGenre(string newGenre)
    {
        bibleVersesGenre = newGenre;
    }
}
