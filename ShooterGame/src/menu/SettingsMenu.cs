using System;
using System.Collections.Generic;
using SDL2;

namespace ShooterGame
{
    public class SettingsMenu : Menu
    {
        public SettingsMenu(IntPtr renderer) : base(renderer)
        {
            MenuItems = new List<MenuItem>();

            // Calculate the height for each menu item
            var menuItemSpacing = Program.SCREEN_HEIGHT / 3; // Divide by the number of menu items +1
            var itemWidth = 200;

            // Create the settings items with their positions
            var changeWindowSizeItem = new MenuItem("Change Window Size", () => { Program.changeWindowSize(); },
                "Change Window Size", "lazy.ttf",
                new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = menuItemSpacing, w = itemWidth, h = 50 },
                new SDL.SDL_Color { r = 255, g = 255, b = 0, a = 255 }); //color when selected

            var backItem = new MenuItem("Back", () =>
                {
                    Program.CurrentState = GameState.MAIN_MENU;
                    Program.VisibleMenu = new MainMenu(renderer);
                    SDL.SDL_SetRenderDrawColor(Program.gRenderer, 0xFF, 0xFF, 0xFF, 0xFF);
                    SDL.SDL_RenderClear(Program.gRenderer); // Clear the current rendering target with the drawing color
                },
                "Back", "lazy.ttf",
                new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = 2 * menuItemSpacing, w = itemWidth, h = 50 },
                new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255 });

            AddMenuItem(changeWindowSizeItem);
            AddMenuItem(backItem);
        }

        public override void Render(IntPtr renderer)
        {
            // Implement the specific rendering for the settings menu
            // You can use the DisplayText method from the Menu class to display the menu items
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