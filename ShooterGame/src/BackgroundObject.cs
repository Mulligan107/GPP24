using System;
using System.Collections;
using System.Collections.Generic;
using SDL2;

namespace ShooterGame
{
    class BackgroundObject : Entity
    {
        public List<LTexture> bgList;
        public BackgroundObject(List<LTexture> list)
        {
            posX = 0;
            bgList = list;
            texture = bgList[0];
            height = Program.SCREEN_HEIGHT * 2;
            width = Program.SCREEN_WIDTH * 2;
            vecX = -1 * s;

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

        public BackgroundObject copy(double pos, int chooseTexture, double speed)
        {
            BackgroundObject bg = new BackgroundObject(bgList);
            bg.texture = bgList[chooseTexture];
            bg.posX = pos;
            bg.vecX *= speed;
            return bg;
        }
    }
}
    
