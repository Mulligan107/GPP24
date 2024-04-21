using System;
using System.Globalization;
using System.Threading;
using SDL2;

namespace PongGame
{
    class Program
    {
        //Screen dimension constants
        public const int SCREEN_WIDTH = 640;

        public const int SCREEN_HEIGHT = 480;

        public int player1Counter = 0;
        public int player2Counter = 0;

        //The window we'll be rendering to
        private static IntPtr gWindow = IntPtr.Zero;

        //The surface contained by the window
        public static IntPtr gRenderer = IntPtr.Zero;

        //Globally used font
        public static IntPtr Font = IntPtr.Zero;

        //Scene textures
        public static LTexture gDotTexture = new LTexture();
        public static LTexture gBarTexture = new LTexture();
        //Rendered texture
        private static readonly LTexture _TextTexture = new LTexture();

        //Game ticker
        public static LTimer timer = new LTimer();
        

        private static bool init()
        {
            //Initialization flag
            bool success = true;

            //Initialize SDL
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                Console.WriteLine("SDL could not initialize! SDL_Error: {0}", SDL.SDL_GetError());
                success = false;
            }
            else
            {
                //Set texture filtering to linear
                if (SDL.SDL_SetHint(SDL.SDL_HINT_RENDER_SCALE_QUALITY, "1") == SDL.SDL_bool.SDL_FALSE)
                {
                    Console.WriteLine("Warning: Linear texture filtering not enabled!");
                }

                //Create window
                gWindow = SDL.SDL_CreateWindow("PongGame", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED,
                    SCREEN_WIDTH, SCREEN_HEIGHT, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
                if (gWindow == IntPtr.Zero)
                {
                    Console.WriteLine("Window could not be created! SDL_Error: {0}", SDL.SDL_GetError());
                    success = false;
                }
                else
                {
                    //Create vsynced renderer for window
                    var renderFlags = SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC;
                    gRenderer = SDL.SDL_CreateRenderer(gWindow, -1, renderFlags);
                    if (gRenderer == IntPtr.Zero)
                    {
                        Console.WriteLine("Renderer could not be created! SDL Error: {0}", SDL.SDL_GetError());
                        success = false;
                    }
                    else
                    {
                        //Initialize renderer color
                        SDL.SDL_SetRenderDrawColor(gRenderer, 0xFF, 0xFF, 0xFF, 0xFF);

                        //Initialize PNG loading
                        var imgFlags = SDL_image.IMG_InitFlags.IMG_INIT_PNG;
                        if ((SDL_image.IMG_Init(imgFlags) > 0 & imgFlags > 0) == false)
                        {
                            Console.WriteLine("SDL_image could not initialize! SDL_image Error: {0}", SDL.SDL_GetError());
                            success = false;
                        }
                    }
                }
                /*
                //Initialize SDL_ttf
                if (SDL_ttf.TTF_Init() == -1)
                {
                    Console.WriteLine("SDL_ttf could not initialize! SDL_ttf Error: {0}", SDL.SDL_GetError());
                    success = false;
                }
                */
            }

            return success;
        }


        static bool loadMedia()
        {
            //Loading success flag
            bool success = true;

            //Load press texture
            if (!gDotTexture.loadFromFile("dot.bmp"))
            {
                Console.WriteLine("Failed to load!");
                success = false;
            }
            if (!gBarTexture.loadFromFile("bar.bmp"))
            {
                Console.WriteLine("Failed to load!");
                success = false;
            }
            /*
            //Open the font
            Font = SDL_ttf.TTF_OpenFont("runescape_uf.ttf", 28);
            if (Font == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load font! SDL_ttf Error: {0}", SDL.SDL_GetError());
                success = false;
            }
            else
            {
                //Render text
                var textColor = new SDL.SDL_Color();
                if (!_TextTexture.loadFromRenderedText("The quick brown fox jumps over the lazy dog", textColor))
                {
                    Console.WriteLine("Failed to render text texture!");
                    success = false;
                }
            }
            */
            return success;
        }

