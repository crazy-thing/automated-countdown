using System;

// ######### TO DO ############
/*
    Option to choose countdown name
    Option to display days until countdowns
    Make auto start daily work instead of just days
    Add Help command | write docs
*/
/* 

*/


class Program
{
    static void Main(string[] args)
    {
        SettingsManager.LoadSettings();

        var settings = SettingsManager.GetCountdownSettings();

        if (settings.AutoStartCountdown)
        {
            Console.WriteLine("Auto Start Countdown is enabled");
            DateTime startTime = Util.CalculateAutoStartDateTime(settings.AutoCountdownDay, settings.AutoCountdownTime);

            Countdown.StartCountdown(startTime);
        } 
        else
        {
            Console.WriteLine("Auto Start Countdown is disabled");
        }


        while (true)
        {
            Console.WriteLine("Enter a command (start-countdown, stop, show, set, enable-auto-start, disable-auto-start, exit): ");
            string userInput = Console.ReadLine();

            string[] inputParts = userInput.Split(' ');

            string command = inputParts[0].ToLower();



            switch (command)
            {
                case "start-countdown":
                    string dateStr = inputParts[1];
                    string timeStr = inputParts[2];

                    if (!DateTime.TryParseExact(dateStr + " " + timeStr, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime selectedDateTime))
                        {
                            Console.WriteLine("Error: Invalid date and time format. Please use the format yyyy:MM-dd HH:mm:ss ");
                            return;
                        }

                    Countdown.StartCountdown(selectedDateTime);
                    break;
                case "stop":
                    Countdown.StopCountdown(inputParts[1]);
                    break;
                case "show":
                    Countdown.ShowAllCountdowns();
                    break;
                case "set":
                    Util.SetSettings(inputParts);
                    break;
                case "enable-auto-start":
                    SettingsManager.SetAutoStartCountdown(true);
                    break;
                case "disable-auto-start":
                    SettingsManager.SetAutoStartCountdown(false);
                    break;
                case "exit":
                    return;
                default:
                    Console.WriteLine("Invalid command. Please enter (start-countdown, stop, show, set, enable-auto-start, disable-auto-start, exit)");
                    break;
            }


        }
    }    
}