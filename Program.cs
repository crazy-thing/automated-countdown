using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

/*
    ######### Possible future To-Do #########

    Make new command handling method to improve readability and efficiency
    Organize settings
*/

class Program
{   
    public static Dictionary<string, TaskInfo> tasks = new Dictionary<string, TaskInfo>();
    public static Dictionary<string, string> nameToIds = new Dictionary<string, string>();
    public static string helpCmdInfo = @"
    Start command used to start a specific task. Countdown option requires a date and time
    start 
        countdown (yyyy-mm-dd) (hh:mm:ss) **OPTIONAL** name (""enter-name-here"") file-path (""C:\Enter\File\path\here.txt"")
        bible-verses  **OPTIONAL** name (""enter-name-here"") file-path (""C:\Enter\File\path\here.txt"")
        countdown-verses (yyyy-mm-dd) (hh:mm:ss)

    Stop command used to stop a specific task. Requires the name of the task to stop
    stop (""task-name"")
          (""all"")

    Show command used to show all running tasks. If no tasks are running there will be no output.
    show

    Set command used to configure a setting. Each setting option requires arguments
    set
        Sets the text to be displayed with the time
        countdown-text (enter text here)

        Sets the text to be displayed when the time is over
        countdown-over-text (enter text here)

        Sets the format for the time to be displayed in hh:mm:ss mm:ss
        countdown-format (enter format here) 
            dd:hh:mm:ss
            dd:hh:mm
            dd:hh
            dd
            hh:mm:ss
            hh:mm
            hh:ss
            hh
            mm:ss
            mm
            ss

        Sets the file path for where to write the countdown time to
        file-path (C:\Enter\File Path\here\file.txt)

        Sets day and time for the countdown to automatically count to
        auto-start-time (full day of week, e.g: monday, tuesday, etc) (hh:mm:ss)

        Sets how often for new bible verses to be displayed
        bible-verses-interval (number in seconds e.g: 10 )

        Sets the file path for where to write bible verses to
        bible-verses-file-path (C:\Enter\Bilbe Verses\File\path\here.txt)

        Sets the translation for the bible verses
        bible-verses-translation
            ASV
            BBE
            DARBY
            KJV
            WEB
            YLT
            ESV
            NIV
            NLT

        Sets the genre to get bible verses from
        bible-verses-genre
            All
            Law
            History
            Wisdom
            Prophets
            Gospels
            Acts
            Epistles
            Apocalyptic

    Enable-auto-start command used to configure a setting to auto start a task on program startup. ""countdown"" for countdown timer and ""bible-verses"" for bible verses.
    enable-auto-start
        countdown
        bible-verses

    Disable-auto-start command used to configure a setting to not start a task on program startup. ""countdown"" for countdown timer and ""bible-verses"" for bible verses.
    disable-auto-start
        countdown
        bible-verses
    Edit-bible-verses command used to open a txt file to configure settings for the bible verses display. Changes can be made to the font, font-size, and color.
    More changes can be made directly by going to the verses-template.html file.
    edit-bible-verses

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

