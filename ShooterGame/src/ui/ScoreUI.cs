using System;
using System.IO;
using SDL2;

namespace ShooterGame.ui
{
    public class ScoreUI
    {
        public static int Score { get; private set; }
        public static int TargetScore { get; private set; }
        public static int PreviousScore { get; private set; } // New variable to store the previous score
        private static string filePath = "highscore.txt"; // File to store the highscore


        public ScoreUI()
        {
            LoadHighscore();
        }

        public static void IncreaseScore(int increment)
        {
            TargetScore += increment; // Increase the target score instead of the current score
            SaveHighscore();
        }

        // New method to gradually increase the score
        public static void Update()
        {
            PreviousScore = Score; // Store the current score as the previous score before updating it

            if (Score < TargetScore)
            {
                Score++;
            }
            else if (Score > TargetScore)
            {
                Score--;
            }
        }

        private static void SaveHighscore()
        {
            try
            {
                if (Score > int.Parse(File.ReadAllText(filePath)))
                {
                    File.WriteAllText(filePath, Score.ToString());
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(filePath, Score.ToString());
                Console.WriteLine("Error saving highscore: " + ex.Message);
            }
        }

        public static void LoadHighscore()
        {
            
            try
            {
                if (File.Exists(filePath))
                {
                    string scoreText = File.ReadAllText(filePath);
                    Score = 0;
                }else
                {
                    File.AppendText(filePath);
                    Score = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading highscore: " + ex.Message);
            }
        }

        public static void DisplayHighscore(IntPtr renderer)
        {
            var scoreText = "Score: " + Score;
            var highscoreText = "Highscore: " + File.ReadAllText(filePath);
            var position = new Vector2D { X = 10, Y = 10 }; // Top left corner
            var textWidth = 200;
            var fonttext = "lazy.ttf";

            // Determine the color of the score text based on whether the score has increased or decreased
            var scoreColor = new SDL.SDL_Color();
            if (Score > PreviousScore)
            {
                scoreColor = new SDL.SDL_Color { r = 0, g = 255, b = 0, a = 255 }; // Green color
            }
            else if (Score < PreviousScore)
            {
                scoreColor = new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255 }; // Red color
            }
            else
            {
                scoreColor = new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 }; // White color
            }

            // The color of the highscore text is always white
            var highscoreColor = new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 }; // White color

            IntPtr font = SDL_ttf.TTF_OpenFont(fonttext, 60);

            // Display current score
            IntPtr surfaceMessage = SDL_ttf.TTF_RenderText_Solid(font, scoreText, scoreColor);
            IntPtr texture = SDL.SDL_CreateTextureFromSurface(renderer, surfaceMessage);

            SDL.SDL_Rect destRect = new SDL.SDL_Rect
            {
                x = (int)position.X,
                y = (int)position.Y,
                w = textWidth,
                h = textWidth / 4
            };

            SDL.SDL_RenderCopy(renderer, texture, IntPtr.Zero, ref destRect);
            SDL.SDL_DestroyTexture(texture);
            SDL.SDL_FreeSurface(surfaceMessage);

            // Display highscore
            surfaceMessage = SDL_ttf.TTF_RenderText_Solid(font, highscoreText, highscoreColor);
            texture = SDL.SDL_CreateTextureFromSurface(renderer, surfaceMessage);

            destRect = new SDL.SDL_Rect
            {
                x = (int)position.X,
                y = (int)position.Y + textWidth / 4 + 10, // Display highscore below the score
                w = textWidth,
                h = textWidth / 4
            };

            SDL.SDL_RenderCopy(renderer, texture, IntPtr.Zero, ref destRect);
            SDL.SDL_DestroyTexture(texture);
            SDL.SDL_FreeSurface(surfaceMessage);
            SDL_ttf.TTF_CloseFont(font);
        }
    }
}