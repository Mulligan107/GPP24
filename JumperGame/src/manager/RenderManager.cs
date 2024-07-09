using JumperGame;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JumperGame.src.components.testComponents;
using SDL2;
using System.Text.RegularExpressions;
using System.Timers;
using TiledCSPlus;
using static System.Net.Mime.MediaTypeNames;
using JumperGame.gameEntities;
using JumperGame.components;

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
        
        //The surface contained by the window
        public static IntPtr gRenderer = IntPtr.Zero;
        //The window we'll be rendering to
        public static IntPtr gWindow = IntPtr.Zero;
        
        private ColorComponent _colorComponent;

        LTexture timerTexture = new LTexture();

        public static IntPtr Font = IntPtr.Zero;

        public RenderManager(ColorComponent colorComponent)
        {
            _colorComponent = colorComponent;
            _colorComponent.CurrentColor = new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 };
            Initialize();
        }
        
        public void Update(double dt, double timeElapsed)
        {
            SDL.SDL_RenderClear(gRenderer);
            SDL.SDL_SetRenderDrawColor(gRenderer, _colorComponent.CurrentColor.r, _colorComponent.CurrentColor.g, _colorComponent.CurrentColor.b, _colorComponent.CurrentColor.a);

            foreach (Entity enti in JumperGame._entitySystem.GetAllEntities()) {
                var comp = enti.GetComponent<RenderComponent>();
                SDL.SDL_Rect src = comp.srcRect;
                SDL.SDL_Rect dst = comp.dstRect;
                SDL.SDL_RenderCopy(gRenderer, comp.Rendertexture.getTexture(), ref src, ref dst);
                //Console.WriteLine(comp.Rendertexture.getTexture().ToString() + ": X: " + dst.x + " Y: " + dst.y+ " W: " + dst.w + " H : " + dst.h);
            }


            timerTexture = changeText(timerTexture,"Delta: " + dt.ToString("F3") + "\n Timer: " + timeElapsed.ToString("F3"));
            timerTexture.render(10,10);

            
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
                if (SDL.SDL_SetHint(SDL.SDL_HINT_RENDER_SCALE_QUALITY, "1") == SDL.SDL_bool.SDL_FALSE)
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
            

            return true;
        }
    }
}
