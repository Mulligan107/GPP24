using System.Collections.Generic;

public class LevelProgressionSystem
{
    private readonly Dictionary<string, bool> _levelCompletionStatus;
    private readonly LevelProgressionFileHandler _fileHandler;

    public LevelProgressionSystem()
    {
        _fileHandler = new LevelProgressionFileHandler("levelProgression.json");
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
}