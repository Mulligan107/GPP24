using System;
using SDL2;

namespace ShooterGame
{
    class BackgroundObject
    {
        public BackgroundObject()
        {
        }

        public void setBGColor()
        {
            SDL.SDL_SetRenderDrawColor(Program.gRenderer, 0xFF, 0xFF, 0xFF, 255);
            SDL.SDL_RenderClear(Program.gRenderer);

        }

    }
}
    
