using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using SDL2;
using JumperGame.level;
using JumperGame.ui;

/*
TODO - Entity aufsplitten interactable, hit usw.

 */
namespace JumperGame
{
    class Program
    {
        //Game state
        public static GameState CurrentState = GameState.MAIN_MENU;
        public static Menu VisibleMenu { get; set; } 
        
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

        public static ArrayList entityList = new ArrayList();
        public static ArrayList bgList = new ArrayList();

        public static bool quit = false;
        public static bool reset = false;
        public static bool debugMode = false;

        private static bool Init()
        {
            //Initialization flag
            bool success = true;

            //Initialize SDL
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO | SDL.SDL_INIT_AUDIO) < 0)
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
                gWindow = SDL.SDL_CreateWindow("JumperGame", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED,
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


        /**
         * Free media and shut down SDL
         */
        public static void Close()
        {
            SDL_ttf.TTF_CloseFont(Font);
            Font = IntPtr.Zero;
            
            SoundHandler.Close();
            
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

        public static void changeWindowSize()
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
            
            // Update the positions of the menu items
            VisibleMenu?.UpdateMenuItemPositions();
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
                FileHandler fileHandler = new FileHandler();
                success = fileHandler.getStatus();

                SoundHandler.LoadMedia();
                ScoreUI.LoadHighscore();

                //src.EventHandler eventHandler = new src.EventHandler();

                if (success == false)
                {
                    Console.WriteLine("Failed to load media!");
                }
                else
                {
                    LevelManager.LoadLevels();
                    // Add the menu items to the menu
                    VisibleMenu = new MainMenu(gRenderer);
                    
                    while (!quit)
                    {
                        double previous = 0;

                        ////////////////////////////////////// TEST AREA
                        ///
                        reset = false;
                        bgList.Clear();
                        entityList.Clear();

                        List<String> list = new List<String>();
                        list.Add("NebulaBlue");
                        list.Add("Stars");
                        BackgroundObject bgo = new BackgroundObject(fileHandler.getTextureList(list));

                        BackgroundObject bgo2 = bgo.copy(SCREEN_WIDTH * 2, 0, 1);
                        BackgroundObject bgoStars = bgo.copy(0, 1, 2);
                        BackgroundObject bgoStars2 = bgo.copy(SCREEN_WIDTH * 2, 1, 2);
                        bgList.Add(bgo);
                        bgList.Add(bgo2);
                        bgList.Add(bgoStars);
                        bgList.Add(bgoStars2);  //TODO in BGO reinstopfen

                        list.Clear();

                        list.Add("PlayerShip");
                        list.Add("Bullet_move");
                        list.Add("Player_shield");
                        list.Add("Player_dmg1");
                        list.Add("Player_dmg2");
                        list.Add("Player_dmg3");
                        Player arno = new Player(fileHandler.getTextureList(list));
                        list.Clear();
                                                        
                        ////////////////////////////////////// TEST AREA
                        ///
                        entityList.Add(arno);


                        //While application is running
                        while (!reset)
                        {
                            SDL.SDL_RenderClear(gRenderer);

                            double current = timer.getTicks();
                            double elapsed = current - previous;
                            previous = current;
                            if (elapsed < 1.0)
                            {
                                elapsed = 5;
                            }
                            elapsed = 8;
                            switch (CurrentState)
                            {
                                case GameState.MAIN_MENU:
                                case GameState.LEVEL_SELECT:
                                case GameState.SETTINGS:
                                case GameState.INSTRUCTIONS:
                                case GameState.PAUSED:
                                case GameState.GAME_OVER:
                                case GameState.WIN:
                                    
                                    foreach (BackgroundObject backgroundObject in bgList)
                                    {
                                        backgroundObject.checkOutOfBounds();
                                        backgroundObject.update(elapsed);
                                    }
                                    
                                    VisibleMenu?.Render(gRenderer);
                                    break;
                            
                                case GameState.IN_GAME:
                                    VisibleMenu = null;
                                    
                                    entityList = CollisionHandler.checkCollision(entityList);

                                    foreach (BackgroundObject backgroundObject in bgList)
                                    {
                                        backgroundObject.checkOutOfBounds();
                                        backgroundObject.update(elapsed);
                                    }
                                    
                                    ScoreUI.Update();
                                    ScoreUI.DisplayHighscore(gRenderer);
                                    
                                    EnemyAmountUI.DisplayEnemyCount(gRenderer);

                                    
                                    LifeUI.DisplayLives(gRenderer);

                                    foreach (Entity enti in entityList)
                                    {
                                        enti.update(elapsed);
                                    }
                                    
                                    LevelManager.RunCurrentLevelLogic(elapsed, fileHandler, entityList);
                                    
                                    // eventHandler.updateList(entityList);
                                    // eventHandler.timedEvent(elapsed, fileHandler);

                                    break;
                            }
                            
                            ////////////////////////////////////// TEST AREA
                            (double x, double y, int direction, string command) = InputHandler.handleUserInput(elapsed);
                            if (command == "shoot")
                            {
                                entityList.Add(arno.shoot(x, y, direction, elapsed));
                            }
                            else if (command == "move")
                            {
                                arno.vecX = x ;
                                arno.vecY = y ;
                                arno.angle = direction;
                            }


                            ////////////////////////////////////// TEST AREA
                            //Update screen
                            SDL.SDL_RenderPresent(gRenderer);
                        }
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