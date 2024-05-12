using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongGame
{
    class Knight
    {

        public double knightW = 94;
        public static double knightH = 143;

        /*
        //Vector óf 
        public double vectorX;
        public double vectorY;

        
        */
        public float knightRelativePosX;
        public float knightRelativePosY;

        //The X and Y offsets
        public double posX;
        public double posY;

        public static LTexture knightTexture = new LTexture();

        //Current animation frame
        public double frameTicker;
        public int frame;
        public static int anzahlFrames;

        public bool endofAnimation;
        public bool activAttack = false;

        //animation
        private static int ANIMATION_FRAMES;
        private static  SDL.SDL_Rect[] _SpriteClips;


        public Knight(LTexture texture)
        {
            knightTexture = texture;
        }

        public void animate(double deltaTime)
        {

        }

        public  void updateKnightTexture()
        {
            knightTexture = Program.knightTexture;

            if (knightTexture == null)
            {
                Console.WriteLine("Failed to load media!");
                Console.ReadLine();
            }
            anzahlFrames = knightTexture.getWidth() / 120;

            _SpriteClips = new SDL.SDL_Rect[anzahlFrames];
            //Set sprite clips

            for (int i = 0; i < anzahlFrames; i++)
            {

                _SpriteClips[i].x = 0 + (120 * i);
                _SpriteClips[i].y = 0;
                _SpriteClips[i].w = 120;
                _SpriteClips[i].h = 80;
            }
            frame = 0;
            frameTicker = 0;
            endofAnimation = false;
        }

        public void render()
        {
            Console.WriteLine(frame);
            Console.WriteLine(anzahlFrames);
            //Animation Speed
            if (frameTicker >= 0.5)
                {
                    if (frame >= anzahlFrames -1)
                    {
                        frame = 0;
                        endofAnimation = true;
                    }
                    else
                    {
                        frame++;
                        endofAnimation = false;
                }
                    frameTicker = 0;
                }
                //Render current frame
                SDL.SDL_Rect currentClip = _SpriteClips[frame];
                // Console.WriteLine(currentClip.w + currentClip.h);
                
                SDL.SDL_Rect pannelRect = new SDL.SDL_Rect { x = -50, y = -50, w = 120 * 3, h = 80 * 3 };


                // knightTexture.render(((int)System.Math.Floor(posX)), (int)System.Math.Floor(posY), currentClip);


                SDL.SDL_RenderCopy(Program.gRenderer, knightTexture.getTexture(), ref currentClip, ref pannelRect);

                //  SDL.SDL_SetRenderDrawColor(Program.gRenderer, 0xFF, 0x00, 0x00, 0xFF);
                //  SDL.SDL_RenderFillRect(Program.gRenderer, ref pannelRect); //TestFeld

                //  SDL.SDL_BlitScaled(knightSurface, IntPtr.Zero, Program.gWindow, ref pannelRect);
            }

        }

    }

