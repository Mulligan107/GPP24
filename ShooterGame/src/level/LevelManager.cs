using System;
using System.Collections.Generic;
using ShooterGame.level.levels;

namespace ShooterGame.level
{
    public class LevelManager
    {
        public static int CurrentLevel { get; set; } = 0;
        private static List<Level> _levels = new List<Level>();


        public static void LoadLevels()
        {
            _levels.Add(new Level1());
            // levels.Add(new Level2());
            // levels.Add(new Level3());
            // ...
        }

        public static Level GetCurrentLevel()
        {
            if (CurrentLevel >= 0 && CurrentLevel < _levels.Count)
            {
                return _levels[CurrentLevel];
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid level number");
            }
        }
    }
}