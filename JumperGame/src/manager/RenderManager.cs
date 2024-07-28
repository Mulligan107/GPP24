using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using SDL2;
using JumperGame.gameEntities;
using JumperGame.components;
using JumperGame.src.components;
using JumperGame.systems;

namespace JumperGame.src.manager
{
    public class RenderManager
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

        public IntPtr fadeTexture = IntPtr.Zero;

        public bool death;
        public bool levelStart;

        LTexture balken = new LTexture();
        LTexture black = new LTexture();

        SDL.SDL_Rect view;

        public string levelname;

        double counter = 0;
        double fadeCounter = 255;
        double balkenCounter = 0;
        int frame = 1;

        public static IntPtr Font = IntPtr.Zero;

        private CoinCounterSystem _coinCounterSystem;
        private LifeSystem _lifeSystem;
        
        public RenderManager()
        {
            _lifeSystem = LifeSystem.Instance;
            _coinCounterSystem = CoinCounterSystem.Instance; 
            Initialize();
            view = new SDL.SDL_Rect { x = 0, y = 0, h = ScreenHeight / 3, w = ScreenWidth / 3 };
        }
        
        public void Update(double dt, double timeElapsed, MenuSystem menuSystem)
        {
            SDL.SDL_RenderClear(gRenderer);

            SDL.SDL_Rect camera = new SDL.SDL_Rect { w = ScreenWidth, h = ScreenHeight };
            
            Entity player = JumperGame.entitySystem.GetEntityByGID(281);
            var posi = player.GetComponent<RenderComponent>();
            SDL.SDL_Rect newPosi = posi.dstRect;

            //Center the camera over the player
            camera.x = (int)(newPosi.x + newPosi.w / 2) - (camera.w / 2 ) / 3; // TODO (/3) mit variable ersetzen
            camera.y = (int)(newPosi.y) - (camera.h /2) / 3;
            
            // Keep the camera in bounds
            if (camera.x < 0) camera.x = 0;
            if (camera.y < 0) camera.y = 0;
            if (camera.x > levelWidth - camera.w) camera.x = levelWidth - camera.w;
            if (camera.y > 1200 - camera.h) camera.y = 1200 - camera.h;


            SDL.SDL_RenderSetScale(gRenderer, 3f, 3f); // TODO 3 mit variable ersetzen ; Mausrad?

            //Render background

            foreach (Entity enti in JumperGame.entitySystem.GetAllEntities())
            {
                // Check if the entity is active before rendering
                if (!enti.IsActive) continue; // Skip rendering if not active
                
                var renderComponent = enti.GetComponent<RenderComponent>();
                var animationComponent = enti.GetComponent<AnimationComponent>();

                if (renderComponent != null)
                {

                    SDL.SDL_Rect src = renderComponent.srcRect;
                    SDL.SDL_Rect dst = renderComponent.dstRect;

                    // ---------- AnimationManager?
                    if (enti == player && renderComponent.dstRect.y > 950)
                    {
                        death = true;


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




                    if (enti.activeSTATE == Entity.STATE.AIRTIME)
                    {
                        renderComponent.centerPoint = new SDL.SDL_Point {x = adjustedDst.w/2 , y = adjustedDst.h/2  };


                        if(renderComponent.flip == SDL.SDL_RendererFlip.SDL_FLIP_NONE)
                        {
                            renderComponent.angle += dt * 1000;
                        }
                        else
                        {
                            renderComponent.angle -= dt * 1000;
                        }
                        
                    }
                    else
                    {
                        renderComponent.angle = 0;
                    }

                    

                    

                    if (SDL.SDL_HasIntersection(ref dst, ref camera) == SDL.SDL_bool.SDL_TRUE)
                    {
                        SDL.SDL_RenderCopyEx(gRenderer, renderComponent.Rendertexture.getTexture(), ref src, ref adjustedDst, renderComponent.angle, ref renderComponent.centerPoint, renderComponent.flip);
                    }

                }
            }


            //Render objects
            
            _lifeSystem.RenderLifeCount(this);
            _coinCounterSystem.RenderCoinCount(this);

            /*
            timerTexture = changeText(timerTexture, "Delta: " + dt.ToString("F3") + " Timer: " + timeElapsed.ToString("F3"));
            timerTexture.render(10, 10);
            */

            if (death)
            {
                fadeCounter += dt * 500;

                if (fadeCounter > 255)
                {
                    fadeCounter = 255;
                    death = false;
                    levelStart = true;
                    deathEvent(menuSystem);
                }

                black.setAlpha((byte)fadeCounter);
            }


            if (levelStart)
            {
                fadeCounter -= dt * 300;
                balkenCounter += dt * 100;

                if (fadeCounter < 0) // stops flickering. if it goes below 0;
                {
                    fadeCounter = 0;  
                }

                if (balkenCounter > 80)
                {
                    view.y -= (int)(dt * 200);
                    view.h += (int)(dt * 400);
                }

                if (balkenCounter > 300 && fadeCounter == 0)
                {
                    levelStart = false;
                }


                black.setAlpha((byte)fadeCounter);

            }





            SDL.SDL_RenderCopy(gRenderer, black.getTexture() , IntPtr.Zero, ref view);
            SDL.SDL_RenderCopy(gRenderer, balken.getTexture(), IntPtr.Zero, ref view);

            menuSystem.Render();

            SDL.SDL_RenderPresent(gRenderer);
        }
        
        //Ich hasse mich selber für diesen Code aber hab grad keinen bock das zu refactoren / PP anzuwenden
        //TODO: Refactor this
        //Verzeih mir Thoma
        public void InitializeMenu(MenuSystem menuSystem)
        {
            var playMenuItem = new MenuItemEntity(
                new MenuComponent("Play", new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 }, new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255 }, () => InitializePlayMenu(menuSystem), "lazy.ttf"),
                new MenuPositionComponent(new SDL.SDL_Rect())
            );

