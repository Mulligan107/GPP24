using System;
using System.Collections;
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
   x There needs to be a game-over state and the option to play again.

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
        public static int MAX_SCREEN_WIDTH;
        public static int MAX_SCREEN_HEIGHT;
        public static int ALT_SCREEN_WIDTH = 640;
        public static int ALT_SCREEN_HEIGHT = 480;
        public static int SCREEN_WIDTH;
        public static int SCREEN_HEIGHT;

        //Screen size mode
        public static bool isFullScreen = true;

        public static int p1counter = 0;
        public static int p2counter = 0;

        //Liste der EntityPosis
        public static ArrayList pongEntityList = new ArrayList(); //ToDO variable machen

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
        public static LTexture alertTextTexture = new LTexture();

        //Game ticker
        public static LTimer timer = new LTimer();

        public static Random gRandom = new Random();

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

                if (!alertTextTexture.loadFromRenderedText("PAUSE", textColor))
                {
                    Console.WriteLine("Failed to render alert texture!");
                    success = false;
                }
            }

            return success;
        }

        public static void gameReset()
        {
            p1counter = 0;
            p2counter = 0;
            timer.stop();
            timer.start();
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
            alertTextTexture.free();

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
        static void collCheck(Paddle paddle, Ball ball)
        {
            //Versändlichere kurze Namen 
            int kugL = ball.mPosX;
            int kugR = ball.mPosX + ball.dotW;
            int kugOb = ball.mPosY;
            int kugUn = ball.mPosY + ball.dotH;

            int playL = paddle.mPosX;
            int playR = paddle.mPosX + paddle.dotW;
            int playOb = paddle.mPosY;
            int playUn = paddle.mPosY + paddle.dotH;


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
                //Paddle ist 200 Lang
                int aufschlagsPunkt =  ball.mPosY - paddle.mPosY;
                
                // Vectoren sind geeyeballed
                if (aufschlagsPunkt <= 40)
                {
                    ball.vectorX = 4;
                    ball.vectorY = -16;
                }
                if (aufschlagsPunkt >= 40 && aufschlagsPunkt <= 80)
                {
                    ball.vectorX = 8;
                    ball.vectorY = -4;
                }
                if (aufschlagsPunkt >= 80 && aufschlagsPunkt <= 120)
                {
                    ball.changeDir(0);
                }
                if (aufschlagsPunkt >= 120 && aufschlagsPunkt <= 160)
                {
                    ball.vectorX = 8;
                    ball.vectorY = 4;
                }
                if (aufschlagsPunkt > 160)
                {
                    ball.vectorX = 4;
                    ball.vectorY = 16;
                }




                
            }

            //Bedingung Links
            if (kugL < playL && kugUn > playOb && kugOb < playUn && kugR > playL)
            {
                ball.changeDir(0);
            }
        }

        static void changeText(LTexture Ltex, String text)
        {
            Ltex.loadFromRenderedText(text, new SDL.SDL_Color());
        }

        static void createButton()
        {
            /*
             * int posX, int posY, string buttonText
             *
            var textColor = new SDL.SDL_Color();
            LTexture buttonTexture = new LTexture();
            buttonTexture.loadFromRenderedText(buttonText, textColor);
            int scaleValue = 1; //Skalierung für veränderung des Fensters
            var blackline = new SDL.SDL_Rect { x = posX, y = posY, w = 200 * scaleValue, h = 50 };
            SDL.SDL_SetRenderDrawColor(gRenderer, 0x00, 0x00, 0x00, 0xFF);
            SDL.SDL_RenderFillRect(gRenderer, ref blackline);
            */
            var pauseBackground = new SDL.SDL_Rect { x = 0, y = 0, w = SCREEN_WIDTH, h = SCREEN_HEIGHT };
            SDL.SDL_SetRenderDrawColor(gRenderer, 0xF0, 0xF0, 0xF0, 0x10);
            SDL.SDL_RenderFillRect(gRenderer, ref pauseBackground);
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
                    bool paused = true;
                    bool gameover = false;
                    bool gamestart = true;

                    //Event handler
                    SDL.SDL_Event e;

                    gameReset();

                    //The player that will be moving around on the screen
                    Paddle player = new Paddle();
                    Paddle enemy = new Paddle();

                    /*
                    for (int i = 0; i < 3; i++)
                    {
                        pongEntityList.Add(balls);
                    }
                    */

                    pongEntityList.Add(new Ball());


                    player.startPos(0, 100);
                    enemy.startPos(SCREEN_WIDTH - 20, 100);
                    


                    //Set 


                    //While application is running
                    while (!quit)
                    {
                        //Handle events on queue
                        while (SDL.SDL_PollEvent(out e) != 0)
                        {
                            //Handle input for the player
                            player.handleEvent(e);

                            if (gamestart)
                            {
                                paused = true;
                                changeText(alertTextTexture,
                                    "WELCOME TO PONG - PRESS ANY TO START  F TO TOGGLE FULLSCREEN  P TO PAUSE");
                                alertTextTexture.render((SCREEN_WIDTH / 2) - (alertTextTexture.getWidth() / 2),
                                    (SCREEN_HEIGHT / 2));
                                if (e.type == SDL.SDL_EventType.SDL_KEYDOWN)
                                {
                                    paused = false;
                                    gamestart = false;
                                    changeText(alertTextTexture, "PAUSE");
                                }
                            }

                            //User requests quit via closing the window or pressing esc
                            if (e.type == SDL.SDL_EventType.SDL_QUIT || e.key.keysym.sym == SDL.SDL_Keycode.SDLK_ESCAPE)
                            {
                                quit = true;
                            }

                            //Switch screen size mode if 'F' key was pressed
                            if (e.type == SDL.SDL_EventType.SDL_KEYDOWN) //ToDo könnte ein switch case sein
                            {
                                if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_f)
                                {
                                    // Calculate relative positions
                                    /*
                                    float playerRelativePosX = (float)player.mPosX / SCREEN_WIDTH;
                                    float playerRelativePosY = (float)player.mPosY / SCREEN_HEIGHT;
                                    float enemyRelativePosX = (float)enemy.mPosX / SCREEN_WIDTH;
                                    float enemyRelativePosY = (float)enemy.mPosY / SCREEN_HEIGHT;
                                    float kugRelativePosX = (float)ball.mPosX / SCREEN_WIDTH;
                                    float kugRelativePosY = (float)ball.mPosY / SCREEN_HEIGHT;
                                    */

                                    // Change screen size
                                   

                                    // Calculate relative positions
                                    float playerRelativePosX = (float)player.mPosX / SCREEN_WIDTH;
                                    float playerRelativePosY = (float)player.mPosY / SCREEN_HEIGHT;
                                    float enemyRelativePosX = (float)enemy.mPosX / SCREEN_WIDTH;
                                    float enemyRelativePosY = (float)enemy.mPosY / SCREEN_HEIGHT;

                                    foreach (Ball ballsy in pongEntityList)
                                    {
                                        ballsy.kugRelativePosX = (float)ballsy.mPosX / SCREEN_WIDTH;
                                        ballsy.kugRelativePosY = (float)ballsy.mPosY / SCREEN_HEIGHT;
                                    }
                                        

                                    // Change screen size
                                    isFullScreen = !isFullScreen;
                                    if (isFullScreen)
                                    {
                                        SCREEN_WIDTH = MAX_SCREEN_WIDTH;
                                        SCREEN_HEIGHT = MAX_SCREEN_HEIGHT;
                                    }
                                    else
                                    {
                                        SCREEN_WIDTH = ALT_SCREEN_WIDTH;
                                        SCREEN_HEIGHT = ALT_SCREEN_HEIGHT;
                                    }

                                    SDL.SDL_SetWindowSize(gWindow, SCREEN_WIDTH, SCREEN_HEIGHT);

                                    // Update positions based on relative positions
                                    player.mPosX = (int)(playerRelativePosX * SCREEN_WIDTH);
                                    player.mPosY = (int)(playerRelativePosY * SCREEN_HEIGHT);
                                    enemy.mPosX = (int)(enemyRelativePosX * SCREEN_WIDTH);
                                    enemy.mPosY = (int)(enemyRelativePosY * SCREEN_HEIGHT);
                                        
                                    foreach (Ball ballsy in pongEntityList)
                                    {
                                        ballsy.mPosX = (int)(ballsy.kugRelativePosX * SCREEN_WIDTH);
                                        ballsy.mPosY = (int)(ballsy.kugRelativePosY * SCREEN_HEIGHT);
                                    }
                                }
                                if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_r)
                                {
                                    player.startPos(0, 100);
                                    enemy.startPos(SCREEN_WIDTH - 20, 100);

                                    pongEntityList.Clear();

                                    pongEntityList.Add(new Ball());

                                    gameReset();
                                    gDotTexture.setAlpha(255); //ToDo , ändern Quickfix wegen transparanz
                                    gameover = false;
                                    paused = false;
                                    changeText(alertTextTexture, "PAUSE");
                                }


                                if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_p)
                                {
                                        switch (paused)
                                        {
                                            case false:
                                                paused = true;
                                                gBarTexture.setAlpha(180);
                                                gDotTexture.setAlpha(180);
                                                _TextTexture.setAlpha(180);
                                                break;
                                            case true:
                                                paused = false;
                                                gBarTexture.setAlpha(0xFF);
                                                gDotTexture.setAlpha(0xFF);
                                                _TextTexture.setAlpha(0xFF);
                                                break;
                                        }
                                }

                                // Tasten 1,2,3 für ändern der Farbe
                                if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_1)
                                {
                                    gBarTexture.setColor(255, 0, 0);
                                }
                                if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_2)
                                {
                                    gBarTexture.setColor(0, 255, 0);
                                }
                                if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_3)
                                {
                                    gBarTexture.setColor(0, 0, 255);
                                }
                            }
                        }
                        
                        // < 2 statt == 0 weil ticks manchmal geskippt werden
                        // alle 500 ticks einen neuen Ball hinzufügen bis 3 existiteren
                        if ((timer.getTicks() % 500 < 2) && (pongEntityList.Count < 3))
                        {
                            pongEntityList.Add(new Ball());
                        }
                        

                        if (p1counter >= 3)
                        {
                            paused = true;
                            gameover = true;
                        }

                        if (!paused)
                        {
                            
                            foreach (Ball ballsy in pongEntityList)
                            {
                                collCheck(player, ballsy);
                                collCheck(enemy, ballsy);
                            }

                            //Move the player
                            player.move();
                            enemy.moveEnemy();

                            foreach (Ball ballsy in pongEntityList)
                            {
                                ballsy.move();
                            }
                            
                        }


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

                        if (paused)
                        {
                            createButton();
                            if (gameover)
                            {
                                changeText(alertTextTexture, "GAME OVER - PRESS R TO RETRY");
                                gDotTexture.setAlpha(0); //ToDo , ändern Quickfix wegen transparanz
                            }

                            alertTextTexture.render((SCREEN_WIDTH / 2) - (alertTextTexture.getWidth() / 2),
                                (SCREEN_HEIGHT / 2));
                        }

                        //Render objects
                        player.render();
                        enemy.render();

                        foreach (Ball ballsy in pongEntityList)
                        {
                            ballsy.render();
                        }

                        changeText(_TextTexture,Convert.ToString(p1counter + " : " + p2counter));
                        //Render current frame TEXT
                        _TextTexture.render(((SCREEN_WIDTH / 2) - (_TextTexture.GetWidth() / 2)), 0);

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