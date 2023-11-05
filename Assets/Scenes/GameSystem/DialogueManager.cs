using System;
using System.Collections.Generic;
using System.IO;

public class DialogueManager
{
    private Dictionary<string, string> dialogues = new Dictionary<string, string>();
    private string currentLanguage = "EN";

    public DialogueManager()
    {
        
    }

    public void SwitchLanguage(string languageCode)
    {
        currentLanguage = languageCode;
        LoadAllDialoguesForLanguage();
    }

    private void LoadAllDialoguesForLanguage()
    {
        dialogues.Clear(); // Clear previous dialogues
        LoadDialoguesFromFile($"Dialogues/{currentLanguage}/dialogues1.txt");
        LoadDialoguesFromFile($"Dialogues/{currentLanguage}/dialogues2.txt");
    }

    private void LoadDialoguesFromFile(string filename)
    {
        foreach (var line in File.ReadLines(filename))
        {
            var parts = line.Split('=');
            if (parts.Length == 2)
            {
                var key = parts[0].Trim();
                var value = parts[1].Trim();
                dialogues[key] = value;
            }
        }
    }
}
