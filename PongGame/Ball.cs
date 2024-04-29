using System;
using System.Runtime.InteropServices;
using SDL2;

namespace PongGame
{
    //Texture wrapper class
    class Ball
    {
        //The dimensions of the dot
        public int dotW = 20;
        public int dotH = 20;

        //Maximum axis velocity of the dot
        int DOT_velX = 5;
        int DOT_velY = 5;


        //The X and Y offsets of the dot
        public int mPosX, mPosY;

        //The velocity of the dot
        int mVelX, mVelY;

        //Initializes the variables
        public Ball()
        {

        }
        public void startPos(int poX, int poY)
        {
            mPosX = poX;
            mPosY = poY;
        }

        //Moves the dot
        public void move()
        {
            //var a = string.Format("mPosX:{0};mVelX:{1};mPosY:{2};mVelY:{3}", mPosX, mVelX, mPosY, mVelY);
            //Console.WriteLine(a);

            mPosX += DOT_velX;
            mPosY += DOT_velY;


            //If the dot went too far to the left or right
            if (mPosX < 0)
            {
                //Move back
                changeDir(0);
                Program.p1counter++;
            }

            if (mPosX + dotW > Program.SCREEN_WIDTH)
            {
                //Move back
                changeDir(0);
                Program.p2counter++;
            }


            //If the dot went too far up or down
            if ((mPosY < 0) || (mPosY + dotH > Program.SCREEN_HEIGHT)) // 100 mit Boarder
            {
                //Move back
                changeDir(1);
            }

            //Console.WriteLine("mPosX:{0};mVelX:{1};mPosY:{2};mVelY:{3}", mPosX, mVelX, mPosY, mVelY);
        }

        //Shows the dot on the screen
        public void render()
        {
            //Show the dot
            Program.gDotTexture.render(mPosX, mPosY);
        }

        // X == 0 , Y == 1
        public int changeDir(int dir)
        {
            if (dir == 0)
            {
                return DOT_velX = (DOT_velX * -1);
            }
            else
            {
                return DOT_velY = (DOT_velY * -1);
            }
        }

    }

}