        Console.WriteLine($"Enter a command. {useHelp} ");
        while (true)
        {
            restartLabel:
            Console.Write("> ");
            string userInput = Console.ReadLine();

            string[] inputParts = Regex.Matches(userInput, @"[\""].+?[\""]|[^ ]+")
                            .Cast<Match>()
                            .Select(m => m.Value)
                            .ToArray();;

            string command = string.Empty;
            if (inputParts.Length > 0)
            {
                command = inputParts[0].ToLower();
            }
            else
            {
                goto restartLabel;
            }

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
                                if (inputParts.Length > 3)
                                {
                                    string dateStr = inputParts[2];
                                    string timeStr = inputParts[3];

                                    if (!DateTime.TryParseExact(dateStr + " " + timeStr, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out selectedDateTime))
                                    {
                                        Console.WriteLine("Missing or invalid options. Please enter a date and time in the format yyyy:mm-dd hh:mm:ss");
                                        break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Missing or invalid options. Please enter a date and time in the format yyyy:mm-dd hh:mm:ss ");
                                    break;
                                }
                                string countdownName = null;
                                string filePath = null;
                                if (inputParts.Length > 4)
                                {
                                    if (inputParts.Length > 5 && inputParts[5].StartsWith('"') && inputParts[5].EndsWith('"'))
                                    {
                                        switch (inputParts[4])
                                        {
                                            case "name":
                                                    if(tasks.ContainsKey(inputParts[5].Trim('"')))
                                                    {
                                                        Console.WriteLine("Named already in use!. Getting default names");
                                                    }
                                                    else
                                                    {
                                                        countdownName = inputParts[5].Trim('"');
                                                    }
                                                break;
                                            case "file-path":
                                                filePath = inputParts[5].Trim('"');
                                                break;
                                            default:
                                                Console.WriteLine("Invalid option. Use name or file-path");
                                                goto countdownBreakLabel;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Missing or empty value given after first option. Make sure to put \" \" around the value e.g. name \"enter name here\" Use 'help' command for more details.");
                                        break;
                                    }
                                    if (inputParts.Length > 6)
                                    {
                                        if (inputParts.Length > 7  && inputParts[7].StartsWith('"') && inputParts[7].EndsWith('"'))
                                        {
                                            switch (inputParts[6])
                                            {
                                                case "name":
                                                   if(tasks.ContainsKey(inputParts[7].Trim('"')))
                                                    {
                                                        Console.WriteLine("Named already in use!. Getting default names");
                                                    }
                                                    else
                                                    {
                                                        countdownName = inputParts[7].Trim('"');
                                                    }                                                 
                                                break;
                                                case "file-path":
                                                    filePath = inputParts[7].Trim('"');
                                                    break;
                                                default:
                                                    Console.WriteLine("Invalid option. Use name or file-path");
                                                    goto countdownBreakLabel;
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Missing or empty value given after second option. Make sure to put \" \" around the value e.g. name \"enter name here\" Use 'help' command for more details.");
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("No second option. Using default settings");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No options provided. Using defaults settings");
                                }
                                Countdown.StartCountdown(selectedDateTime, countdownName, filePath);
                                countdownBreakLabel:
                                break;
                            case "bible-verses":
                                string versesName = null;
                                string versesFilePath = null;
                                if (inputParts.Length > 2)
                                {
                                    if (inputParts.Length > 3 && inputParts[3].StartsWith('"') && inputParts[3].EndsWith('"'))
                                    {
                                        switch (inputParts[2])
                                        {
                                            case "name":
                                                if(tasks.ContainsKey(inputParts[3].Trim('"')))
                                                {
                                                    Console.WriteLine("Named already in use!. Getting default names");
                                                }
                                                else
                                                {
                                                    versesName = inputParts[3].Trim('"');
                                                }                                                
                                                break;
                                            case "file-path":
                                                versesFilePath = inputParts[3].Trim('"');
                                                break;
                                            default:
                                                Console.WriteLine("Invalid option. Use name or file-path");
                                                goto versesBreakLabel;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Missing or empty value given after first option. Make sure to put \" \" around the value e.g. name \"enter name here\" Use 'help' command for more details.");
                                        break;
                                    }
                                    if (inputParts.Length > 4)
                                    {
                                        if (inputParts.Length > 5 && inputParts[5].StartsWith('"') && inputParts[5].EndsWith('"'))
                                        {
                                            switch (inputParts[4])
                                            {
                                                case "name":
                                                   if(tasks.ContainsKey(inputParts[5].Trim('"')))
                                                    {
                                                        Console.WriteLine("Named already in use! Getting default names");
                                                    }
                                                    else
                                                    {
                                                        versesName = inputParts[5].Trim('"');
                                                    }
                                                    break;
                                                case "file-path":
                                                    versesFilePath = inputParts[5].Trim('"');
                                                    break;
                                                default:
                                                    Console.WriteLine("Invalid option. Use name or file-path");
                                                    goto versesBreakLabel;
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Missing or empty value given after second option. Make sure to put \" \" around the value e.g. name \"enter name here\" Use 'help' command for more details.");
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("No second option provided. Using default settings");

                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No options provided. Using default settings");
                                }
                                BibleVersesWriter.StartBibleVerses(versesName, versesFilePath);
                                versesBreakLabel:
                                break;
                            case "countdown-verses":
                                if (inputParts.Length > 3)
                                {
                                    string dateStr = inputParts[2];
                                    string timeStr = inputParts[3];

                                    if (!DateTime.TryParseExact(dateStr + " " + timeStr, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out selectedDateTime))
                                    {
                                        Console.WriteLine("Missing or invalid options. Please enter a date and time in the format yyyy:mm-dd hh:mm:ss");
                                        break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Missing or invalid options. Please enter a date and time in the format yyyy:mm-dd hh:mm:ss ");
                                    break;
                                }
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
                    if (inputParts.Length > 1 && inputParts[1].StartsWith('"') && inputParts[1].EndsWith('"'))
                    {
                        Util.StopTask(inputParts[1].Trim('"'));
                    }
                    else
                    {
                        Console.WriteLine($"Missing or invalid option. Make sure to place option in quotations \" \".");
                    }
                    break;
                case "show":
                    Util.ShowAllTasks();
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
                case "edit-bible-verses":
                    BibleVersesWriter.EditBibleVersesVariables();
                    break;
                case "exit":
                    return;
                default:
                    Console.WriteLine($"Invalid command. {useHelp}");
                    break;
            }
        }
    }


}