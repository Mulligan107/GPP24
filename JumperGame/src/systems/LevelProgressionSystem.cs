using System.Collections.Generic;
using System.Linq;

public class LevelProgressionSystem
{
    private Dictionary<string, bool> _levelCompletionStatus;
    private LevelProgressionFileHandler _fileHandler;

    public LevelProgressionSystem(string filePath)
    {
        _fileHandler = new LevelProgressionFileHandler(filePath);
        _levelCompletionStatus = _fileHandler.LoadLevelCompletionStatus();
    }

    public void MarkLevelAsCompleted(string levelName)
    {
        if (_levelCompletionStatus.ContainsKey(levelName))
        {
            _levelCompletionStatus[levelName] = true;
            _fileHandler.SaveLevelCompletionStatus(_levelCompletionStatus);
        }
    }

    public bool IsLevelUnlocked(string levelName)
    {
        return _levelCompletionStatus.ContainsKey(levelName) && _levelCompletionStatus[levelName];
    }

    public void UnlockAllLevels()
    {
        foreach (var key in _levelCompletionStatus.Keys.ToList())
        {
            _levelCompletionStatus[key] = true;
        }
        _fileHandler.SaveLevelCompletionStatus(_levelCompletionStatus);
    }
}