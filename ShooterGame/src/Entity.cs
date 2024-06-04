using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SDL2;

namespace ShooterGame
{
    class Entity : ICloneable
    {
        public double width;
        public double height;

        public double posX;
        public double posY;

        public double vecX = 0;
        public double vecY = 0;

        public double angle;

        public double speed = 1;

        public double lives;
        public bool iframe;

        public bool alive = true;

        public double s = Program.SCREEN_WIDTH / Program.SCREEN_HEIGHT;

        public LTexture texture;

        public List<LTexture> textureList;


        public int animationCounter;

        public SDL.SDL_Rect sorRect;
        public SDL.SDL_Rect destRect;
        private static SDL.SDL_Rect[] _SpriteClips;

        //Current animation frame
        public double frameTicker;
        public int frame;
        public static int anzahlFrames;
        public int repeats;
        public int choosenAnim;
        public int animationSpeed = 1;


        public bool startAnimation;

        public Entity()
        {
            
        }

        public void move(double deltaTime)
        {
            posX += vecX * (deltaTime / 10) * speed;
            posY += vecY * (deltaTime / 10) * speed;
        }

        public void kill()
        {
            alive = false;
        }
        public void killOutOfBounds()
        {
            if (posX < -100 || posX > Program.SCREEN_WIDTH + 100)
            {
                kill();
            }
            else if (posY < -100 || posY > Program.SCREEN_HEIGHT + 100)
            {
                kill();
            }
        }

        public void render()
        {
            if (alive)
            {
                Console.WriteLine(choosenAnim);
                //Rendert Basis Texture unter Schild
                if(choosenAnim == 3) {
                    SDL.SDL_RenderCopyEx(Program.gRenderer, textureList[0].getTexture(), ref sorRect, ref destRect, angle, IntPtr.Zero, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
                }
                SDL.SDL_RenderCopyEx(Program.gRenderer, texture.getTexture() , ref sorRect, ref destRect, angle, IntPtr.Zero, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
               // tex.render(((int)System.Math.Floor(posX)), (int)System.Math.Floor(posY));
            }
            
        }

        public void setupAnimation(int anzahlFrames)
        {
            if (textureList != null) //TODO ändern, 2 ist wenn die Liste eine Deathanimation hat
            {
                texture = textureList[choosenAnim];
                _SpriteClips = new SDL.SDL_Rect[anzahlFrames];
                //Set sprite clips

                for (int i = 0; i < anzahlFrames; i++)
                {
                    _SpriteClips[i].x = 0 + ((texture.getWidth() / anzahlFrames) * i);
                    _SpriteClips[i].y = 0;
                    _SpriteClips[i].w = texture.getWidth() / anzahlFrames;
                    _SpriteClips[i].h = texture.getHeight();
                }
                frame = 0;
                frameTicker = 0;
            }
            else
            {
                kill(); //TODO ist für Bullets, ändern
            }

            startAnimation = true;
        }


        public void update(double deltatime)
        {
            move(deltatime);

            destRect = new SDL.SDL_Rect { x = (int)System.Math.Floor(posX), y = (int)System.Math.Floor(posY), w = (int)System.Math.Floor(width), h = (int)System.Math.Floor(height) }; // Skalierung auf dieses Rect
            if (startAnimation && repeats > animationCounter)
            {
                frameTicker += deltatime/10 * animationSpeed;
                    if (frameTicker > 2) { 
                        if (frame < _SpriteClips.Length)
                        {
                            sorRect = _SpriteClips[frame];
                        }
                        else
                        {
                            frame = 0;
                            animationCounter++;
                            iframe = false;
                            if (choosenAnim == 2)
                            {
                                kill();
                            }
                        }
                        frame++;
                        frameTicker = 0;
                    }
                killOutOfBounds(); // Muss hier sein weil BGO keine Animation hat
            }
            else
            {
                startAnimation = false;
                if (textureList != null) //TODO Klassen ändern das sie immer Listen haben
                {
                    texture = textureList[0];
                }
                
                sorRect = new SDL.SDL_Rect { x = 0, y = 0, w = texture.getWidth(), h = texture.getHeight() };
            }
            render();
        }

        public void spawn(double x, double y)
        {
            posX = x; 
            posY = y; 
        }

        public object Clone()
        {
           return this.MemberwiseClone();
        }
    }

}