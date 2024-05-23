using System;
using System.Collections;
using System.Globalization;
using System.Threading;
using SDL2;

/*


 */
namespace ShooterGame
{
    class Program
    {
        //Screen dimension constants
        public static int MAX_SCREEN_WIDTH;
        public static int MAX_SCREEN_HEIGHT;
        public static int ALT_SCREEN_WIDTH;
        public static int ALT_SCREEN_HEIGHT;
        public static int SCREEN_WIDTH;
        public static int SCREEN_HEIGHT;

        //Screen size mode
        public static bool isFullScreen = true;

        //The window we'll be rendering to
        public static IntPtr gWindow = IntPtr.Zero;

        //The surface contained by the window
        public static IntPtr gRenderer = IntPtr.Zero;

        //Globally used font
        public static IntPtr Font = IntPtr.Zero;

        //Game ticker
        public static LTimer timer = new LTimer();

        public static Random gRandom = new Random();

        public static bool quit = false;

        //Event handler
        static SDL.SDL_Event e;

        private static bool Init()
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

                //Get display mode for the current display
                SDL.SDL_DisplayMode current;
                if (SDL.SDL_GetCurrentDisplayMode(0, out current) == 0)
                {
                    MAX_SCREEN_WIDTH = current.w;
                    MAX_SCREEN_HEIGHT = current.h;

                    ALT_SCREEN_WIDTH = (int)(MAX_SCREEN_WIDTH * 0.75);
                    ALT_SCREEN_HEIGHT = (int)(MAX_SCREEN_HEIGHT * 0.75);

                }
                else
                {
                    Console.WriteLine("Could not get display mode for video display: {0}", SDL.SDL_GetError());
                    success = false;
                }

                //Set initial screen size
                SCREEN_WIDTH = MAX_SCREEN_WIDTH;
                SCREEN_HEIGHT = MAX_SCREEN_HEIGHT;

                //Create window
                gWindow = SDL.SDL_CreateWindow("ShooterGame", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED,
                    SCREEN_WIDTH, SCREEN_HEIGHT, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
                if (gWindow == IntPtr.Zero)
                {
                    Console.WriteLine("Window could not be created! SDL_Error: {0}", SDL.SDL_GetError());
                    success = false;
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
                        success = false;
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
                            success = false;
                        }

                        //Initialize SDL_ttf
                        if (SDL_ttf.TTF_Init() == -1)
                        {
                            Console.WriteLine("SDL_ttf could not initialize! SDL_ttf Error: {0}", SDL.SDL_GetError());
                            success = false;
                        }

                        //Initialize SDL_mixer
                        if (SDL_mixer.Mix_OpenAudio(44100, SDL_mixer.MIX_DEFAULT_FORMAT, 2, 2048) < 0)
                        {
                            Console.WriteLine("SDL_mixer could not initialize! SDL_mixer Error: {0}", SDL.SDL_GetError());
                            success = false;
                        }
                    }
                }
            }

            return success;
        }


        static bool LoadMedia()
        {
            //Loading success flag
            bool success = true;

            return success;
        }

        /**
         * Free media and shut down SDL
         */
        private static void Close()
        {
            //Destroy window
            SDL.SDL_DestroyRenderer(gRenderer);
            SDL.SDL_DestroyWindow(gWindow);
            gWindow = IntPtr.Zero;
            gRenderer = IntPtr.Zero;
            timer.stop();

            //Quit SDL subsystems
            SDL_ttf.TTF_Quit();
            SDL_image.IMG_Quit();
            SDL.SDL_Quit();
        }


        static void handleUserInput()
        {
            while (SDL.SDL_PollEvent(out e) != 0)
            {

                //User requests quit via closing the window or pressing esc
                if (e.type == SDL.SDL_EventType.SDL_QUIT || e.key.keysym.sym == SDL.SDL_Keycode.SDLK_ESCAPE)
                {
                    quit = true;
                }

                //Switch screen size mode if 'F' key was pressed
                if (e.type == SDL.SDL_EventType.SDL_KEYDOWN) //ToDo könnte ein switch case sein
                {
                    // Change screen size
                    if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_f)
                    {
                        // Change screen size
                        isFullScreen = !isFullScreen;
                        if (isFullScreen)
                        {
                            SCREEN_WIDTH = MAX_SCREEN_WIDTH;
                            SCREEN_HEIGHT = MAX_SCREEN_HEIGHT;

                            // Set the window to fullscreen
                            SDL.SDL_SetWindowFullscreen(gWindow,
                                (int)SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP);

                        }

                        else
                        {
                            SCREEN_WIDTH = ALT_SCREEN_WIDTH;
                            SCREEN_HEIGHT = ALT_SCREEN_HEIGHT;

                            // calculate the middle of screen
                            int windowPosX = (MAX_SCREEN_WIDTH - ALT_SCREEN_WIDTH) / 2;
                            int windowPosY = (MAX_SCREEN_HEIGHT - ALT_SCREEN_HEIGHT) / 2;

                            // Set the window position
                            SDL.SDL_SetWindowPosition(gWindow, windowPosX, windowPosY);

                            SDL.SDL_SetWindowFullscreen(gWindow, (int)SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
                        }

                        SDL.SDL_SetWindowSize(gWindow, SCREEN_WIDTH, SCREEN_HEIGHT);
                    }
                }
            }
        }

        static int Main(string[] args)
        {
            SDL.SDL_SetHint(SDL.SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING, "1");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            //Start up SDL and create window
            var success = Init();
            if (success == false)
            {
                Console.WriteLine("Failed to initialize!");
            }
            else
            {
                //Load media
                success = LoadMedia();

                if (success == false)
                {
                    Console.WriteLine("Failed to load media!");
                }
                else
                {
                    double previous = 0.0;
                    BackgroundObject bgo = new BackgroundObject();

                    //While application is running
                    while (!quit)
                    {
                        double current = timer.getTicks();
                        double elapsed = current - previous;
                        previous = current;
                        if (elapsed < 1.0)
                        {
                            elapsed = 5.0;
                        }
                        handleUserInput();

                        
                        bgo.setBGColor();


                        //Update screen
                        SDL.SDL_RenderPresent(gRenderer);
                    }
                }
            }

            //Free resources and close SDL
            Close();

            if (success == false)
                Console.ReadLine();

            return 0;
        }
    }
}