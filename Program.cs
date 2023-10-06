using System;
using Microsoft.VisualBasic;

// ######### TO DO ############
/*
    Finish error handling in Util.cs in the handle settings config
    Add variables to easily allow editing of the bible verses template css
    Add option to link countdown timer and bible verses interval
    Option to display days until countdowns
    Make auto start daily work instead of just days
    Organize settings
    Add a notepad file that can be opened with edit command to edit the html template for bible-verses
*/
/* 

*/


class Program
{   
    public static Dictionary<string, CancellationTokenSource> tasks = new Dictionary<string, CancellationTokenSource>();
    public static Dictionary<string, string> nameToIds = new Dictionary<string, string>();
    public static string helpCmdInfo = @"
    Start command used to start a specific task. Countdown option requires a date and time
    start 
        ""countdown"" (""yyyy-mm-dd"") (""hh:mm:ss"") 
        ""bible-verses"" 
        ""countdown-verses""

    Stop command used to stop a specific task. Requires the name of the task to stop
    stop (""task-name"")

    Show command used to show all running tasks. If no tasks are running there will be no output.
    show

    Set command used to configure a setting. Each setting option requires arguments
    set
        Sets the text to be displayed with the time
        ""countdown-text"" (""enter text here"")

        Sets the text to be displayed when the time is over
        ""countdown-over-text"" (""enter text here"")

        Sets the format for the time to be displayed in ""hh:mm:ss"" ""mm:ss""
        ""countdown-format"" (""enter format here"") 
            *Allowed Formats: 
                ""hh:mm:ss"",
                ""hh:mm"", 
                ""hh:ss"", 
                ""hh"",
                ""mm:ss"",
                ""mm"",
                ""ss""* 

        Sets the file path for where to write the countdown time to
        ""file-path"" (""C:\Enter\File Path\here\file.txt"")

        Sets day and time for the countdown to automatically count to
        ""auto-start-time"" (""full day of week, e.g: monday, tuesday, etc"") (""hh:mm:ss"")

        Sets how often for new bible verses to be displayed
        bible-verses-interval (""number in seconds e.g: 10 "")

        Sets the file path for where to write bible verses to
        bible-verses-file-path (""C:\Enter\Bilbe Verses\File\path\here.txt"")

        Sets the translation for the bible verses
        bible-verses-translation
            Available Options:
                ""ASV""
                ""BBE""
                ""DARBY""
                ""KJV""
                ""WEB""
                ""YLT""
                ""ESV""
                ""NIV""
                ""NLT""

        Sets the genre to get bible verses from
        bible-verses-genre
            ""All""
            ""Law""
            ""History""
            ""Wisdom""
            ""Prophets""
            ""Gospels""
            ""Acts""
            ""Epistles""
            ""Apocalyptic""

    Enable-auto-start command used to configure a setting to auto start a task on program startup. ""countdown"" for countdown timer and ""bible-verses"" for bible verses.
    enable-auto-start
        ""countdown""
        ""bible-verses""

    Disable-auto-start command used to configure a setting to not start a task on program startup. ""countdown"" for countdown timer and ""bible-verses"" for bible verses.
    disable-auto-start
        ""countdown""
        ""bible-verses""

    Exit command used to close close program
    exit

    ";
    public static string useHelp = "Use ('help') to see all commands";

    public static object lockObject = new object();
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
        if (settings.AutoStartBibleVersesLoop)
        {
            Console.WriteLine("Auto Start Bible Verses is enabled");
            BibleVersesWriter.StartBibleVerses();
        }
        else
        {
            Console.WriteLine("Auto Start Bible verses is disabled");
        }


