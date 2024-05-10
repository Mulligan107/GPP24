using System;
using System.Runtime.InteropServices;
using SDL2;


namespace PongGame
{
    //Texture wrapper class
    class Paddle
    {
        //The dimensions of the dot
        public  double dotW = 20;
        public  double dotH = 200;

        //Maximum axis velocity of the dot
        public double DOT_VEL = 10;
        public double DOT_VEL_ENEMY = 5;


        //The X and Y offsets of the dot
        public double mPosX, mPosY;

        //The velocity of the dot
        public double mVelX, mVelY;

        //Initializes the variables
        public Paddle()
        {
          //  startPos(x, y);
        }
        public void startPos(double poX, double poY)
        {
            mPosX = poX;
            mPosY = poY;
        }

        //Takes key presses and adjusts the dot's velocity
        public void handleEvent(SDL.SDL_Event e)
        {
            //If a key was pressed
            if (e.type == SDL.SDL_EventType.SDL_KEYDOWN && e.key.repeat == 0)
            {
                //Adjust the velocity
                switch (e.key.keysym.sym)
                {
                    case SDL.SDL_Keycode.SDLK_UP: mVelY -= DOT_VEL; break;
                    case SDL.SDL_Keycode.SDLK_DOWN: mVelY += DOT_VEL; break;
                    //case SDL.SDL_Keycode.SDLK_LEFT: mVelX -= DOT_VEL; break;
                    //case SDL.SDL_Keycode.SDLK_RIGHT: mVelX += DOT_VEL; break;
                }
            }
            //If a key was released
            else if (e.type == SDL.SDL_EventType.SDL_KEYUP && e.key.repeat == 0)
            {
                //Adjust the velocity
                switch (e.key.keysym.sym)
                {
                    case SDL.SDL_Keycode.SDLK_UP: mVelY += DOT_VEL; break;
                    case SDL.SDL_Keycode.SDLK_DOWN: mVelY -= DOT_VEL; break;
                    //case SDL.SDL_Keycode.SDLK_LEFT: mVelX += DOT_VEL; break;
                    //case SDL.SDL_Keycode.SDLK_RIGHT: mVelX -= DOT_VEL; break;
                }
            }
        }

        //Moves the dot
        public void move(double deltaTime)
        {
            //var a = string.Format("mPosX:{0};mVelX:{1};mPosY:{2};mVelY:{3}", mPosX, mVelX, mPosY, mVelY);
            //Console.WriteLine(a);

            //Move the dot left or right
            mPosX += mVelX * (deltaTime / 10);

            //If the dot went too far to the left or right
            if ((mPosX < 0) || (mPosX + dotW > Program.SCREEN_WIDTH))
            {
                //Move back
                mPosX -= mVelX;
            }

            //Move the dot up or down
            mPosY += mVelY;

            //If the dot went too far up or down
            if ((mPosY < Program.pannelH) || (mPosY + dotH > Program.SCREEN_HEIGHT))
            {
                //Move back
                mPosY -= mVelY;
                DOT_VEL_ENEMY = DOT_VEL_ENEMY * (-1);
            }

            //Console.WriteLine("mPosX:{0};mVelX:{1};mPosY:{2};mVelY:{3}", mPosX, mVelX, mPosY, mVelY);
        }

        public void moveEnemy(double deltaTime)
        {
            mPosY -= DOT_VEL_ENEMY * (deltaTime / 10);

            if ((mPosY < Program.pannelH) || (mPosY + dotH > Program.SCREEN_HEIGHT))
            {
                DOT_VEL_ENEMY = DOT_VEL_ENEMY * (-1);
            }
        }

        //Shows the dot on the screen
        public void render()
        {
            //Show the dot
            Program.gBarTexture.render((int)System.Math.Floor(mPosX), (int)System.Math.Floor(mPosY));
        }

    }

}