using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Collections.Generic;
using System.Globalization;

class Countdown
{   

    private static string paddedCountdownOverText;




    public static void StartCountdown(DateTime selectedDateTime)
    { 

            Console.WriteLine("Started with time of: " + selectedDateTime);
            string countdownName = $"countdown{Program.nameToIds.Count + 1}";
            string countdownId = Guid.NewGuid().ToString();

            Program.nameToIds.Add(countdownName, countdownId);
    
            CancellationTokenSource cts = new CancellationTokenSource();
            Task.Run(() => StartCountdownInternal(selectedDateTime, cts.Token, countdownName));

            lock (Program.lockObject)
            {
                Program.tasks.Add(countdownName, cts);
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
            Program.StopTask(countdownName);
            writer.Dispose();
        }
        else
        {   
            writer.BaseStream.Seek(0, SeekOrigin.Begin);
            writer.Write($"\r{paddedCountdownOverText}");
            writer.Flush();
            Console.WriteLine($"Countdown {countdownName} canceled");
            writer.Dispose();
        }
    }





}

