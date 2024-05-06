using System;

namespace PongGame
{
    //Texture wrapper class
    class Ball
    {
        //The dimensions of the dot
        public double dotW = 20;
        public double dotH = 20;

        //Vector óf 
        public double vectorX = 5;
        public double vectorY = 5;

        public double speed = 0.5;


        //The X and Y offsets of the dot
        public double mPosX = Program.SCREEN_WIDTH / 2;
        public double mPosY = Program.SCREEN_HEIGHT / 2;

        public float kugRelativePosX;
        public float kugRelativePosY;

        public LTexture gDotTexture = new LTexture();

        public int letzteFarbe = 0;

        //Initializes the variables
        public Ball()
        {
            //Load press texture
            if (!gDotTexture.loadFromFile("imgs/dot.bmp"))
            {
                Console.WriteLine("Failed to load!");
                //success = false;
            }
            changeColor();
            vectorX = getRandomVector();
            vectorY = getRandomVector();
        }
        public void startPos(double poX, double poY)
        {
            mPosX = poX;
            mPosY = poY;
        }

        //Moves the dot
        public void move(double deltaTime)
        {
            //var a = string.Format("mPosX:{0};mVelX:{1};mPosY:{2};mVelY:{3}", mPosX, mVelX, mPosY, mVelY);
            //Console.WriteLine(a);

            mPosX += vectorX * (deltaTime / 10) * speed;
            mPosY += vectorY * (deltaTime / 10) * speed;


            //If the dot went too far to the left or right
            if (mPosX < 0)
            {
                //Move back
                //changeDir(0);
                startPos((Program.SCREEN_WIDTH / 2), (Program.SCREEN_HEIGHT / 2));
                vectorX = getRandomVector();
                vectorY = getRandomVector();
                Program.p1counter++;
                changeColor();
            }

            if (mPosX + dotW > Program.SCREEN_WIDTH)
            {
                //Move back
                //changeDir(0);
                startPos((Program.SCREEN_WIDTH / 2), (Program.SCREEN_HEIGHT / 2));
                vectorX = getRandomVector();
                vectorY = getRandomVector();
                Program.p2counter++;
                changeColor();
            }


            //If the dot went too far up or down
            if ((mPosY < 0) || (mPosY + dotH > Program.SCREEN_HEIGHT)) // 100 mit Boarder
            {
                //Move back
                changeDir(1);
                changeColor();
            }

            //Console.WriteLine("mPosX:{0};mVelX:{1};mPosY:{2};mVelY:{3}", mPosX, mVelX, mPosY, mVelY);
        }

        //Shows the dot on the screen
        public void render()
        {
            //Show the dot
            gDotTexture.render(((int)System.Math.Floor(mPosX)), (int)System.Math.Floor(mPosY));
        }

        // X == 0 , Y == 1
        public double changeDir(double dir)
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

        public void changeColor()
        {
            //int switcher = Program.gRandom.Next(3);
            switch (letzteFarbe)
            {
                case (0):
                    gDotTexture.setColor(255, 0, 0);
                    letzteFarbe = 1;
                    break;
                case (1):
                    gDotTexture.setColor(0, 255, 0);
                    letzteFarbe = 2;
                    break;
                case (2):
                    gDotTexture.setColor(0, 0, 255);
                    letzteFarbe = 0;
                    break;
            }
        }




        public double getRandomVector() //ToDo Vecotren besser mit Double
        {
            double minimum = -5.0;
            double maximum = 5.0;

            
            double randValue = Program.gRandom.NextDouble() * (maximum - minimum) + minimum; 

            if ( randValue <= 1.0 && randValue >= -1.0)
            {
                if (randValue <= 0)
                {
                    randValue -= 1.0;
                }
                else
                {
                    randValue += 1.0;
                } 
            }

            return randValue;
        }

    }

}