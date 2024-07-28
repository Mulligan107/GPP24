using System.Collections.Generic;

public class LevelProgressionSystem
{
    private readonly Dictionary<string, bool> _levelCompletionStatus;

    public LevelProgressionSystem()
    {
        _levelCompletionStatus = new Dictionary<string, bool>
        {
            { "Level1", true }, // Level 1 is always unlocked
            { "Level2", false },
            { "Level3", false }
        };
    }

    public void MarkLevelAsCompleted(string levelName)
    {
        if (_levelCompletionStatus.ContainsKey(levelName))
        {
            _levelCompletionStatus[levelName] = true;
        }
    }

    public bool IsLevelUnlocked(string levelName)
    {
        return _levelCompletionStatus.ContainsKey(levelName) && _levelCompletionStatus[levelName];
    }
}