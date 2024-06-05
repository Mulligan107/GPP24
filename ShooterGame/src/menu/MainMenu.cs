using System;
using SDL2;

namespace ShooterGame
{
    public class MainMenu : Menu
    {
        public MainMenu(IntPtr renderer) : base(renderer)
        {
            // Calculate the height for each menu item
            var menuItemSpacing = Program.SCREEN_HEIGHT / 4; // Divide by the number of menu items + 1
            var itemWidth = 200;

            var startItem = new MenuItem("Start", () =>
                {
                    Program.CurrentState = GameState.LEVEL_SELECT;
                    Program.VisibleMenu = new LevelSelectMenu(renderer);
                    Program.VisibleMenu.SelectedIndex = 0; // Reset the selected index
                    SDL.SDL_RenderClear(Program.gRenderer); // Clear the current rendering target with the drawing color
                },
                "Start", "lazy.ttf",
                new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = menuItemSpacing, w = itemWidth, h = 50 },
                new SDL.SDL_Color { r = 0, g = 255, b = 0, a = 255 }); //color when selected

            var settingsItem = new MenuItem("Settings", () =>
                {
                    Program.CurrentState = GameState.SETTINGS;
                    Program.VisibleMenu = new SettingsMenu(renderer);
                    Program.VisibleMenu.SelectedIndex = 0;
                    SDL.SDL_RenderClear(Program.gRenderer);
                },
                "Settings", "lazy.ttf",
                new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = 2 * menuItemSpacing, w = itemWidth, h = 50 },
                new SDL.SDL_Color { r = 255, g = 255, b = 0, a = 255 });

            var quitItem = new MenuItem("Quit", () =>
                {
                    Program.Close();
                    Environment.Exit(0);
                },
                "Quit", "lazy.ttf",
                new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = 3 * menuItemSpacing, w = itemWidth, h = 50 },
                new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255 });

            // Add the menu items to the menu
            AddMenuItem(startItem);
            AddMenuItem(settingsItem);
            AddMenuItem(quitItem);
        }

        public override void Render(IntPtr renderer)
        {
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