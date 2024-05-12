using SDL2;
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
        public double mPosY = (Program.SCREEN_HEIGHT / 2) + Program.pannelH;

        public float kugRelativePosX;
        public float kugRelativePosY;

        public LTexture gDotTexture = new LTexture();

        public int letzteFarbe = 0;


        //animation
        private const int ANIMATION_FRAMES = 3;
        private static readonly SDL.SDL_Rect[] _SpriteClips = new SDL.SDL_Rect[ANIMATION_FRAMES];


        //Initializes the variables
        public Ball(LTexture texture)
        {
            gDotTexture = texture;

            if (gDotTexture == null)
            {
                Console.WriteLine("Failed to load media!");
                Console.ReadLine();
            }

            //Set sprite clips
            _SpriteClips[0].x = 0;
            _SpriteClips[0].y = 0;
            _SpriteClips[0].w = 21;
            _SpriteClips[0].h = 21;

            _SpriteClips[1].x = 21;
            _SpriteClips[1].y = 0;
            _SpriteClips[1].w = 21;
            _SpriteClips[1].h = 21;

            _SpriteClips[2].x = 42;
            _SpriteClips[2].y = 0;
            _SpriteClips[2].w = 21;
            _SpriteClips[2].h = 21;


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
                startPos((Program.SCREEN_WIDTH / 2) , (Program.SCREEN_HEIGHT / 2) + Program.pannelH);
                vectorX = getRandomVector();
                vectorY = getRandomVector();
                Program.p1counter++;
                changeColor();
            }

            if (mPosX + dotW > Program.SCREEN_WIDTH)
            {
                //Move back
                //changeDir(0);
                startPos((Program.SCREEN_WIDTH / 2), (Program.SCREEN_HEIGHT / 2) + Program.pannelH);
                vectorX = getRandomVector();
                vectorY = getRandomVector();
                Program.p2counter++;
                changeColor();
            }


            //If the dot went too far up or down
            if ((mPosY < Program.pannelH)) // 100 mit Boarder
            {
                //Move back
                changeDir(1);
                changeColor();
                mPosY += 5;
            }
            if (mPosY + dotH > Program.SCREEN_HEIGHT) {
                //Move back
                changeDir(1);
                changeColor();
                mPosY -= 5;
            }

            //Console.WriteLine("mPosX:{0};mVelX:{1};mPosY:{2};mVelY:{3}", mPosX, mVelX, mPosY, mVelY);
        }

        //Shows the dot on the screen
        public void render()
        {
            //Render current frame
            SDL.SDL_Rect currentClip = _SpriteClips[letzteFarbe];
            //Show the dot
            gDotTexture.render(((int)System.Math.Floor(mPosX)), (int)System.Math.Floor(mPosY),currentClip);
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
                    letzteFarbe = 1;
                    break;
                case (1):
                    letzteFarbe = 2;
                    break;
                case (2):
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