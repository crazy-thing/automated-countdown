class Util
{   
    private static string[] autoStartOptsDays = {"daily", "monday","tuesday","wednesday","thursday","friday","saturday","sunday"};


        public static void SetSettings(string[] inputParts)
    {
        string setting = inputParts[1];
        string newSetting = string.Join(" ", inputParts.Skip(2));
        Console.WriteLine("selected setting:" + setting);

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
            case "file-path":
                SettingsManager.SetFilePath(newSetting);
                Console.WriteLine("set the filepath: " + SettingsManager.GetFilePath());
                break;
            case "auto-start-config":

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
                        // Map the index to the corresponding DayOfWeek enum value
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

                break;
            default:
                Console.WriteLine("Invalid parameter. Please enter countdown-text, countdown-over-text, file-path");
                break;
        }

        SettingsManager.SaveSettings();
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