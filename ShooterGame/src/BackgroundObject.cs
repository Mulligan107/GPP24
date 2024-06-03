using System;
using System.Collections;
using System.Collections.Generic;
using SDL2;

namespace ShooterGame
{
    class BackgroundObject : Entity
    {
        public static ArrayList bgList = new ArrayList();
        public BackgroundObject(LTexture tex)
        {
            texture = tex;
            height = Program.SCREEN_HEIGHT * 2;
            width = Program.SCREEN_WIDTH * 2;
            vecX = -2 * s;

        }

        public void setBGColor()
        {
            SDL.SDL_SetRenderDrawColor(Program.gRenderer, 0xFF, 0xFF, 0xFF, 255);
            SDL.SDL_RenderClear(Program.gRenderer);

        }

        public void checkOutOfBounds()
        {
            if (posX < (width * -1))
            {
                posX = Program.SCREEN_WIDTH * 2;
            }
        }

        public BackgroundObject copy(double pos)
        {
            BackgroundObject bg = new BackgroundObject(texture);
            bg.posX = pos;
            return bg;
        }
    }
}
    
