using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
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
X    Add interesting gameplay twists.
-    Add multiple levels.
+    Add additional screens. (e.g. Intro, Settings, Credits...)
-    Add support for a persistent Highscore.
    Add collectible Power-Ups.
X    Allow the game to be played in fullscreen mode.

 */


namespace PongGame
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

        public static double skalierung;

        public static int pannelH = 200;

        //Screen size mode
        public static bool isFullScreen = true;

        public static int p1counter = 0;
        public static int p2counter = 0;
        public static int speedLevel = 0;

        //Liste der EntityPosis
        public static ArrayList ballList = new ArrayList();
        public static ArrayList ghostList = new ArrayList();

        //The music that will be played
        private static IntPtr _Music = IntPtr.Zero;
        
        //The sound effects that will be used
        public static IntPtr _Boing = IntPtr.Zero;

        //The window we'll be rendering to
        public static IntPtr gWindow = IntPtr.Zero;

        //The surface contained by the window
        public static IntPtr gRenderer = IntPtr.Zero;

        //Globally used font
        public static IntPtr Font = IntPtr.Zero;

        public static IntPtr ghostSurface = IntPtr.Zero;

        //Scene textures

        public static LTexture gBarTexture = new LTexture();
        public static LTexture ghostTexture = new LTexture();
        public static LTexture ballTexture = new LTexture();

        public static LTexture knightTexture = new LTexture();
        public static ArrayList knightAnimList = new ArrayList();
        public static string[] knightFiles = { "imgs/_Attack2.png", "imgs/_DeathNoMovement.png", "imgs/_Hit.png", "imgs/_Idle.png" };
        public static Knight playerKnight = new Knight(knightTexture);

        public static LTexture backgroundTexture = new LTexture();
        public static LTexture pannelBackgroundTexture = new LTexture();


        //Rendered texture
        private static readonly LTexture _ScoreTextTexture = new LTexture();
        private static readonly LTexture _HighScoreTextTexture = new LTexture();
        public static LTexture alertTextTexture1 = new LTexture();
        public static LTexture alertTextTexture2 = new LTexture();

        //Game ticker
        public static LTimer timer = new LTimer();

        public static Random gRandom = new Random();

        //Main loop flag
        static bool quit = false;
        static bool paused = true;
        static bool gameover = false;
        static bool gamestart = true;

        //Event handler
        static SDL.SDL_Event e;

        //The player that will be moving around on the screen
        public static Paddle player = new Paddle(gBarTexture);
        public static Paddle enemy = new Paddle(gBarTexture);

        static HighScoreManager highScoreManager = new HighScoreManager();
        
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
            
            //Load music
            _Music = SDL_mixer.Mix_LoadMUS("sounds/8bit_Forest.wav");
            if (_Music == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load! {0}", SDL.SDL_GetError());
                success = false;
            }
            
            if (!File.Exists("sounds/8bit_Boing.wav"))
            {
                Console.WriteLine("File does not exist");
            }
            
            //Load sound effects
            _Boing = SDL_mixer.Mix_LoadWAV("sounds/8bit_Boing.wav");
            if (_Boing == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load! {0}", SDL.SDL_GetError());
                success = false;
            }
            
            if (!gBarTexture.loadFromFile("imgs/padleSpriteSheet.png"))
            {
                Console.WriteLine("Failed to load!");
                success = false;
            }

            if (!ghostTexture.loadFromFile("imgs/ghostSpriteSheet0.png"))
            {
                Console.WriteLine("Failed to load!");
                success = false;
            }

            if (!ballTexture.loadFromFile("imgs/dotSpriteSheet.png"))
            {
                Console.WriteLine("Failed to load!");
                success = false;
            }

            if (!backgroundTexture.loadFromFile("imgs/Gray.png"))
            {
                Console.WriteLine("Failed to load!");
                success = false;
            }

            if (!pannelBackgroundTexture.loadFromFile("imgs/Green.png"))
            {
                Console.WriteLine("Failed to load!");
                success = false;
            }

            for (int i = 0; i < 4; i++)
            {
                LTexture knight = new LTexture();
                if (!knight.loadFromFile(knightFiles[i]))
                {
                    Console.WriteLine("Failed to load!");
                    success = false;
                }
                knightAnimList.Add(knight); // Attack , Death , Hit , Idle
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
                if (!_ScoreTextTexture.loadFromRenderedText("The quick brown fox jumps over the lazy dog", textColor))
                {
                    Console.WriteLine("Failed to render text texture!");
                    success = false;
                }
                
                //Render text
                var highScoreTextColor = new SDL.SDL_Color();
                if (!_HighScoreTextTexture.loadFromRenderedText("The quick brown fox jumps over the lazy dog", highScoreTextColor))
                {
                    Console.WriteLine("Failed to render text texture!");
                    success = false;
                }

                if (!alertTextTexture1.loadFromRenderedText("PAUSE", textColor))
                {
                    Console.WriteLine("Failed to render alert texture!");
                    success = false;
                }
            }

            return success;
        }

        public static void playMusic()
        {
            //If there is no music playing
            if (SDL_mixer.Mix_PlayingMusic() == 0)
            {
                //Play the music
                SDL_mixer.Mix_PlayMusic(_Music, -1);
            }
            //If music is being played
            else
            {
                //If the music is paused
                if (SDL_mixer.Mix_PausedMusic() == 1)
                {
                    //Resume the music
                    SDL_mixer.Mix_ResumeMusic();
                }
                //If the music is playing
                else
                {
                    //Pause the music
                    SDL_mixer.Mix_PauseMusic();
                }
            }
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

            foreach (Ball ballsy in ballList)
            {
                ballsy.gDotTexture.free();
            }

            //Free the sound effects
            SDL_mixer.Mix_FreeChunk(_Boing);
            _Boing = IntPtr.Zero;
            
            //Free the music
            SDL_mixer.Mix_FreeMusic(_Music);
            _Music = IntPtr.Zero;
            
            gBarTexture.free();

            //Free loaded images
            _ScoreTextTexture.free();
            _HighScoreTextTexture.free();
            alertTextTexture1.free();

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
            double kugL = ball.mPosX;
            double kugR = ball.mPosX + ball.dotW;
            double kugOb = ball.mPosY;
            double kugUn = ball.mPosY + ball.dotH;

            double playL = paddle.mPosX;
            double playR = paddle.mPosX + paddle.dotW;
            double playOb = paddle.mPosY;
            double playUn = paddle.mPosY + paddle.dotH;


            //Bedingung Rechts
            if (kugL < playR && kugUn > playOb && kugOb < playUn && kugR > playR)
            {
                if (player.color == ball.letzteFarbe) //3 zum testen
                {
                    SDL_mixer.Mix_PlayChannel(-1, _Boing, 0);
                    //Paddle ist 200 Lang
                    double aufschlagsPunkt = ball.mPosY - paddle.mPosY;

                    double vecX = 0.0;
                    double vecY = -5.0;

                    for (int i = 0; i <= aufschlagsPunkt; i++)
                    {
                        if (i > 100)
                        {
                            vecX -= 0.05;
                        }
                        else
                        {
                            vecX += 0.05;
                        }

                        vecY += 0.05;
                        /*
                         * aufschlagspunkt = 0 - 200
                         *
                         * d0 = x1 , y-5
                         * d100 = x5, y0
                         * d200 = x1, y5
                         *
                         */
                    }

                    ball.mPosX += 5;
                    ball.vectorX = vecX; 
                    ball.vectorY = vecY;

                    ball.changeColor();
                }
            }

            //Bedingung Links
            if (kugL < playL && kugUn > playOb && kugOb < playUn && kugR > playL)
            {
                if (player.color == ball.letzteFarbe || paddle == enemy) //3 zum testen
                {
                    SDL_mixer.Mix_PlayChannel(-1, _Boing, 0);
                    ball.changeDir(0);
                    ball.changeColor();
                    ball.mPosX -= 5;
                }
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


        static void handleUserInput()
        {
            while (SDL.SDL_PollEvent(out e) != 0)
            {
                //Handle input for the player
                player.handleEvent(e);

                if (gamestart)
                {
                    paused = true;
                    changeText(alertTextTexture1,
                        "WELCOME TO PONG - PRESS ANY TO START  F TO TOGGLE FULLSCREEN  P TO PAUSE");
                    alertTextTexture1.render((SCREEN_WIDTH / 2) - (alertTextTexture1.getWidth() / 2),
                        (SCREEN_HEIGHT / 2));

                    foreach (Ball ballsy in ballList)
                    {
                        ballsy.gDotTexture.setAlpha(0); //ToDo , ändern Quickfix wegen transparanz
                    }

                    if (e.type == SDL.SDL_EventType.SDL_KEYDOWN)
                    {
                        paused = false;
                        gamestart = false;
                        changeText(alertTextTexture1, "PAUSE");
                        foreach (Ball ballsy in ballList)
                        {
                            ballsy.gDotTexture.setAlpha(255); //ToDo , ändern Quickfix wegen transparanz
                        }
                    }
                }

                //User requests quit via closing the window or pressing esc
                if (e.type == SDL.SDL_EventType.SDL_QUIT || e.key.keysym.sym == SDL.SDL_Keycode.SDLK_ESCAPE)
                {
                    highScoreManager.CheckScore(p1counter);
                    highScoreManager.SaveHighScore();
                    quit = true;
                }

                //Switch screen size mode if 'F' key was pressed
                if (e.type == SDL.SDL_EventType.SDL_KEYDOWN) //ToDo könnte ein switch case sein
                {
                    // Change screen size
                    if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_f)
                    {
                        // Calculate relative positions
                        float playerRelativePosX = (float)player.mPosX / SCREEN_WIDTH;
                        float playerRelativePosY = (float)player.mPosY / SCREEN_HEIGHT;
                        float enemyRelativePosX = (float)enemy.mPosX / SCREEN_WIDTH;
                        float enemyRelativePosY = (float)enemy.mPosY / SCREEN_HEIGHT;
                        
                        foreach (Ball ballsy in ballList)
                        {
                            ballsy.kugRelativePosX = (float)ballsy.mPosX / SCREEN_WIDTH;
                            ballsy.kugRelativePosY = (float)ballsy.mPosY / SCREEN_HEIGHT;
                        }

                        foreach (Ghost ghostys in ghostList)
                        {
                            ghostys.ghostRelativePosX = (float)ghostys.posX / SCREEN_WIDTH;
                            ghostys.ghostRelativePosY = (float)ghostys.posY / SCREEN_WIDTH;
                        }
                        
                        // Change screen size
                        isFullScreen = !isFullScreen;
                        if (isFullScreen)
                        {
                            SCREEN_WIDTH = MAX_SCREEN_WIDTH;
                            SCREEN_HEIGHT = MAX_SCREEN_HEIGHT;

                            // Set the window to fullscreen
                            SDL.SDL_SetWindowFullscreen(gWindow, 
                                (int)SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP);
                        
                            // Update positions based on relative positions
                            player.mPosX = (int)(playerRelativePosX * SCREEN_WIDTH);
                            player.mPosY = (int)(playerRelativePosY * SCREEN_HEIGHT);
                            enemy.mPosX = (int)(enemyRelativePosX * SCREEN_WIDTH);
                            enemy.mPosY = (int)(enemyRelativePosY * SCREEN_HEIGHT);

                            foreach (Ball ballsy in ballList)
                            {
                                ballsy.mPosX = (int)(ballsy.kugRelativePosX * SCREEN_WIDTH);
                                ballsy.mPosY = (int)(ballsy.kugRelativePosY * SCREEN_HEIGHT);
                            }

                            foreach (Ghost ghostys in ghostList)
                            {
                                ghostys.posX = (float)ghostys.ghostRelativePosX * SCREEN_WIDTH;
                                ghostys.posY = (float)ghostys.ghostRelativePosY * SCREEN_WIDTH;
                            }


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
                            
                            /*
                            This is so the paddles don't bug into the screen edges. Depending on whether the
                            paddle is under or above the middle of the screen the paddle gets an "extra position boost"
                            */
                            
                            if (playerRelativePosY * SCREEN_HEIGHT < SCREEN_HEIGHT / 2)
                            {
                                player.mPosY = (int)(playerRelativePosY * SCREEN_HEIGHT) + 100;
                            }
                            else
                            {
                                player.mPosY = (int)(playerRelativePosY * SCREEN_HEIGHT) - 100;
                            }

                            if (enemyRelativePosY * SCREEN_HEIGHT < SCREEN_HEIGHT / 2)
                            {
                                enemy.mPosY = (int)(enemyRelativePosY * SCREEN_HEIGHT) + 100;
                            }
                            else
                            {
                                enemy.mPosY = (int)(enemyRelativePosY * SCREEN_HEIGHT) - 100;
                            }
                        
                            // Update positions based on relative positions
                            player.mPosX = (int)(playerRelativePosX * SCREEN_WIDTH);
                            enemy.mPosX = (int)(enemyRelativePosX * SCREEN_WIDTH);
                        }

                        SDL.SDL_SetWindowSize(gWindow, SCREEN_WIDTH, SCREEN_HEIGHT);

                        foreach (Ball ballsy in ballList)
                        {
                            ballsy.mPosX = (int)(ballsy.kugRelativePosX * SCREEN_WIDTH);
                            ballsy.mPosY = (int)(ballsy.kugRelativePosY * SCREEN_HEIGHT);
                        }

                        foreach (Ghost ghostys in ghostList)
                        {
                            ghostys.posX = (float)ghostys.ghostRelativePosX * SCREEN_WIDTH;
                            ghostys.posY = (float)ghostys.ghostRelativePosY * SCREEN_WIDTH;
                        }

                        pannelH = calcPannelH();

                    }

                    if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_m)
                    {
                        playMusic();
                    }
                    
                    if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_r)
                    {
                        highScoreManager.CheckScore(p1counter);
                        highScoreManager.SaveHighScore();
                        player.startPos(20, pannelH);
                        enemy.startPos(SCREEN_WIDTH - 40, pannelH);

                        ballList.Clear();
                        //ballList.Add(new Ball());

                        foreach (Ball ballsy in ballList)
                        {
                            ballsy.gDotTexture.setAlpha(0xFF); //ToDo , ändern Quickfix wegen transparanz
                        }

                        ghostList.Clear();
                        ghostList.Add(new Ghost(ghostTexture));

                        gameReset();


                        gameover = false;
                        paused = false;
                        gamestart = false;
                        changeText(alertTextTexture1, "PAUSE");
                    }


                    if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_p)
                    {
                        switch (paused)
                        {
                            case false:
                                paused = true;
                                playMusic();
                                gBarTexture.setAlpha(180);
                                _ScoreTextTexture.setAlpha(180);
                                ghostTexture.setAlpha(180);
                                timer.pause();
                                foreach (Ball ballsy in ballList)
                                {
                                    ballsy.gDotTexture.setAlpha(180);
                                }


                                break;

                            case true:
                                paused = false;
                                playMusic();
                                gBarTexture.setAlpha(0xFF);
                                _ScoreTextTexture.setAlpha(0xFF);
                                ghostTexture.setAlpha(0xFF);
                                timer.unpause();
                                foreach (Ball ballsy in ballList)
                                {
                                    ballsy.gDotTexture.setAlpha(0xFF);
                                }

                                break;
                        }
                    }

                    // Tasten 1,2,3 für ändern der Farbe
                    if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_1)
                    {
                        player.color = 0;
                    }

                    if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_2)
                    {
                        player.color = 1;
                    }

                    if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_3)
                    {
                        player.color = 2;
                    }

                    if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_t)
                    {
                        foreach (Ball ballsy in ballList)
                        {
                            if (ballsy.speed <= 2.0)
                            {
                                ballsy.speed += 0.5;
                            }
                            else
                            {
                                ballsy.speed = 0.5;
                            }
                        }
                    }
                }
            }
        }

        static void renderObjects()
        {
            //Render objects
            player.render();
            enemy.render();

            foreach (Ball ballsy in ballList)
            {
                ballsy.render();
            }

            foreach (Ghost ghosts in ghostList)
            {
                ghosts.render();
            }

            playerKnight.render();
            

        }

        static void drawBackground()
        {
            pannelH = calcPannelH();
            byte alpha;
            if (paused)
            {
                alpha = 180;
            }
            else
            {
                alpha = 0xFF;
            }
            //Clear screen
            SDL.SDL_SetRenderDrawColor(gRenderer, 0xFF, 0xFF, 0xFF, alpha);
            SDL.SDL_RenderClear(gRenderer);
            

            for (int j = 0; j < SCREEN_HEIGHT; j += backgroundTexture.getHeight())
            {
                for (int i = 0; i < SCREEN_WIDTH; i += backgroundTexture.getWidth())
                {
                    backgroundTexture.render(i, j);
                    backgroundTexture.setAlpha(alpha);
                }
            }
            int counterJ = 0;
            int counterI = 0;

            for (int j = 10; j < SCREEN_HEIGHT / 5; j += pannelBackgroundTexture.getHeight())
            {
                counterI = 0;
                for (int i = 11; i < SCREEN_WIDTH - 15; i += pannelBackgroundTexture.getWidth())
                {
                    pannelBackgroundTexture.render(i, j);
                    pannelBackgroundTexture.setAlpha(alpha);
                    counterI++;
                }
                counterJ++;
            }

            

            //Render black outlined pannel  i = wanddicke
            for (int i = 0; i < 5; i++)
            {
                var outlineRect = new SDL.SDL_Rect
                { x = 5 + i, y = 5 + i, w = pannelBackgroundTexture.getWidth() * counterI, h = pannelBackgroundTexture.getHeight() * counterJ};
                SDL.SDL_SetRenderDrawColor(gRenderer, 0x00, 0x00, 0x00, alpha);
                SDL.SDL_RenderDrawRect(gRenderer, ref outlineRect);
            }

            

            /*
            //Begrenzung
            var blackline = new SDL.SDL_Rect { x = 0, y = 100, w = SCREEN_WIDTH, h = 2 };
            SDL.SDL_SetRenderDrawColor(gRenderer, 0x00, 0x00, 0x00, 0xFF);
            SDL.SDL_RenderFillRect(gRenderer, ref blackline);
            */

            var dotline = new SDL.SDL_Rect { x = SCREEN_WIDTH / 2, y = pannelH + 5, w = 2, h = 8 };
            SDL.SDL_SetRenderDrawColor(gRenderer, 155, 155, 155, alpha);
            for (dotline.y = pannelH + 5; dotline.y < SCREEN_HEIGHT; dotline.y += 10)
            {
                SDL.SDL_RenderFillRect(gRenderer, ref dotline);
            }
            
            //Current Score Text
            changeText(_ScoreTextTexture, Convert.ToString(p1counter + " : " + p2counter));
            //Render current frame TEXT
            _ScoreTextTexture.render(((SCREEN_WIDTH / 2) - (_ScoreTextTexture.GetWidth() / 2)), pannelH + 15);
            
            //Highscore Text
            changeText(_HighScoreTextTexture, Convert.ToString("Highscore : " + highScoreManager.ReadHighScore()));
            _HighScoreTextTexture.render(((SCREEN_WIDTH / 2) - (_ScoreTextTexture.GetWidth() / 2)) - 35, pannelH + 35);

            for (int i = 0; i < 5; i++)
            {
                SDL.SDL_Rect pannelRect = new SDL.SDL_Rect { x = 10 + 200 + i, y = 10 + i, w = (SCREEN_WIDTH/10) - i * 2, h = pannelH - 10 - i * 2};
                SDL.SDL_SetRenderDrawColor(Program.gRenderer, 0xFF, 0x00, 0x00, 0xFF);
                SDL.SDL_RenderDrawRect(Program.gRenderer, ref pannelRect); //TestFeld
            }

            
             

        }

        static void updateGame(double deltaTime)
        {
            
            //Gameover abfrage
            if (p2counter >= 3)
            {
                paused = true;
                gameover = true;
                highScoreManager.CheckScore(p1counter);
                highScoreManager.SaveHighScore();
            }

            if (!paused)
            {
                // < 2 statt == 0 weil ticks manchmal geskippt werden
                // alle 3000 ticks einen neuen Ball hinzufügen bis 3 existiteren
                if (((timer.getTicks() * (deltaTime / 10)) % 3000 < 5) && (ballList.Count < 3))
                {
                    ballList.Add(new Ball(ballTexture));
                }

                foreach (Ball ballsy in ballList)
                {
                    collCheck(player, ballsy);
                    collCheck(enemy, ballsy);
                    ballsy.move(deltaTime);
                }

                foreach (Ghost ghosts in ghostList)
                {
                    ghosts.frameTicker += (deltaTime / 1000);
                    ghosts.move(deltaTime / 10);
                }

                playerKnight.frameTicker += (deltaTime / 500);


                //Move the player
                player.move(deltaTime);
                enemy.moveEnemy(deltaTime);
            }
        }

        public static int calcPannelH()
        {
            int counterJ = 0;
            int counterI = 0;

            for (int j = 10; j < SCREEN_HEIGHT/5; j += pannelBackgroundTexture.getHeight())
            {
                counterI = 0;
                for (int i = 11; i < SCREEN_WIDTH - 15; i += pannelBackgroundTexture.getWidth())
                {
                    counterI++;
                }
                counterJ++;
            }

            return (pannelBackgroundTexture.getHeight() * counterJ);
        }

        static int Main(string[] args)
        {
            SDL.SDL_SetHint(SDL.SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING, "1");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            //(Re-)Set point counter and timer
            gameReset();

            

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
                    pannelH = calcPannelH() + 10;

                    playMusic();

                    knightTexture = (LTexture)knightAnimList[3];

                    playerKnight.updateKnightTexture();

                    
                    ghostList.Add(new Ghost(ghostTexture));

                    ballList.Add(new Ball(ballTexture));

                    player.startPos(20, pannelH);
                    enemy.startPos(SCREEN_WIDTH - 40, pannelH);

                    double previous = 0.0;

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

                        //Handle events on queue
                        handleUserInput();

                        //Update nach deltaTime?
                        updateGame(elapsed);

                        // createButton();
                        drawBackground();

                        if (paused)
                        {
                            
                            if (gameover)
                            {
                                changeText(alertTextTexture1, "GAME OVER - PRESS R TO RETRY");


                                foreach (Ball ballsy in ballList)
                                {
                                    ballsy.gDotTexture.setAlpha(0); //ToDo , ändern Quickfix wegen transparanz
                                }
                            }

                            alertTextTexture1.render((SCREEN_WIDTH / 2) - (alertTextTexture1.getWidth() / 2),
                                (SCREEN_HEIGHT / 2));
                        }

                        renderObjects();


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