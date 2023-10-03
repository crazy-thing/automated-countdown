using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Collections.Generic;
using System.Globalization;

class Countdown
{   

    private static Dictionary<string, CancellationTokenSource> countdowns = new Dictionary<string, CancellationTokenSource>();
    private static Dictionary<string, string> countdownNamesToIds = new Dictionary<string, string>();
    private static object lockObject = new object();
    private static string paddedCountdownOverText;




    public static void StartCountdown(DateTime selectedDateTime)
    { 

            Console.WriteLine("Started with time of: " + selectedDateTime);
            string countdownName = $"countdown{countdownNamesToIds.Count + 1}";
            string countdownId = Guid.NewGuid().ToString();

            countdownNamesToIds.Add(countdownName, countdownId);
    
            CancellationTokenSource cts = new CancellationTokenSource();
            Task.Run(() => StartCountdownInternal(selectedDateTime, cts.Token, countdownName));

            lock (lockObject)
            {
                countdowns.Add(countdownName, cts);
            }

            Console.WriteLine($"Countdown started with name: {countdownName}");
    }

    public static void StartCountdownInternal(DateTime targetDateTime, CancellationToken cancellationToken, string countdownName)
    {

        StreamWriter writer = new StreamWriter(SettingsManager.GetFilePath());

        string prevCountdownText = string.Empty;
        string prevCountDownOverText = string.Empty;

        while (!cancellationToken.IsCancellationRequested && DateTime.Now < targetDateTime)
        {
            TimeSpan timeRemaining = targetDateTime - DateTime.Now;
            string formattedTime = timeRemaining.ToString(SettingsManager.GetCountdownFormat()); 
            string paddedCountdownText = $"{SettingsManager.GetCountdownText()}: {formattedTime}".PadRight(prevCountdownText.Length);
            paddedCountdownOverText = $"{SettingsManager.GetCountdownOverText()}".PadRight(prevCountDownOverText.Length);


            writer.BaseStream.Seek(0, SeekOrigin.Begin);
            writer.Write(paddedCountdownText);
            writer.Flush();

            prevCountdownText = paddedCountdownText;
            prevCountDownOverText = paddedCountdownOverText;

            Thread.Sleep(1000);
        }

        if (!cancellationToken.IsCancellationRequested)
        {
            writer.BaseStream.Seek(0, SeekOrigin.Begin);
            writer.Write($"\r{paddedCountdownOverText}");
            writer.Flush();
            Console.WriteLine($"Countdown {countdownName} finished!");
            StopCountdown(countdownName);
            writer.Close();
        }
        else
        {   
            writer.BaseStream.Seek(0, SeekOrigin.Begin);
            writer.Write($"\r{paddedCountdownOverText}");
            writer.Flush();
            Console.WriteLine($"Countdown {countdownName} canceled");
            writer.Close();
        }
    }

    public static void StopCountdown(string countdownName)
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

    public static void ShowAllCountdowns()
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

