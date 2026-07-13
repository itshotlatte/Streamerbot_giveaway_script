using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class CPHInline
{
    private static readonly object fileLock = new object();
    private readonly string filePath = @"C:\Users\giveaway_entries.json";  //Directory

    public bool Execute()
    {

    if (!args.TryGetValue("input0", out object inputObj) || //! is NOT, out is output, and || is the OR operator in C#
            !int.TryParse(inputObj.ToString(), out int requestedWinners) || //Checks if string can be converted into integer
            requestedWinners <= 0) //Checks if integer is less than or equal to 0
        {
            CPH.SendMessage("How many winners mister strimer? (e.g., !end 3)");  //Can add custom text here
            return true; // Safely exit the script
        }

        lock (fileLock)
        {
            if (!File.Exists(filePath))  //Checks if the file exists in the directory
            {
                CPH.SendMessage("JSON file not found! Check the files location NotLikeThis ");  //Can add custom text here
                return true; //exits code
            }

            string json = File.ReadAllText(filePath); //Reads the json
            List<string> entries = JsonConvert.DeserializeObject<List<string>>(json) ?? new List<string>(); //converts into a list

            if (entries.Count == 0)  //Checks if list is empty
            {
                CPH.SendMessage("No entries. DID YOU MESS SOMETHING Up? NotLikeThis "); //Can add custom text here
                return true;
            } 

            Random rnd = new Random(); //RNG
            int winnerCount = Math.Min(requestedWinners, entries.Count); //requestedWinners is what the user typed with !end <number> to end the giveaway
            var winners = entries.OrderBy(x => rnd.Next()).Take(winnerCount).ToList(); //Creates a new variable called winners and picks names using the rnd variable AND adds them to list (phew)

            string winnersList = string.Join(", ", winners); //Creates readable string to send to chat
            CPH.SendMessage($"The giveaway has ended! Congratulations to our {winnerCount} winner(s): @{winnersList}");

            // File.Delete(filePath); maybe I want to incorporate a delete function?
        }

        return true;
    }
}
