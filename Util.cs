class Util
{   
    private static string[] autoStartOptsDays = {"daily", "monday","tuesday","wednesday","thursday","friday","saturday","sunday"};
    private static string[] allowedFormats = {"hh:mm:ss", "hh:mm", "hh:ss", "hh","mm:ss","mm","ss"};
    private static string[] allowedTranslations = {"ASV", "BBE", "DARBY", "KJV", "WEB", "YLT", "ESV", "NIV", "NLT"};
    public static string[] allowedGenres = {"All", "Law", "History", "Wisdom", "Prophets","Gospels", "Acts", "Epistles", "Apocalyptic"};
        public static void SetSettings(string[] inputParts)
    {
        try
        {
            
            string setting = inputParts[1];
            string newSetting = string.Join(" ", inputParts.Skip(2));

            if (inputParts.Length > 1)
            {
                switch (setting)
                {
                    case "countdown-text":
                        SettingsManager.SetCountdownText(newSetting);
                        Console.WriteLine("set the countdown text message: " + SettingsManager.GetCountdownText());
                        break;
                    case "countdown-over-text":
                        SettingsManager.SetCountdownOverText(newSetting);
                        Console.WriteLine("set the text for when the countdown is over: " + SettingsManager.GetCountdownOverText());
                        break;
                    case "countdown-format":

                        if (allowedFormats.Contains(newSetting))
                        {
                            string newFormat = newSetting.Replace(":", "\\:");
                            SettingsManager.SetCountdownFormat(newFormat);
                        }
                        else
                        {
                            Console.WriteLine($"Invalid format. Please enter a valid display format. {allowedFormats}");
                        }
                        break;
                    case "file-path":
                        SettingsManager.SetFilePath(newSetting);
                        Console.WriteLine("set the filepath: " + SettingsManager.GetFilePath());
                        break;
                    case "auto-start-time":
                    if (inputParts.Length > 3)
                    {
                        string dayOpt = inputParts[2];
                        string targetTime = inputParts[3];
                        int index = Array.IndexOf(autoStartOptsDays, dayOpt);
                        DayOfWeek targetDayOfWeek = DateTime.Now.DayOfWeek;
                        if (index != -1)
                        {
                            if (index == 0)
                            {
                                targetDayOfWeek = DateTime.Now.DayOfWeek;
                            }
                            else
                            {
                                targetDayOfWeek = (DayOfWeek)(index);
                            }
                            Console.WriteLine($"Found {dayOpt} at index {index}");
                        }
                        else
                        {
                            Console.WriteLine($"Invalid option '{dayOpt}' - Please use daily, monday, tuesday, wednesday, thursday, friday, saturday, sunday ");
                        }

                        SettingsManager.SetAutoCountdownDay(targetDayOfWeek);
                        SettingsManager.SetAutoCountdownTime(targetTime);
                        
                        DateTime selectedAutoStartDateTime = CalculateAutoStartDateTime(targetDayOfWeek, targetTime);
                    }
                    else
                    {
                        Console.WriteLine("Missing required options. Please enter a day of the week and time e.g. monday 16:00:00");
                    }
                        break;
                    case "bible-verses-interval":
                        if (!string.IsNullOrWhiteSpace(newSetting))
                        {
                            int newIntervalSeconds = int.Parse(newSetting);
                            int newIntervalMs = newIntervalSeconds * 1000;
                            SettingsManager.SetBibleVersesLoopInterval(newIntervalMs);
                        }
                        else
                        {
                            Console.WriteLine("Missing option. Please enter a number in seconds e.g. 10 ");
                        }
                        break;
                    case "bible-verses-file-path":
                        SettingsManager.SetBibleVersesFilePath(newSetting);
                        break;
                    case "bible-verses-translation":
                        if(allowedTranslations.Contains(newSetting))
                        {
                            SettingsManager.SetBibleVersesTranslation(newSetting);
                        }
                        else
                        {
                            Console.WriteLine("Invalid translation. Please choose from {ASV, BBE, DARBY, KJV, WEB, YLT, ESV, NIV, NLT}");
                        }

                        break;
                    case "bible-verses-genre":
                        if (allowedGenres.Contains(newSetting))
                        {
                            SettingsManager.SetBibleVersesGenre(newSetting);
                        }
                        else
                        {
                            Console.WriteLine("Invalid genre. Please choose from {All, Law, History, Prophets, Gospels, Acts, Epistles, Apocalyptic}.");
                        }
                        break;
                    default:
                        Console.WriteLine($"Invalid option. {Program.useHelp}");
                        break;
                }
                SettingsManager.SaveSettings();
            }
            else
            {
                Console.WriteLine($"No option provided. {Program.useHelp}");
            }
        }
        catch (System.Exception)
        {
            Console.WriteLine("An error occurred while processing the command.");
            throw;
        }

    }

    public static DateTime CalculateAutoStartDateTime(DayOfWeek targetDayOfWeek, string targetTime)
{
    DateTime dateTime = CalculateNextDayOfWeek(targetDayOfWeek);
    string dateStr = dateTime.ToString("yyyy-MM-dd");

    Console.WriteLine("Date: " + dateStr + "time: " + targetTime);
    if (!DateTime.TryParseExact(dateStr + " " + targetTime, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime selectedAutoStartDateTime))
        {
            Console.WriteLine("Error: Invalid date and time format. Please use the format yyyy:MM-dd HH:mm:ss ");
            return selectedAutoStartDateTime;
        }
    SettingsManager.SetAutoCountdownDateTime(selectedAutoStartDateTime);
    return selectedAutoStartDateTime;
}

    public static DateTime CalculateNextDayOfWeek(DayOfWeek targetDayOfWeek)
    {
            DateTime currentDate = DateTime.Now;
            int daysUntilTargetDay = ((int)targetDayOfWeek - (int)currentDate.DayOfWeek + 7) % 7;
            DateTime nextDay = currentDate.Date.AddDays(daysUntilTargetDay);
            return nextDay;
    }

}

// extra 
/*                 case "bible-verses-file-path":
                    SettingsManager.SetBibleVersesFilePath(newSetting);
                    break;
                case "bible-verses-info-file-path":
                    SettingsManager.SetBibleVersesInfoFilePath(newSetting);
                    break; 
                    
                    */