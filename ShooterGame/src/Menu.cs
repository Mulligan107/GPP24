using System;
using SDL2;

namespace ShooterGame
{
    public class Menu
    {
        //Globally used font
        public static IntPtr Font = IntPtr.Zero;
        
        public static void penisVerlaengern()
        {
            SDL_ttf.TTF_Init();
            Font = SDL_ttf.TTF_OpenFont("lazy.ttf", 28);
            
            SDL.SDL_Color white = new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 };
            
            var surface = SDL_ttf.TTF_RenderText_Solid(Font, "Testtext", white);
            var texture = SDL.SDL_CreateTextureFromSurface(Program.gRenderer, surface);
            
            SDL.SDL_Rect messageRect = new SDL.SDL_Rect { x = 0, y = 0, w = 100, h = 100 };
            
            SDL.SDL_RenderCopy(Program.gRenderer, texture, IntPtr.Zero, ref messageRect);
            
            SDL.SDL_FreeSurface(surface);
        }
    }
}