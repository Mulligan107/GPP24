using System;
using System.Globalization;
using System.Threading;
using SDL2;


//TODO Collcheck oben unten, gegenerKI, Pausemenue, Physic?, Kugel Vel und richtung ändern Basierend auf Aufprall auf der Bar, etc

// TODO Bar ->  dotH = 100 , 0 - 50 vel porportialnal steigern -y , 51 - 100 vel prop steigern +y

//TODO Idee -> Pegglin Style. Drei Kugeln mit unterschiedlichen Farben. Mit 1,2,3 kann Farber der Bar geändert werden
// Oberer Screen zeigt Ritter mit konstant zulaufen Gegnern. Gegner haben auch unterschiedliche Farben. Schwert von Ritter 
// ist mit der Farbe der Bar verlinkt

/*
   x The player takes control of a ‘paddle’ and interacts with the gameworld by reflecting a ‘ball’. 
   x Keep track of and display a score! 
    There needs to be a game-over state and the option to play again.
 
+    Add sound and visual FX
+    Add an AI opponent or a multiplayer component.
    Add gamepad support.
+    Add interesting gameplay twists.
-    Add multiple levels.
+    Add additional screens. (e.g. Intro, Settings, Credits...)
-    Add support for a persistent Highscore.
    Add collectible Power-Ups.
+    Allow the game to be played in fullscreen mode.
  
 */


namespace PongGame
{
    class Program
    {
        //Screen dimension constants
        public const int SCREEN_WIDTH = 640;
        public const int SCREEN_HEIGHT = 480;

        public static int p1counter = 0;
        public static int p2counter = 0;


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

                //Create window
                gWindow = SDL.SDL_CreateWindow("SDL Tutorial", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED,
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
                        Console.WriteLine("gRenderer could not be created! SDL Error: {0}", SDL.SDL_GetError());
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

                        //Initialize SDL_ttf
                        if (SDL_ttf.TTF_Init() == -1)
                        {
                            Console.WriteLine("SDL_ttf could not initialize! SDL_ttf Error: {0}", SDL.SDL_GetError());
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

            //Load press texture
            if (!gDotTexture.loadFromFile("imgs/dot.bmp"))
            {
                Console.WriteLine("Failed to load!");
                success = false;
            }
            if (!gBarTexture.loadFromFile("imgs/player.bmp"))
            {
                Console.WriteLine("Failed to load!");
                success = false;
            }

            //Open the font
            Font = SDL_ttf.TTF_OpenFont("lazy.ttf", 28);
            if (Font == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load lazy font! SDL_ttf Error: {0}", SDL.SDL_GetError());
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

            return success;
        }

        
        /**
         * Free media and shut down SDL
         */
        private static void Close()
        {
            //Free loaded images
            gDotTexture.free();
            gBarTexture.free();

            //Free loaded images
            _TextTexture.free();

            //Free global font
            SDL_ttf.TTF_CloseFont(Font);
            Font = IntPtr.Zero;

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
        
        /**
         * Check collision between Dot and Kug
         */
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


            /*
            if (kugUn > playOb && kugOb < playUn && kugR > playR)
            {
                Console.WriteLine(true);
            }
            else
            {
                Console.WriteLine(false);
            }
            */


            //Bedingung Rechts
            if (kugL < playR && kugUn > playOb && kugOb < playUn && kugR > playR)
            {
                kug.changeDir(0);
            }
            //Bedingung Links
            if (kugL < playL && kugUn > playOb && kugOb < playUn && kugR > playL)
            {
                kug.changeDir(0);
            }
        }

        static void changeText(String text)
        {
            _TextTexture.loadFromRenderedText(text, new SDL.SDL_Color());
        }

        static int Main(string[] args)
        {
            

            SDL.SDL_SetHint(SDL.SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING, "1");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            timer.start(); //Später wichtig für Pause und Zeit

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
                    //Main loop flag
                    bool quit = false;
                    
                    //Event handler
                    SDL.SDL_Event e;

                    //The player that will be moving around on the screen
                    Dot player = new Dot();
                    Dot enemy = new Dot();
                    Kug kug = new Kug();

                    //Set Startpos
                    kug.startPos((SCREEN_WIDTH / 2), (SCREEN_HEIGHT / 2));
                    player.startPos(0, 100);
                    enemy.startPos(SCREEN_WIDTH - enemy.dotW, 100);

                    //While application is running
                    while (!quit)
                    {
                        //Handle events on queue
                        while (SDL.SDL_PollEvent(out e) != 0)
                        {
                            //User requests quit via closing the window or pressing esc
                            if (e.type == SDL.SDL_EventType.SDL_QUIT || e.key.keysym.sym == SDL.SDL_Keycode.SDLK_ESCAPE)
                            {
                                quit = true;
                            }
                            //Handle input for the player
                            player.handleEvent(e);
                        }

                        collCheck(player, kug);
                        collCheck(enemy, kug);

                        //Move the player
                        player.move();
                        kug.move();
                        enemy.moveEnemy();

                        //Clear screen
                        SDL.SDL_SetRenderDrawColor(gRenderer, 0xFF, 0xFF, 0xFF, 0xFF);
                        SDL.SDL_RenderClear(gRenderer);

                        /*
                        //Begrenzung
                        var blackline = new SDL.SDL_Rect { x = 0, y = 100, w = SCREEN_WIDTH, h = 2 };
                        SDL.SDL_SetRenderDrawColor(gRenderer, 0x00, 0x00, 0x00, 0xFF);
                        SDL.SDL_RenderFillRect(gRenderer, ref blackline);
                        */

                        var dotline = new SDL.SDL_Rect { x = SCREEN_WIDTH / 2, y = 0, w = 2, h = 8 };
                        SDL.SDL_SetRenderDrawColor(gRenderer, 155, 155, 155, 255);
                        for (dotline.y = 0; dotline.y < SCREEN_HEIGHT; dotline.y += 10)
                        {
                            SDL.SDL_RenderFillRect(gRenderer, ref dotline);
                        }

                        //Render current frame TEXT
                        _TextTexture.render((SCREEN_WIDTH - _TextTexture.GetWidth()) / 2, 0);

                        //Render objects
                        player.render();
                        kug.render();
                        enemy.render();

                        changeText(Convert.ToString(p1counter + " : " + p2counter));

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