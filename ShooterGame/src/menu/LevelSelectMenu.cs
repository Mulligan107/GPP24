using System;
using System.Collections.Generic;
using SDL2;

namespace ShooterGame
{
    public class LevelSelectMenu : Menu
    {
        public LevelSelectMenu(IntPtr renderer) : base(renderer)
        {
            menuItems = new List<MenuItem>();
            
            // Load the textures for the level items
            IntPtr level1Texture = SDL_image.IMG_LoadTexture(renderer, "imgs/menu/level1.png");
            IntPtr level2Texture = SDL_image.IMG_LoadTexture(renderer, "imgs/menu/level2.png");
            IntPtr level3Texture = SDL_image.IMG_LoadTexture(renderer, "imgs/menu/level3.png");
            IntPtr backTexture = SDL_image.IMG_LoadTexture(renderer, "imgs/menu/Back.png");

            // Calculate the height for each menu item
            int itemHeight = Program.SCREEN_HEIGHT / 5; // Divide by the number of menu items +1 
            int itemWidth = 200;
            
            // Create the level items with their positions
            MenuItem level1Item = new MenuItem("Level 1", () => 
                { 
                    Program.CurrentState = GameState.IN_GAME; 
                    Program.CurrentLevel = 1; // Set the current level to 1
                }, 
                level1Texture, new SDL.SDL_Rect { x = (Program.SCREEN_WIDTH - itemWidth) / 2, y = itemHeight, w = itemWidth, h = 50 }, new SDL.SDL_Color { r = 255, g = 255, b = 0, a = 255});
            
            MenuItem level2Item = new MenuItem("Level 2", () =>
                {
                Program.CurrentState = GameState.IN_GAME;
                Program.CurrentLevel = 2; // Set the current level to 2
                }, 
                level2Texture, new SDL.SDL_Rect { x = (Program.SCREEN_WIDTH - itemWidth) / 2, y = 2 * itemHeight, w = itemWidth, h = 50 }, new SDL.SDL_Color { r = 255, g = 255, b = 0, a = 255});
            
            MenuItem level3Item = new MenuItem("Level 3", () =>
                {
                Program.CurrentState = GameState.IN_GAME;
                Program.CurrentLevel = 3; // Set the current level to 3
                },
                level3Texture, new SDL.SDL_Rect { x = (Program.SCREEN_WIDTH - itemWidth) / 2, y = 3 * itemHeight, w = itemWidth, h = 50 }, new SDL.SDL_Color { r = 255, g = 255, b = 0, a = 255});
            
            MenuItem backItem = new MenuItem("Back", () =>
                {
                Program.CurrentState = GameState.MAIN_MENU;
                Program.mainMenu = new Menu(renderer);
                SDL.SDL_SetRenderDrawColor(Program.gRenderer, 0xFF, 0xFF, 0xFF, 0xFF);
                SDL.SDL_RenderClear(Program.gRenderer); // Clear the current rendering target with the drawing color
                }, 
                backTexture, new SDL.SDL_Rect { x = (Program.SCREEN_WIDTH - itemWidth) / 2, y = 4 * itemHeight, w = itemWidth, h = 50 }, new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255});

            // Add the level items to the menu
            AddMenuItem(level1Item);
            AddMenuItem(level2Item);
            AddMenuItem(level3Item);
            AddMenuItem(backItem);
            // Add more levels as needed
        }
    }
}