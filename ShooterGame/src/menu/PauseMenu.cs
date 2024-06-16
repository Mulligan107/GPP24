using ShooterGame.level;

namespace ShooterGame
{
    using System;
using SDL2;

namespace ShooterGame
{
    public class PauseMenu : Menu
    {
        public PauseMenu(IntPtr renderer) : base(renderer)
        {
            var menuItemSpacing = Program.SCREEN_HEIGHT / 3; // Divide by the number of menu items + 1
            var itemWidth = 200;

            var resumeItem = new MenuItem("Resume", () =>
                {
                    Program.CurrentState = GameState.IN_GAME;
                },
                "Resume", "lazy.ttf",
                new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = menuItemSpacing, w = itemWidth, h = 50 },
                new SDL.SDL_Color { r = 0, g = 255, b = 0, a = 255 }); //color when selected

            var mainMenuItem = new MenuItem("Main Menu", () =>
                {
                    Program.reset = true;
                    
                    LevelManager.ResetStats();
                    
                    Program.CurrentState = GameState.MAIN_MENU;
                    Program.VisibleMenu = new MainMenu(renderer);
                    SDL.SDL_RenderClear(Program.gRenderer);
                },
                "Main Menu", "lazy.ttf",
                new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = 2 * menuItemSpacing, w = itemWidth, h = 50 },
                new SDL.SDL_Color { r = 255, g = 255, b = 0, a = 255 });

            // Add the menu items to the menu
            AddMenuItem(resumeItem);
            AddMenuItem(mainMenuItem);
        }

        public override void Render(IntPtr renderer)
        {
            // Display the title
            var titlePosition = new Vector2D { X = Program.SCREEN_WIDTH / 2, Y = Program.SCREEN_HEIGHT / 8 }; // Adjust the Y value as needed
            var titleColor = new SDL.SDL_Color { r = 0, g = 0, b = 0, a = 255 }; // White color
            DisplayText("Paused", titlePosition, 500, "lazy.ttf", renderer, titleColor); // Adjust the width as needed

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
}