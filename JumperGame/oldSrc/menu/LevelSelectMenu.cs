using System;
using System.Collections.Generic;
using SDL2;
using ShooterGame.level;

namespace ShooterGame
{
    public class LevelSelectMenu : Menu
    {
        public LevelSelectMenu(IntPtr renderer) : base(renderer)
        {
            MenuItems = new List<MenuItem>();

            // Calculate the height for each menu item
            var menuItemSpacing = Program.SCREEN_HEIGHT / 5; // Divide by the number of menu items +1 
            var itemWidth = 200;

            // Create the level items with their positions
            var level1Item = new MenuItem("Level 1", () =>
                {
                    Program.CurrentState = GameState.IN_GAME;
                    LevelManager.CurrentLevel = 0;
                    LevelManager.ResetStats();
                },
                "Level 1", "lazy.ttf",
                new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = menuItemSpacing, w = itemWidth, h = 50 },
                new SDL.SDL_Color { r = 255, g = 255, b = 0, a = 255 }); //color when selected

            var level2Item = new MenuItem("Level 2", () =>
                {
                    Program.CurrentState = GameState.IN_GAME;
                    LevelManager.CurrentLevel = 1;
                    LevelManager.ResetStats();
                },
                "Level 2", "lazy.ttf",
                new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = 2 * menuItemSpacing, w = itemWidth, h = 50 },
                new SDL.SDL_Color { r = 255, g = 255, b = 0, a = 255 });

            var level3Item = new MenuItem("Level 3", () =>
                {
                    Program.CurrentState = GameState.IN_GAME;
                    LevelManager.CurrentLevel = 2; 
                    LevelManager.ResetStats();
                },
                "Level 3", "lazy.ttf",
                new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = 3 * menuItemSpacing, w = itemWidth, h = 50 },
                new SDL.SDL_Color { r = 255, g = 255, b = 0, a = 255 });

            var backItem = new MenuItem("Back", () =>
                {
                    Program.CurrentState = GameState.MAIN_MENU;
                    Program.VisibleMenu = new MainMenu(renderer);
                    SDL.SDL_SetRenderDrawColor(Program.gRenderer, 0xFF, 0xFF, 0xFF, 0xFF);
                    SDL.SDL_RenderClear(Program.gRenderer); // Clear the current rendering target with the drawing color
                },
                "Back", "lazy.ttf",
                new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = 4 * menuItemSpacing, w = itemWidth, h = 50 },
                new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255 });

            // Add the level items to the menu
            AddMenuItem(level1Item);
            AddMenuItem(level2Item);
            AddMenuItem(level3Item);
            AddMenuItem(backItem);
        }

        public override void Render(IntPtr renderer)
        {
            var titlePosition = new Vector2D { X = Program.SCREEN_WIDTH / 2, Y = Program.SCREEN_HEIGHT / 8 }; // Adjust the Y value as needed
            var titleColor = new SDL.SDL_Color { r = 0, g = 0, b = 0, a = 255 }; // White color
            DisplayText("Level Selection", titlePosition, 300, "lazy.ttf", renderer, titleColor); // Adjust the width as needed

            for (var i = 0; i < MenuItems.Count; i++)
            {
                var position = MenuItems[i].Position;

                // If the current menu item is the selected one, change the color to the selected color
                if (i == SelectedIndex)
                {
                    DisplayText(MenuItems[i].Text, new Vector2D { X = position.x, Y = position.y }, position.w,
                        MenuItems[i].Font, renderer, MenuItems[i].SelectedColor);
                }
                else
                {
                    // Otherwise, reset the color to white
                    var white = new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 };
                    DisplayText(MenuItems[i].Text, new Vector2D { X = position.x, Y = position.y }, position.w,
                        MenuItems[i].Font, renderer, white);
                }
            }
        }
    }
}