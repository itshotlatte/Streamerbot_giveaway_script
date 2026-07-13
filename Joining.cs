using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class CPHInline
{
    private static readonly object fileLock = new object();
    private readonly string filePath = @"C:\Users\giveaway_entries.json"; //Directory, filepath

    public bool Execute()
    {
        if (!args.TryGetValue("user", out object userObj)) return false; //Checks if variable "user" is given by streamer.bot to the script upon triggering the command, if not, close the script
        string username = userObj.ToString(); //converts the user variable from streamer.bot into a string

        lock (fileLock)
        {
            List<string> entries = new List<string>();

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath); //checks if the file exists in the directory, filepath
                entries = JsonConvert.DeserializeObject<List<string>>(json) ?? new List<string>(); //I did not know of this function, Gemini helped. Its a newtonsoft.json assembly reference. Converts string into list inside json
            } //side note, ?? is a null coalesing operator. Like an if/else statement to prevent the NullReferenceException crash

            if (!entries.Contains(username)) //checks if name is present in list
            {
                entries.Add(username);
                File.WriteAllText(filePath, JsonConvert.SerializeObject(entries, Formatting.Indented));
                CPH.SendMessage($"@{username} You have successfully entered the giveaway!");
            }
            else
            {
                CPH.SendMessage($"@{username} You have already entered the giveaway!");
            }
        }

        return true; //That was easy
