using JumperGame.src.components;
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
        LTexture tileTexEnvi = new LTexture();
        LTexture tileTexCoin = new LTexture();


        public RenderManager(ColorComponent colorComponent)
        {
            _colorComponent = colorComponent;
            
        }
        
        public void Update()
        {
            SDL.SDL_RenderClear(gRenderer);
            SDL.SDL_SetRenderDrawColor(gRenderer, _colorComponent.CurrentColor.r, _colorComponent.CurrentColor.g, _colorComponent.CurrentColor.b, _colorComponent.CurrentColor.a);

            var map = new TiledMap("src\\worlds\\testWorld.tmx");
            var tilesets = map.GetTiledTilesets("src/worlds/"); // DO NOT forget the / at the end
            var tileLayers = map.Layers.Where(x => x.Type == TiledLayerType.TileLayer);

            

            foreach (var layer in tileLayers)
            {
                for (var y = 0; y < layer.Height; y++)
                {
                    for (var x = 0; x < layer.Width; x++)
                    {
                        var index = (y * layer.Width) + x; // Assuming the default render order is used which is from right to bottom
                        var gid = layer.Data[index]; // The tileset tile index
                        var tileX = (x * map.TileWidth);
                        var tileY = (y * map.TileHeight);

                        // Gid 0 is used to tell there is no tile set
                        if (gid == 0)
                        {
                            continue;
                        }

                        // Helper method to fetch the right TieldMapTileset instance. 
                        // This is a connection object Tiled uses for linking the correct tileset to the gid value using the firstgid property.
                        var mapTileset = map.GetTiledMapTileset(gid);

                        // Retrieve the actual tileset based on the firstgid property of the connection object we retrieved just now
                       // var tileset = tilesets[mapTileset.firstgid];

                        // Use the connection object as well as the tileset to figure out the source rectangle.
                     //   var rect = map.GetSourceRect(mapTileset, tileset, gid);

                        SDL.SDL_Rect destRect = new SDL.SDL_Rect { x = tileX, y = tileY, h = map.TileWidth, w = map.TileWidth };

                        


                        if (gid < 257)
                        {
                            int fucker = (gid)/16;
                            

                            int foo = 256 * fucker; //kekw

                            SDL.SDL_Rect srcRect = new SDL.SDL_Rect { x = ((gid -1) * 16) - foo, y = fucker * 16 , h = map.TileWidth, w = map.TileWidth };
                            SDL.SDL_RenderCopy(gRenderer, tileTexEnvi.getTexture(), ref srcRect, ref destRect);



                           // Console.WriteLine("SRC: " + gid + " " + srcRect.x + " " + srcRect.y + " " + srcRect.h + " " + srcRect.w);
                           // Console.WriteLine("DEST: " + gid + " " + destRect.x + " " + destRect.y + " " + destRect.h + " " + destRect.w);
                        }
                        else
                        {
                            SDL.SDL_Rect srcRect = new SDL.SDL_Rect { x = gid -256, y = 0, h = map.TileWidth, w = map.TileWidth };
                            SDL.SDL_RenderCopy(gRenderer, tileTexCoin.getTexture(), ref srcRect, ref destRect);
                        }

                        // Render sprite at position tileX, tileY using the rect
                    }
                }
            }

                SDL.SDL_RenderPresent(gRenderer);
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
                    }
                }
            }

            tileTexEnvi.loadFromFile("src\\tilesets/world_tileset.png");

            tileTexCoin.loadFromFile("src\\tilesets/coin.png");

            return true;
        }
    }
}
