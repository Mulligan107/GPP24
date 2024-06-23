using System;
using SDL2;

namespace JumperGame.src.manager
{
    public class FontManager
    {
        public bool Initialize()
        {
            //Initialize SDL_ttf
            if (SDL_ttf.TTF_Init() == -1)
            {
                Console.WriteLine("SDL_ttf could not initialize! SDL_ttf Error: {0}", SDL.SDL_GetError());
                return false;
            }
            return true;
        }
    }
}