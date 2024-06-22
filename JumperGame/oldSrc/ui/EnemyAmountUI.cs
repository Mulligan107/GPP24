using System;
using SDL2;

namespace ShooterGame.ui
{
    public class EnemyAmountUI
    {
        public static void DisplayEnemyCount(IntPtr renderer)
        {
            var enemyCountText = "Enemies left: " + Enemy.TotalEnemies;
            var textWidth = 200;
            var fonttext = "lazy.ttf";
            var enemyCountColor = new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 }; // White color

            IntPtr font = SDL_ttf.TTF_OpenFont(fonttext, 60);

            // Display enemy count
            IntPtr surfaceMessage = SDL_ttf.TTF_RenderText_Solid(font, enemyCountText, enemyCountColor);
            IntPtr texture = SDL.SDL_CreateTextureFromSurface(renderer, surfaceMessage);

            var position = new Vector2D { X = Program.SCREEN_WIDTH - textWidth - 10, Y = 10 }; // Adjust position as needed

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
            SDL_ttf.TTF_CloseFont(font);
        }
    }
}