using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class LevelProgressionFileHandler
{
    private readonly string _filePath;

    public LevelProgressionFileHandler(string filePath)
    {
        _filePath = filePath;
    }

    public Dictionary<string, bool> LoadLevelCompletionStatus()
    {
        if (!File.Exists(_filePath))
        {
            return new Dictionary<string, bool>
            {
                { "Level1", true }, // Level 1 is always unlocked
                { "Level2", false },
                { "Level3", false }
            };
        }

        var json = File.ReadAllText(_filePath);
        return JsonConvert.DeserializeObject<Dictionary<string, bool>>(json);
    }

    public void SaveLevelCompletionStatus(Dictionary<string, bool> levelCompletionStatus)
    {
        var json = JsonConvert.SerializeObject(levelCompletionStatus, Formatting.Indented);
        File.WriteAllText(_filePath, json);
    }
}