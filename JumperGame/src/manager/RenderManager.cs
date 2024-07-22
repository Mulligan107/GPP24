using JumperGame;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SDL2;
using System.Text.RegularExpressions;
using System.Timers;
using TiledCSPlus;
using JumperGame.gameEntities;
using JumperGame.components;
using System.Numerics;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using JumperGame.src.components;

namespace JumperGame.src.manager
{
    class RenderManager
    {
        //Screen dimension constants
        public int ScreenWidth;
        public int ScreenHeight;
        public int MaxScreenWidth;
        public int MaxScreenHeight;
        public int AltScreenWidth;
        public int AltScreenHeight;

        public int levelWidth = 4096;
        public int levelHeight = 4096;
        
        //The surface contained by the window
        public static IntPtr gRenderer = IntPtr.Zero;
        //The window we'll be rendering to
        public static IntPtr gWindow = IntPtr.Zero;

        LTexture timerTexture = new LTexture();
        LTexture bg = new LTexture();
        double counter = 0;
        int frame = 1;

        public static IntPtr Font = IntPtr.Zero;

        public RenderManager()
        {
            Initialize();
        }
        
        public void Update(double dt, double timeElapsed)
        {
            SDL.SDL_RenderClear(gRenderer);

            SDL.SDL_Rect camera = new SDL.SDL_Rect { w = ScreenWidth, h = ScreenHeight };

            Entity player = JumperGame._entitySystem.GetEntityByGID(281);
            var posi = player.GetComponent<RenderComponent>();
            SDL.SDL_Rect newPosi = posi.dstRect;

            var s = ScreenWidth / ScreenHeight;

            //Center the camera over the dot
            camera.x = (int)(newPosi.x + newPosi.w / 2) - (camera.w / 2 ) / 3; // TODO (/3) mit variable ersetzen
            camera.y = (int)(newPosi.y + newPosi.h / 2) - (camera.h / 2) / 3;

            //Console.WriteLine("CAM: " + camera.x);
            
            
            //Keep the camera in bounds
            if (camera.x < 0)
            {
                camera.x = 0;
            }
            if (camera.y < 0)
            {
                camera.y = 0;
            }
            if (camera.x > levelWidth - camera.w)
            {
                camera.x = levelWidth - camera.w;
            }
            if (camera.y > levelHeight - camera.h)
            {
                camera.y = levelHeight - camera.h;
            }


            SDL.SDL_RenderSetScale(gRenderer, 3f, 3f); // TODO 3 mit variable ersetzen ; Mausrad?

            //Render background

            bg.render(0, 0, camera);

            foreach (Entity enti in JumperGame._entitySystem.GetAllEntities())
            {
                var renderComponent = enti.GetComponent<RenderComponent>();
                var animationComponent = enti.GetComponent<AnimationComponent>();

                

                if (renderComponent != null)
                {

                    SDL.SDL_Rect src = renderComponent.srcRect;
                    SDL.SDL_Rect dst = renderComponent.dstRect;

                    // ---------- AnimationManager?
                    if (enti == player)
                    {
                        Console.WriteLine("RENDERMANAGER/ STATE: " + enti.activeSTATE);
                        Console.WriteLine("RENDERMANAGER/ GROUNDED: " + player.GetComponent<PhysicsComponent>().Grounded);
                    }


                    if (animationComponent != null) 
                    {

                    if (timeElapsed > counter)
                        {
                            counter = timeElapsed + animationComponent.duration * 0.0005;
                            frame++;
                        }

                    if (animationComponent.animationFrame < animationComponent.AnimimationList.Length-1)
                        {
                            animationComponent.animationFrame = frame;
                        }
                    else
                        {
                            animationComponent.animationFrame = 1;
                            frame = 1;
                        }

                        SDL.SDL_Rect loopSrc = animationComponent.Update(timeElapsed);


                        src = loopSrc;
                    }
                    // ---------- AnimationManager?


                    SDL.SDL_Rect adjustedDst = new SDL.SDL_Rect
                    {
                        x = dst.x - camera.x,
                        y = dst.y - camera.y,
                        w = dst.w,
                        h = dst.h
                    };

                    if (SDL.SDL_HasIntersection(ref dst, ref camera) == SDL.SDL_bool.SDL_TRUE)
                    {
                        SDL.SDL_RenderCopyEx(gRenderer, renderComponent.Rendertexture.getTexture(), ref src, ref adjustedDst, renderComponent.angle, ref renderComponent.centerPoint, renderComponent.flip);
                    }

                }
            }


            //Render objects
            
            

            timerTexture = changeText(timerTexture, "Delta: " + dt.ToString("F3") + "\n Timer: " + timeElapsed.ToString("F3"));
            timerTexture.render(10, 10);

            SDL.SDL_RenderPresent(gRenderer);
        }

        

