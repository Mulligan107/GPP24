using System;
using System.Collections.Generic;
using SDL2;

namespace ShooterGame
{
    public class SettingsMenu : Menu
    {
        public SettingsMenu(IntPtr renderer) : base(renderer)
        {
            menuItems = new List<MenuItem>();
            
            // Load the textures for the settings items
            IntPtr changeWindowSizeTexture = SDL_image.IMG_LoadTexture(renderer, "imgs/menu/Back.png");
            IntPtr backTexture = SDL_image.IMG_LoadTexture(renderer, "imgs/menu/Back.png");
            
            // Calculate the height for each menu item
            int itemHeight = Program.SCREEN_HEIGHT / 3; // Divide by the number of menu items +1 
            int itemWidth = 200;

            // Create the settings items with their positions
            MenuItem changeWindowSizeItem = new MenuItem("Change Window Size", () => 
                { 
                    Program.changeWindowSize(); 
                }, 
                changeWindowSizeTexture, new SDL.SDL_Rect { x = (Program.SCREEN_WIDTH - itemWidth) / 2, y = itemHeight, w = itemWidth, h = 50 }, new SDL.SDL_Color { r = 255, g = 255, b = 0, a = 255});
            
            MenuItem backItem = new MenuItem("Back", () =>
                {
                    Program.CurrentState = GameState.MAIN_MENU;
                    Program.mainMenu = new Menu(renderer);
                    SDL.SDL_SetRenderDrawColor(Program.gRenderer, 0xFF, 0xFF, 0xFF, 0xFF);
                    SDL.SDL_RenderClear(Program.gRenderer); // Clear the current rendering target with the drawing color
                }, 
                backTexture, new SDL.SDL_Rect { x = (Program.SCREEN_WIDTH - itemWidth) / 2, y = 2 * itemHeight, w = itemWidth, h = 50 }, new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255});
            
            AddMenuItem(changeWindowSizeItem);
            AddMenuItem(backItem);
        }
    }
}