using System;
using SDL2;

namespace ShooterGame
{
    class Player : Entity
    {

        
        public Player(LTexture tex)
        {
            spawn(Program.SCREEN_WIDTH / 2, Program.SCREEN_HEIGHT / 2);
          //  SDL.SDL_Rect pannelRect = new SDL.SDL_Rect { x = 0, y = 0, w = tex.getWidth(), h = tex.getHeight() };
            render(tex);

        }

    }
}