            var settingsMenuItem = new MenuItemEntity(
                new MenuComponent("Settings", new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 }, new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255 }, () => InitializeSettingsMenu(menuSystem), "lazy.ttf"),
                new MenuPositionComponent(new SDL.SDL_Rect())
            );

            var exitMenuItem = new MenuItemEntity(
                new MenuComponent("Quit Game", new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 }, new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255 }, menuSystem.ExitGame, "lazy.ttf"),
                new MenuPositionComponent(new SDL.SDL_Rect())
            );

            InitializeSubMenu(menuSystem, new List<MenuItemEntity> { playMenuItem, settingsMenuItem, exitMenuItem });
        }

        public void deathEvent(MenuSystem menuSystem)
        {
            switch (levelname)
            {
                case "Level1":
                    menuSystem.StartLevel1();
                    break;
                case "Level2":
                    menuSystem.StartLevel2();
                    break;
                case "movementTest":
                    menuSystem.StartLevel3();
                    break;
            }
        }

            private void InitializeSubMenu(MenuSystem menuSystem, List<MenuItemEntity> menuItems)
        {
            menuSystem.ClearMenuItems(); 

            int centerX = ScreenWidth / 2 / 3; 
            int itemHeight = 50;
            int totalHeight = menuItems.Count * itemHeight;
            int startY = (ScreenHeight / 2 / 3) - (totalHeight / 2); // Centering the list

            for (int i = 0; i < menuItems.Count; i++)
            {
                var menuItem = menuItems[i];
                menuItem.PositionComponent.Position = new SDL.SDL_Rect { x = centerX - 100, y = startY + i * itemHeight, w = 200, h = itemHeight };
                menuSystem.AddMenuItem(menuItem);
            }
        }

        public void resetSystem()
        {
            fadeCounter = 255;
            balkenCounter = 0;
            view = new SDL.SDL_Rect { x = 0, y = 0, h = ScreenHeight / 3, w = ScreenWidth / 3 };
        }

        public void InitializePlayMenu(MenuSystem menuSystem)
        {
            
            resetSystem();
            var level1MenuItem = new MenuItemEntity(
                new MenuComponent("Level 1", new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 }, new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255 }, menuSystem.StartLevel1, "lazy.ttf"),
                new MenuPositionComponent(new SDL.SDL_Rect())
            );

            var level2MenuItem = new MenuItemEntity(
                new MenuComponent("Level 2", new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 }, new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255 }, menuSystem.StartLevel2, "lazy.ttf"),
                new MenuPositionComponent(new SDL.SDL_Rect())
            );

            var level3MenuItem = new MenuItemEntity(
                new MenuComponent("Level 3", new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 }, new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255 }, menuSystem.StartLevel3, "lazy.ttf"),
                new MenuPositionComponent(new SDL.SDL_Rect())
            );

            var backMenuItem = new MenuItemEntity(
                new MenuComponent("Back", new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 }, new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255 }, () => InitializeMenu(menuSystem), "lazy.ttf"),
                new MenuPositionComponent(new SDL.SDL_Rect())
            );

            InitializeSubMenu(menuSystem, new List<MenuItemEntity> { level1MenuItem, level2MenuItem, level3MenuItem, backMenuItem });
        }
        
        public void InitializeSettingsMenu(MenuSystem menuSystem)
        {
            var backMenuItem = new MenuItemEntity(
                new MenuComponent("Back", new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 }, new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255 }, () => InitializeMenu(menuSystem), "lazy.ttf"),
                new MenuPositionComponent(new SDL.SDL_Rect())
            );

            InitializeSubMenu(menuSystem, new List<MenuItemEntity> { backMenuItem });
        }
        
        public void InitializePauseMenu(MenuSystem menuSystem)
        {
            var resumeMenuItem = new MenuItemEntity(
                new MenuComponent("Resume", new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 }, new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255 }, menuSystem.resume, "lazy.ttf"),
                new MenuPositionComponent(new SDL.SDL_Rect())
            );
            
            var mainMenuItem = new MenuItemEntity(
                new MenuComponent("Back to Main Menu", new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 }, new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255 }, () => InitializeMenu(menuSystem), "lazy.ttf"),
                new MenuPositionComponent(new SDL.SDL_Rect())
            );
            
            var quitMenuItem = new MenuItemEntity(
                new MenuComponent("Quit Game", new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 }, new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255 }, menuSystem.ExitGame, "lazy.ttf"),
                new MenuPositionComponent(new SDL.SDL_Rect())
            );

            InitializeSubMenu(menuSystem, new List<MenuItemEntity> { resumeMenuItem, mainMenuItem, quitMenuItem });
        }
        
        public void InitializeDeathMenu(MenuSystem menuSystem)
        {
            var mainMenuItem = new MenuItemEntity(
                new MenuComponent("Back to Main Menu", new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 }, new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255 }, () => InitializeMenu(menuSystem), "lazy.ttf"),
                new MenuPositionComponent(new SDL.SDL_Rect())
            );
            
            var quitMenuItem = new MenuItemEntity(
                new MenuComponent("Quit Game", new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 }, new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255 }, menuSystem.ExitGame, "lazy.ttf"),
                new MenuPositionComponent(new SDL.SDL_Rect())
            );

            InitializeSubMenu(menuSystem, new List<MenuItemEntity> { mainMenuItem, quitMenuItem});
        }
        
        public void RenderLifeCount(int lifeCount)
        {
            string lifeCountText = "Lives: " + lifeCount.ToString();
            LTexture lifeCountTexture = new LTexture();
            lifeCountTexture = changeText(lifeCountTexture, lifeCountText);
            lifeCountTexture.render(10, 100);
        }
        
        public void RenderCoinCount(int coinCount)
        {
            string coinCountText = "Coins: " + coinCount.ToString();
            LTexture coinCountTexture = new LTexture();
            coinCountTexture = changeText(coinCountTexture, coinCountText);
            coinCountTexture.render(10, 50);
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


            black.loadFromFile("src\\tilesets/black.png");
            balken.loadFromFile("src\\tilesets/balken.png");

            return true;
        }
    }
}