        while (true)
        {
            Console.WriteLine($"Enter a command. {useHelp} ");
            string userInput = Console.ReadLine();

            string[] inputParts = userInput.Split(' ');

            string command = inputParts[0].ToLower();

            DateTime selectedDateTime = DateTime.Now;


            switch (command)
            {
                case "help":
                    Console.WriteLine(helpCmdInfo);
                    break;
                case "start":
                    if (inputParts.Length > 1)
                    {


                        switch (inputParts[1])
                        {

                            case "countdown":
                                if (inputParts.Length > 2)
                                {
                                    string dateStr = inputParts[2];
                                    string timeStr = inputParts[3];

                                    if (!DateTime.TryParseExact(dateStr + " " + timeStr, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out selectedDateTime))
                                    {
                                        Console.WriteLine("Error: Invalid date and time format. Please use the format yyyy:MM-dd HH:mm:ss ");
                                        return;
                                    }
                                }
                                string countdownName = null;
                                string filePath = null;
                                if (inputParts.Length > 4)
                                {

                                    switch (inputParts[4])
                                    {
                                        case "name":
                                            countdownName = inputParts[5];
                                            break;
                                        case "file-path":
                                            filePath = inputParts[5];
                                            break;
                                        default:
                                            Console.WriteLine("No options provided. Using default settings");
                                            break;
                                    }
                                    if (inputParts.Length > 6)
                                    {
                                        switch (inputParts[6])
                                        {
                                            case "name":
                                                countdownName = inputParts[7];
                                                break;
                                            case "file-path":
                                                filePath = inputParts[7];
                                                break;
                                            default:
                                                Console.WriteLine("No second option. Using default settings");
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("No second option");
                                    }
                                }
                                Countdown.StartCountdown(selectedDateTime, countdownName, filePath);
                                break;
                            case "bible-verses":
                                string versesName = null;
                                string versesFilePath = null;
                                if (inputParts.Length > 2)
                                {

                                    switch (inputParts[2])
                                    {
                                        case "name":
                                            versesName = inputParts[3];
                                            break;
                                        case "file-path":
                                            versesFilePath = inputParts[3];
                                            break;
                                        default:
                                            Console.WriteLine("No options provided. Using default settings");
                                            break;
                                    }
                                    if (inputParts.Length > 4)
                                    {
                                        switch (inputParts[4])
                                        {
                                            case "name":
                                                versesName = inputParts[5];
                                                break;
                                            case "file-path":
                                                versesFilePath = inputParts[5];
                                                break;
                                            default:
                                                Console.WriteLine("No second option. Using default settings");
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("No second option");
                                    }
                                }
                                BibleVersesWriter.StartBibleVerses(versesName, versesFilePath);
                                break;
                            case "countdown-verses":
                                Countdown.StartCountdown(selectedDateTime);
                                BibleVersesWriter.StartBibleVerses();
                                break;
                            default:
                                Console.WriteLine($"Invalid option. {useHelp}");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"No option provided. {useHelp}");
                    }

                    break;
                case "stop":
                    if (inputParts.Length > 1)
                    {
                        StopTask(inputParts[1]);
                    }
                    else
                    {
                        Console.WriteLine($"No option provided. {useHelp}");
                    }
                    break;
                case "show":
                    ShowAllTasks();
                    break;
                case "set":
                    if (inputParts.Length > 1)
                    {
                        Util.SetSettings(inputParts);
                    }
                    else
                    {
                        Console.WriteLine($"No option provided. {useHelp}");
                    }
                    break;
                case "enable-auto-start":
                    if (inputParts.Length > 1)
                    {
                        switch (inputParts[1])
                        {
                            case "countdown":
                                SettingsManager.SetAutoStartCountdown(true);
                                break;
                            case "bible-verses":
                                SettingsManager.SetAutoStartBibleVersesLoop(true);
                                break;
                            default:
                                Console.WriteLine($"Invalid command. {useHelp}");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"No option provided. {useHelp}");
                    }
                    break;
                case "disable-auto-start":
                    if (inputParts.Length > 1)
                    {
                        switch (inputParts[1])
                        {
                            case "countdown":
                                SettingsManager.SetAutoStartCountdown(false);
                                break;
                            case "bible-verses":
                                SettingsManager.SetAutoStartBibleVersesLoop(false);
                                break;
                            default:
                                Console.WriteLine($"Invalid command. {useHelp}");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"No option provided. {useHelp}");
                    }
                    break;
                case "exit":
                    return;
                default:
                    Console.WriteLine($"Invalid command. {useHelp}");
                    break;
            }


        }
    }
    public static void ShowAllTasks()
    {
        lock (lockObject)
        {
            foreach (var kvp in Program.tasks)
            {
                Console.WriteLine($"Task {kvp.Key}: {(kvp.Value.IsCancellationRequested ? "Canceled" : "Running")} ");
            }
        }
    }

    public static void StopTask(string taskName)
    {
        lock (lockObject)
        {
            if (tasks.TryGetValue(taskName, out CancellationTokenSource cts))
            {
                cts.Cancel();
                tasks.Remove(taskName);
                nameToIds.Remove(taskName);
                Console.WriteLine($"Task {taskName} stopped.");
            }
            else
            {
                Console.WriteLine($"Task {taskName} not found.");
            }            
        }
    }

}