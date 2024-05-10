using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PongGame
{
    class Ghost
    {
        
        public double ghostW = 94;
        public static double ghostH = 143;

        /*
        //Vector óf 
        public double vectorX;
        public double vectorY;

        public double speed = 0.5;

        public float ghostRelativePosX;
        public float ghostRelativePosY;
        */
        public double ghostVel = 10;

        //The X and Y offsets
        public double posX = Program.SCREEN_WIDTH;
        public double posY = (Program.SCREEN_HEIGHT - (int)ghostH) / 2;

        public bool alive = true;

        private static LTexture ghostTexture = new LTexture();

        //Current animation frame
        public double frameTicker;
        public int frame;

        //animation
        private const int ANIMATION_FRAMES = 2;
        private static readonly SDL.SDL_Rect[] _SpriteClips = new SDL.SDL_Rect[ANIMATION_FRAMES];


        public Ghost(LTexture texture)
        {
            ghostTexture = texture;

            if (ghostTexture == null)
            {
                Console.WriteLine("Failed to load media!");
                Console.ReadLine();
            }

            //Set sprite clips
            _SpriteClips[0].x = 0;
            _SpriteClips[0].y = 0;
            _SpriteClips[0].w = 94;
            _SpriteClips[0].h = 143;

            _SpriteClips[1].x = 94;
            _SpriteClips[1].y = 0;
            _SpriteClips[1].w = 94;
            _SpriteClips[1].h = 143;

        }
        public void move(double deltaTime)
        {
           posX -= ghostVel * (deltaTime / 10);

            if (posX < 0)
            {
                posX = Program.SCREEN_WIDTH;
            }
        }

        public void render()
        {
            if (alive)
            {
                Console.WriteLine(frameTicker);

                //Animation Speed
                if (frameTicker >= 0.5)
                {
                    switch (frame)
                    {
                        case (0):
                            frame = 1;
                            break;
                        case (1):
                            frame = 0;
                            break;
;                    }
                    frameTicker = 0;
                }
                //Render current frame
                SDL.SDL_Rect currentClip = _SpriteClips[frame];
                Console.WriteLine(currentClip.w + currentClip.h);
                ghostTexture.render(((int)System.Math.Floor(posX)), (int)System.Math.Floor(posY), currentClip);

            }

        }

    }
}
