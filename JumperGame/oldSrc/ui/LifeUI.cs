using System;
using SDL2;

namespace ShooterGame.ui
{
    public class LifeUI
    {
        public static void DisplayLives(IntPtr renderer)
        {
            var livesText = "Lives: " + Player.lifes;
            var position = new Vector2D { X = 10, Y = Program.SCREEN_HEIGHT - 50 }; // Bottom left corner
            var textWidth = 200;
            var fonttext = "lazy.ttf";
            var color = new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 }; // White color

            IntPtr font = SDL_ttf.TTF_OpenFont(fonttext, 60);

            // Render the text
            IntPtr surfaceMessage = SDL_ttf.TTF_RenderText_Solid(font, livesText, color);
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
            SDL_ttf.TTF_CloseFont(font);
        }
    }
}