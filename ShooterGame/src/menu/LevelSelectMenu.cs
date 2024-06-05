﻿using System;
using System.Collections.Generic;
using SDL2;

namespace ShooterGame
{
    public class LevelSelectMenu : Menu
    {
        public LevelSelectMenu(IntPtr renderer) : base(renderer)
        {
            MenuItems = new List<MenuItem>();
            
            // Calculate the height for each menu item
            int itemHeight = Program.SCREEN_HEIGHT / 5; // Divide by the number of menu items +1 
            int itemWidth = 200;
            
            // Create the level items with their positions
            MenuItem level1Item = new MenuItem("Level 1", () => 
                { 
                    Program.CurrentState = GameState.IN_GAME; 
                    Program.CurrentLevel = 1; // Set the current level to 1
                }, 
                "Level 1", "lazy.ttf", new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = itemHeight, w = itemWidth, h = 50 }, new SDL.SDL_Color { r = 255, g = 255, b = 0, a = 255});
            
            MenuItem level2Item = new MenuItem("Level 2", () =>
                {
                Program.CurrentState = GameState.IN_GAME;
                Program.CurrentLevel = 2; // Set the current level to 2
                }, 
                "Level 2", "lazy.ttf", new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = 2 * itemHeight, w = itemWidth, h = 50 }, new SDL.SDL_Color { r = 255, g = 255, b = 0, a = 255});
            
            MenuItem level3Item = new MenuItem("Level 3", () =>
                {
                Program.CurrentState = GameState.IN_GAME;
                Program.CurrentLevel = 3; // Set the current level to 3
                },
                "Level 3", "lazy.ttf", new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = 3 * itemHeight, w = itemWidth, h = 50 }, new SDL.SDL_Color { r = 255, g = 255, b = 0, a = 255});
            
            MenuItem backItem = new MenuItem("Back", () =>
                {
                Program.CurrentState = GameState.MAIN_MENU;
                Program.mainMenu = new MainMenu(renderer);
                SDL.SDL_SetRenderDrawColor(Program.gRenderer, 0xFF, 0xFF, 0xFF, 0xFF);
                SDL.SDL_RenderClear(Program.gRenderer); // Clear the current rendering target with the drawing color
                }, 
                "Back", "lazy.ttf", new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = 4 * itemHeight, w = itemWidth, h = 50 }, new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255});

            // Add the level items to the menu
            AddMenuItem(level1Item);
            AddMenuItem(level2Item);
            AddMenuItem(level3Item);
            AddMenuItem(backItem);
            // Add more levels as needed
        }
        
        public override void Render(IntPtr renderer)
        {
            // Implement the specific rendering for the settings menu
            // You can use the DisplayText method from the Menu class to display the menu items
            for (int i = 0; i < MenuItems.Count; i++)
            {
                SDL.SDL_Rect position = MenuItems[i].Position;

                // If the current menu item is the selected one, change the color to the selected color
                if (i == SelectedIndex)
                {
                    DisplayText(MenuItems[i].Text, new Vector2D { X = position.x, Y = position.y }, position.w, MenuItems[i].Font, renderer, MenuItems[i].SelectedColor);
                }
                else
                {
                    // Otherwise, reset the color to white
                    SDL.SDL_Color white = new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 };
                    DisplayText(MenuItems[i].Text, new Vector2D { X = position.x, Y = position.y }, position.w, MenuItems[i].Font, renderer, white);
                }
            }
        }
        
    }
}
