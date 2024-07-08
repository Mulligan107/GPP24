using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace JumperGame.src.manager
{
    class AudioManager
    {
        public bool Initialize()
        {
            //Initialize SDL
            if (SDL.SDL_Init(SDL.SDL_INIT_AUDIO) < 0)
            {
                Console.WriteLine("SDL could not initialize! SDL_Error: {0}", SDL.SDL_GetError());
                return false;
            }
            
            return true;
        }
    }
}
