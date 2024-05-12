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

        
        */
        public float ghostRelativePosX;
        public float ghostRelativePosY;
        
        public double speed = 0.5;
        public double ghostVel = 10;

        public double SinusValue = 0;

        //The X and Y offsets
        public double posX = Program.SCREEN_WIDTH;
        public double posY;

        public bool alive = true;
        public int color = 0;

        private static LTexture ghostTexture = new LTexture();

        //Current animation frame
        public double frameTicker;
        public int frame;

        //animation
        private const int ANIMATION_FRAMES = 6;
        private static readonly SDL.SDL_Rect[] _SpriteClips = new SDL.SDL_Rect[ANIMATION_FRAMES];


        public Ghost(LTexture texture, int farbe)
        {
            ghostTexture = texture;
            color = farbe;
            if (ghostTexture == null)
            {
                Console.WriteLine("Failed to load media!");
                Console.ReadLine();
            }

            //Set sprite clips
            //Ghost 1
            _SpriteClips[0].x = 0;
            _SpriteClips[0].y = 0;
            _SpriteClips[0].w = 94;
            _SpriteClips[0].h = 143;

            _SpriteClips[1].x = 94;
            _SpriteClips[1].y = 0;
            _SpriteClips[1].w = 90;
            _SpriteClips[1].h = 143;

            //Ghost 2
            _SpriteClips[2].x = 184;
            _SpriteClips[2].y = 0;
            _SpriteClips[2].w = 94;
            _SpriteClips[2].h = 143;

            _SpriteClips[3].x = 278;
            _SpriteClips[3].y = 0;
            _SpriteClips[3].w = 90;
            _SpriteClips[3].h = 143;

            //Ghost 3
            //Set sprite clips
            _SpriteClips[4].x = 368;
            _SpriteClips[4].y = 0;
            _SpriteClips[4].w = 94;
            _SpriteClips[4].h = 143;

            _SpriteClips[5].x = 462;
            _SpriteClips[5].y = 0;
            _SpriteClips[5].w = 90;
            _SpriteClips[5].h = 143;



        }
        public void move(double deltaTime)
        {
           posX -= ghostVel * (deltaTime / 10) * speed;

            if (posX < 0 - ghostW)
            {
                posX = Program.SCREEN_WIDTH;
            }

            SinusValue += (0.05); // länge der Amplitude
            posY = posY + Math.Sin(SinusValue) * Program.pannelH/70; // Höhe der Amplitude


        }

        public void render()
        {
            if (alive)
            {
                
                //Animation Speed
                if (frameTicker >= 0.5)
                {
                    int c2 = color + 1;
                    switch (color)
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
                // Console.WriteLine(currentClip.w + currentClip.h);
                SDL.SDL_Rect pannelRect = new SDL.SDL_Rect { x = (int)posX - 15 - 46 , y = (int)posY + 15, w = 46 , h = 70 };


               // ghostTexture.render(((int)System.Math.Floor(posX)), (int)System.Math.Floor(posY), currentClip);


                SDL.SDL_RenderCopy(Program.gRenderer, ghostTexture.getTexture(), ref currentClip, ref pannelRect);

              //  SDL.SDL_SetRenderDrawColor(Program.gRenderer, 0xFF, 0x00, 0x00, 0xFF);
              //  SDL.SDL_RenderFillRect(Program.gRenderer, ref pannelRect); //TestFeld

              //  SDL.SDL_BlitScaled(ghostSurface, IntPtr.Zero, Program.gWindow, ref pannelRect);
            }

        }

    }
}
