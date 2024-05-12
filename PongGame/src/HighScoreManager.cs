namespace PongGame
{
    using System;
    using System.IO;

    public class HighScoreManager
    {
        private string highScoreFile = Path.Combine("Highscore", "highscore.txt");
        private int highScore;

        public HighScoreManager()
        {
            highScore = ReadHighScore();
        }

        public int ReadHighScore()
        {
            if (!File.Exists(highScoreFile))
            {
                return 0;
            }

            string scoreText = File.ReadAllText(highScoreFile);
            if (int.TryParse(scoreText, out int score))
            {
                return score;
            }
            else
            {
                return 0;
            }
        }

        private void WriteHighScore(int score)
        {
            string dir = Path.GetDirectoryName(highScoreFile);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.WriteAllText(highScoreFile, score.ToString());
        }

        public void CheckScore(int score)
        {
            if (score > highScore)
            {
                highScore = score;
                WriteHighScore(highScore);
            }
        }

        public void SaveHighScore()
        {
            WriteHighScore(highScore);
        }
    }
}