        static LTexture changeText(LTexture Ltex, String text)
        {
            Ltex.loadFromRenderedText(text, new SDL.SDL_Color());
            return Ltex;
        }

        

        public bool Initialize()
        {
            SDL.SDL_SetHint(SDL.SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING, "1");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            
            //Initialize SDL
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                Console.WriteLine("SDL could not initialize! SDL_Error: {0}", SDL.SDL_GetError());
                return false;
            }
            else
            {
                //Set texture filtering to linear
                if (SDL.SDL_SetHint(SDL.SDL_HINT_RENDER_SCALE_QUALITY, "0") == SDL.SDL_bool.SDL_FALSE)
                {
                    Console.WriteLine("Warning: Linear texture filtering not enabled!");
                }

                //Get display mode for the current display
                SDL.SDL_DisplayMode current;
                if (SDL.SDL_GetCurrentDisplayMode(0, out current) == 0)
                {
                    MaxScreenWidth = current.w;
                    MaxScreenHeight = current.h;

                    AltScreenWidth = (int)(MaxScreenWidth * 0.75);
                    AltScreenHeight = (int)(MaxScreenHeight * 0.75);

                }
                else
                {
                    Console.WriteLine("Could not get display mode for video display: {0}", SDL.SDL_GetError());
                    return false;
                }

                //Set initial screen size
                ScreenWidth = MaxScreenWidth;
                ScreenHeight = MaxScreenHeight;

                //Create window
                gWindow = SDL.SDL_CreateWindow("JumperGame", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED,
                    ScreenWidth, ScreenHeight, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
                if (gWindow == IntPtr.Zero)
                {
                    Console.WriteLine("Window could not be created! SDL_Error: {0}", SDL.SDL_GetError());
                    return false;
                }
                else
                {
                    //Create vsynced renderer for window
                    var renderFlags = SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
                                      SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC;
                    gRenderer = SDL.SDL_CreateRenderer(gWindow, -1, renderFlags);
                    if (gRenderer == IntPtr.Zero)
                    {
                        Console.WriteLine("gRenderer could not be created! SDL Error: {0}", SDL.SDL_GetError());
                        return false;
                    }
                    else
                    {
                        //Initialize renderer color
                        SDL.SDL_SetRenderDrawColor(gRenderer, 0xFF, 0xFF, 0xFF, 0x00);

                        //Initialize PNG loading
                        var imgFlags = SDL_image.IMG_InitFlags.IMG_INIT_PNG;
                        if ((SDL_image.IMG_Init(imgFlags) > 0 & imgFlags > 0) == false)
                        {
                            Console.WriteLine("SDL_image could not initialize! SDL_image Error: {0}",
                                SDL.SDL_GetError());
                            return false;
                        }

                        //Initialize SDL_ttf
                        if (SDL_ttf.TTF_Init() == -1)
                        {
                            Console.WriteLine("SDL_ttf could not initialize! SDL_ttf Error: {0}", SDL.SDL_GetError());
                            return false;
                        }
                    }
                }
            }

            
            Font = SDL_ttf.TTF_OpenFont("lazy.ttf", 28);


            bg.loadFromFile("src\\tilesets/bg.png");

            return true;
        }
    }
}
