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

        //Vector óf 
        public int vectorX = 5;
        public int vectorY = 5;


        //The X and Y offsets of the dot
        public int mPosX, mPosY;

        //The velocity of the dot
        int mVelX, mVelY;

        //Initializes the variables
        public Ball()
        {
            getRandomVector();
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

            mPosX += vectorX;
            mPosY += vectorY;


            //If the dot went too far to the left or right
            if (mPosX < 0)
            {
                //Move back
                //changeDir(0);
                startPos((Program.SCREEN_WIDTH / 2), (Program.SCREEN_HEIGHT / 2));
                getRandomVector();
                Program.p1counter++;
            }

            if (mPosX + dotW > Program.SCREEN_WIDTH)
            {
                //Move back
                //changeDir(0);
                startPos((Program.SCREEN_WIDTH / 2), (Program.SCREEN_HEIGHT / 2));
                getRandomVector();
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
                return vectorX = (vectorX * -1);
            }
            else
            {
                return vectorY = (vectorY * -1);
            }
        }

        public void getRandomVector() //ToDo Vecotren besser mit Double
        {
            Random r = new Random();

            int rIntX = r.Next(2);
            int rIntY = r.Next(2);

            if (rIntX == 0)
            {
                vectorX = -5;
            }
            else
            {
                vectorX = 5;
            }
            if (rIntY == 0)
            {
                vectorY = -5;
            }
            else
            {
                vectorY = 5;
            }


        }

    }

}