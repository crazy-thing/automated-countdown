using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Collections.Generic;
using System.Globalization;

class Countdown
{   
    private static string paddedCountdownOverText;
    public static void StartCountdown(DateTime selectedDateTime, string countdownName = null, string filePath = null)
    { 
            Console.WriteLine("Started with time of: " + selectedDateTime);
            
            if (string.IsNullOrWhiteSpace(countdownName))
            {
                countdownName = $"countdown{Program.nameToIds.Count + 1}";
            }

            string countdownId = Guid.NewGuid().ToString();
            Program.nameToIds.Add(countdownName, countdownId);

            if(string.IsNullOrWhiteSpace(filePath))
            {
                filePath = SettingsManager.GetFilePath();
            }
    
            CancellationTokenSource cts = new CancellationTokenSource();
            Task.Run(() => StartCountdownInternal(selectedDateTime, cts.Token, countdownName, filePath));
            TaskInfo taskInfo = new TaskInfo
            {
                TaskType = "Countdown",
                CancellationTokenSource = cts,
            };
            lock (Program.lockObject)
            {
                Program.tasks.Add(countdownName, taskInfo);
            }

            Console.WriteLine($"Countdown started with name: {countdownName}");
    }

    public static void StartCountdownInternal(DateTime targetDateTime, CancellationToken cancellationToken, string countdownName, string filePath)
    {
        StreamWriter writer = new StreamWriter(filePath);

        string prevCountdownText = string.Empty;
        string prevCountDownOverText = string.Empty;

        while (!cancellationToken.IsCancellationRequested && DateTime.Now < targetDateTime)
        {
            TimeSpan timeRemaining = targetDateTime - DateTime.Now;
            string format = SettingsManager.GetCountdownFormat();
            string formattedTime = timeRemaining.ToString(format); 
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
            Util.StopTask(countdownName);
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

