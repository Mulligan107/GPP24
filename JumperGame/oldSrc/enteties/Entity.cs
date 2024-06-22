using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        public bool friendly;
        public string animationFlag = "idle";

        public double s = Program.SCREEN_WIDTH / Program.SCREEN_HEIGHT;

        public LTexture texture;
        public LTexture overTexture;

        public List<LTexture> textureList;


        public int animationCounter;

        public SDL.SDL_Rect sourceRect;
        public SDL.SDL_Rect overSourceRect;
        public SDL.SDL_Rect destRect;
        public SDL.SDL_Rect hitbox;

        Dictionary<string, SDL.SDL_Rect[]> animationMap = new Dictionary<string, SDL.SDL_Rect[]>(); // Dictionary == HashMap, zum Mapping der Animationen

        //Current animation frame
        public double frameTicker;
        public int frame;
        public static int anzahlFrames;
        public int repeats;
        public int choosenAnim = 0;
        public int animationSpeed = 1;


        public bool startAnimation;

        public Entity()
        {
            animationMap.Add(animationFlag, null);
        }

        public virtual void move(double deltaTime)
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
                LTexture choosenTexture = new LTexture();
                if (textureList != null && !animationFlag.Equals("shield"))
                {
                    foreach (LTexture tex in textureList)
                    {
                        if (tex.getName().Contains(animationFlag))
                        {
                            choosenTexture = tex;
                            break;
                        }
                        else
                        {
                            choosenTexture = texture;
                        }
                    }
                }
                else
                {
                    choosenTexture = texture;
                }
                
                if (Program.debugMode)
                {
                    SDL.SDL_Rect debugSourceRect = hitbox;
                    debugSourceRect.h += 5;
                    debugSourceRect.w += 5;

                    SDL.SDL_SetRenderDrawColor(Program.gRenderer, 255, 0, 0, 255); // Red color for the border
                    SDL.SDL_RenderDrawRect(Program.gRenderer, ref debugSourceRect);
                    SDL.SDL_SetRenderDrawColor(Program.gRenderer, 255, 255, 255, 255); // Red color for the border
                }


                SDL.SDL_RenderCopyEx(Program.gRenderer, choosenTexture.getTexture() , ref sourceRect, ref destRect, angle, IntPtr.Zero, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
                //Rendert Basis Texture unter Schild
                if (iframe && !animationFlag.Equals("death"))
                {
                    SDL.SDL_RenderCopyEx(Program.gRenderer, overTexture.getTexture(), ref overSourceRect, ref destRect, angle, IntPtr.Zero, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
                }

                // tex.render(((int)System.Math.Floor(posX)), (int)System.Math.Floor(posY));
            }
            
        }
        public void animationHelper(int reps, int aniSpeed, string flag)
        {
            startAnimation = true;
            frame = 0;
            frameTicker = 0;
            animationCounter = 0;
            repeats = reps;
            animationSpeed = aniSpeed;
            animationFlag = flag;

        }

        public void setupAnimation(int anzahlFrames, string name, LTexture parameterTexture)
        {
            LTexture tex = parameterTexture;
            SDL.SDL_Rect[] _SpriteClips = new SDL.SDL_Rect[anzahlFrames];

            for (int i = 0; i < anzahlFrames; i++)
            {
                _SpriteClips[i].x = 0 + ((tex.getWidth() / anzahlFrames) * i);
                _SpriteClips[i].y = 0;
                _SpriteClips[i].w = tex.getWidth() / anzahlFrames;
                _SpriteClips[i].h = tex.getHeight();
            }

            animationMap.Add(name, _SpriteClips); 
        }


        public void handleAnimation(double deltatime, string flag)
        {

            SDL.SDL_Rect[] _SpriteClips = animationMap[flag];

            if (startAnimation && repeats > animationCounter)
            {
                frameTicker += deltatime / 10 * animationSpeed;
                if (frameTicker > 2)
                {
                    if (frame < _SpriteClips.Length)
                    {
                        if (flag.Equals("shield"))
                        {
                            overSourceRect = _SpriteClips[frame];
                            sourceRect = new SDL.SDL_Rect { x = 0, y = 0, w = texture.getWidth(), h = texture.getHeight() };
                        }
                        else
                        {
                            sourceRect = _SpriteClips[frame];
                        }     
                    }
                    else // Wenn die Animation durch ist
                    {
                        frame = 0;
                        animationCounter++;
                        if (flag.Equals("death"))
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
                iframe = false;
                animationFlag = "idle";
                if (textureList != null) //TODO Klassen ändern das sie immer Listen haben
                {
                    texture = textureList[0];
                }

                sourceRect = new SDL.SDL_Rect { x = 0, y = 0, w = texture.getWidth(), h = texture.getHeight() };
            }
        }

        public void update(double deltatime)
        {
            move(deltatime);

            destRect = new SDL.SDL_Rect { x = (int)System.Math.Floor(posX), y = (int)System.Math.Floor(posY), w = (int)System.Math.Floor(width), h = (int)System.Math.Floor(height) }; // Skalierung auf dieses Rect
            hitbox = shrinkRect(ref destRect);

            handleAnimation(deltatime, animationFlag);
            render();
        }

        public void spawn(double x, double y)
        {
            posX = x; 
            posY = y; 
        }

        static SDL.SDL_Rect shrinkRect(ref SDL.SDL_Rect rect)
        {
            SDL.SDL_Rect shrunkRect = new SDL.SDL_Rect { x = rect.x + rect.w / 4, y = rect.y + rect.h / 4, w = rect.w / 2, h = rect.h / 2 };
            return shrunkRect;
        }


        public object Clone()
        {
           return this.MemberwiseClone();
        }
    }

}