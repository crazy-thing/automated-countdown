using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;



// TO DO
// HANDLE MULTIPLE COUNTDOWNS BETTER ( LET WRITE TO DIFFERENT FILES BASED ON USERS CHOICE )
// ADD SETTINGS TO SET VARIABLES AND PICK PREFERENCES

class Countdown
{   

    private static Dictionary<string, CancellationTokenSource> countdowns = new Dictionary<string, CancellationTokenSource>();
    private static Dictionary<string, string> countdownNamesToIds = new Dictionary<string, string>();
    private static object lockObject = new object();
    private static string countdownText = "Time remaining";
    private static string countdownOverText = "Live Stream Will Begin Shortly";
    private static string filePath = "./countdown_log.txt";



    static void Main(string[] args)
    {

        while (true)
        {
            Console.WriteLine("Enter a command (start, stop, show, exit): ");
            string userInput = Console.ReadLine();

            string[] inputParts = userInput.Split(' ');

            string command = inputParts[0].ToLower();

            switch (command)
            {
                case "start-countdown":
                    StartCountdown(inputParts);
                    break;
                case "stop":
                    StopCountdown(inputParts[1]);
                    break;
                case "show":
                    ShowAllCountdowns();
                    break;
                case "exit":
                    return;
                default:
                    Console.WriteLine("Invalid command. Please enter start, stop, show, exit");
                    break;
            }
        }

    }

    static void StartCountdown(string[] inputParts)
    {
        
        string dateStr = inputParts[1];
        string timeStr = inputParts[2];



        if (!DateTime.TryParseExact(dateStr + " " + timeStr, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime selectedDateTime))
            {
                Console.WriteLine("Error on line 62Invalid date and time format. Please use the format yyyy:MM-dd HH:mm:ss ");
                return;
            }

            string countdownName = $"countdown{countdownNamesToIds.Count + 1}";
            string countdownId = Guid.NewGuid().ToString();

            countdownNamesToIds.Add(countdownName, countdownId);
    
            CancellationTokenSource cts = new CancellationTokenSource();
            Task.Run(() => StartCountdownInternal(selectedDateTime, cts.Token, countdownId));

            lock (lockObject)
            {
                countdowns.Add(countdownName, cts);
            }

            Console.WriteLine($"Countdown started with name: {countdownName}");
    }

    static void StartCountdownInternal(DateTime targetDateTime, CancellationToken cancellationToken, string countdownName)
    {
        StreamWriter writer = new StreamWriter(filePath);


        while (!cancellationToken.IsCancellationRequested && DateTime.Now < targetDateTime)
        {
            TimeSpan timeRemaining = targetDateTime - DateTime.Now;
            
            string formattedTime = timeRemaining.ToString("mm\\:ss");

            writer.BaseStream.Seek(0, SeekOrigin.Begin);
            writer.Write($"{countdownText}: {formattedTime}");
            writer.Flush();
            Thread.Sleep(1000);
        }

        if (!cancellationToken.IsCancellationRequested)
        {
            writer.BaseStream.Seek(0, SeekOrigin.Begin);
            writer.Write($"\r{countdownOverText}");
            writer.Flush();
            Console.WriteLine($"Countdown {countdownName} finished!");
            StopCountdown(countdownName);
            writer.Close();
        }
        else
        {   
            writer.BaseStream.Seek(0, SeekOrigin.Begin);
            writer.Write($"\r{countdownOverText}");
            writer.Flush();
            Console.WriteLine($"Countdown {countdownName} canceled");
            writer.Close();
        }
    }

    
    static void StopCountdown(string countdownName)
    {
        lock (lockObject)
        {
            if (countdowns.TryGetValue(countdownName, out CancellationTokenSource cts))
            {
                cts.Cancel();
                countdowns.Remove(countdownName);
                countdownNamesToIds.Remove(countdownName);
                Console.WriteLine($"Countdown {countdownName} stopped.");
            }
            else
            {
                Console.WriteLine($"Countdown {countdownName} not found.");
            }            
        }
    }

    static void ShowAllCountdowns()
    {
        lock (lockObject)
        {
            foreach (var kvp in countdowns)
            {
                Console.WriteLine($"Countdown {kvp.Key}: {(kvp.Value.IsCancellationRequested ? "Canceled" : "Running")} ");
            }
        }
    }


}