using System;

class Program
{
    static void Main(string[] args)
    {
        SettingsManager.LoadSettings();

        while (true)
        {
            Console.WriteLine("Enter a command (start, stop, show, set, exit): ");
            string userInput = Console.ReadLine();

            string[] inputParts = userInput.Split(' ');

            string command = inputParts[0].ToLower();



            switch (command)
            {
                case "start-countdown":
                    Countdown.StartCountdown(inputParts);
                    break;
                case "stop":
                    Countdown.StopCountdown(inputParts[1]);
                    break;
                case "show":
                    Countdown.ShowAllCountdowns();
                    break;
                case "set":
                    Countdown.SetSettings(inputParts);
                    break;
                case "exit":
                    return;
                default:
                    Console.WriteLine("Invalid command. Please enter start, stop, show, set, exit");
                    break;
            }
        }
    }    
}