        private static void close()
        {
            //Free loaded images
            gDotTexture.free();
            gBarTexture.free();


            //Free loaded images
            _TextTexture.free();

            /*
            //Free global font
            SDL_ttf.TTF_CloseFont(Font);
            Font = IntPtr.Zero;
            */

            //Destroy window
            SDL.SDL_DestroyRenderer(gRenderer);
            SDL.SDL_DestroyWindow(gWindow);
            gWindow = IntPtr.Zero;
            gRenderer = IntPtr.Zero;
            timer.stop();

            //Quit SDL subsystems
            SDL_image.IMG_Quit();
            SDL.SDL_Quit();
        }

        static void collCheck(Dot dot, Kug kug)
        {
            //Versändlichere kurze Namen 
            int kugL = kug.mPosX;
            int kugR = kug.mPosX + kug.dotW;
            int kugOb = kug.mPosY;
            int kugUn = kug.mPosY + kug.dotH;

            int playL = dot.mPosX;
            int playR = dot.mPosX + dot.dotW;
            int playOb = dot.mPosY;
            int playUn = dot.mPosY + dot.dotH;


            if (kugUn > playOb && kugOb < playUn && kugR > playR)
            {
                Console.WriteLine(true);
            }else
            {
                Console.WriteLine(false);
            }


                //Bedingung X
                if (kugL < playR && kugUn > playOb && kugOb < playUn && kugR > playR)
            {
                kug.changeDir(0);
            }
        }

        static int Main(string[] args)
        {
            SDL.SDL_SetHint(SDL.SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING, "1");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            timer.start(); //Später wichtig für Pause und Zeit


            //Start up SDL and create window
            var success = init();
            if (success == false)
            {
                Console.WriteLine("Failed to initialize!");
            }
            else
            {
                //Load media
                success = loadMedia();
                if (success == false)
                {
                    Console.WriteLine("Failed to load media!");
                }
                else
                {
                    //Main loop flag
                    bool quit = false;

                    //Event handler
                    SDL.SDL_Event e;

                    //The dot that will be moving around on the screen
                    Dot dot = new Dot();
                    Kug kug = new Kug();

                    //Set Startpos
                    kug.startPos((SCREEN_WIDTH/2), (SCREEN_HEIGHT/2));
                    dot.startPos(0, 100);

                    //While application is running
                    while (!quit)
                    {
                        //Handle events on queue
                        while (SDL.SDL_PollEvent(out e) != 0)
                        {
                            //User requests quit
                            if (e.type == SDL.SDL_EventType.SDL_QUIT)
                            {
                                quit = true;
                            }

                            //Handle input for the dot
                            dot.handleEvent(e);
                        }

                        collCheck(dot, kug);

                        //Move the dot
                        dot.move();
                        kug.move();

                        
                       // Console.WriteLine(kug.mPosX);

                        //Clear screen
                        SDL.SDL_SetRenderDrawColor(gRenderer, 0xFF, 0xFF, 0xFF, 0xFF);
                        SDL.SDL_RenderClear(gRenderer);

                        //Begrenzung
                        var blackline = new SDL.SDL_Rect { x = 0, y = 100, w = SCREEN_WIDTH, h = 2 };
                        SDL.SDL_SetRenderDrawColor(gRenderer, 0x00, 0x00, 0x00, 0xFF);
                        SDL.SDL_RenderFillRect(gRenderer, ref blackline);

                        //Render current frame
                      //  _TextTexture.render((SCREEN_WIDTH - _TextTexture.GetWidth()) / 2, (SCREEN_HEIGHT - _TextTexture.GetHeight()) / 2);

                        //Render objects
                        dot.render();
                        kug.render();

                        //Update screen
                        SDL.SDL_RenderPresent(gRenderer);
                    }
                }
            }


            //Free resources and close SDL
            close();

            if (success == false)
                Console.ReadLine();

            return 0;
        }

        //Key press surfaces constants
        public enum KeyPressSurfaces
        {
            KEY_PRESS_SURFACE_DEFAULT,
            KEY_PRESS_SURFACE_UP,
            KEY_PRESS_SURFACE_DOWN,
            KEY_PRESS_SURFACE_LEFT,
            KEY_PRESS_SURFACE_RIGHT,
            KEY_PRESS_SURFACE_TOTAL
        };

    }

}
