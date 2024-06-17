using System;
using SDL2;
using ShooterGame.level;

namespace ShooterGame
{
    public class WinMenu : Menu
    {
        public WinMenu(IntPtr renderer) : base(renderer)
        {
            var menuItemSpacing = Program.SCREEN_HEIGHT / 3; // Divide by the number of menu items + 1
            var itemWidth = 200;

            if (LevelManager.CurrentLevel < 2)
            {
                var nextLevelItem = new MenuItem("Next Level", () =>
                    {
                        LevelManager.AdvanceToNextLevel();
                        Program.CurrentState = GameState.IN_GAME;
                    },
                    "Next Level", "lazy.ttf",
                    new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = menuItemSpacing, w = itemWidth, h = 50 },
                    new SDL.SDL_Color { r = 0, g = 255, b = 0, a = 255 }); //color when selected

                AddMenuItem(nextLevelItem);
            }

            var mainMenuItem = new MenuItem("Main Menu", () =>
                {
                    Program.reset = true;
                    Program.CurrentState = GameState.MAIN_MENU;
                    Program.VisibleMenu = new MainMenu(renderer);
                    SDL.SDL_RenderClear(Program.gRenderer);
                },
                "Main Menu", "lazy.ttf",
                new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = 2 * menuItemSpacing, w = itemWidth, h = 50 },
                new SDL.SDL_Color { r = 255, g = 255, b = 0, a = 255 });

            //nextlevel item init above
            AddMenuItem(mainMenuItem);
        }

        public override void Render(IntPtr renderer)
        {
            // Display the title
            var titlePosition = new Vector2D { X = Program.SCREEN_WIDTH / 2, Y = Program.SCREEN_HEIGHT / 8 }; // Adjust the Y value as needed
            var titleColor = new SDL.SDL_Color { r = 0, g = 0, b = 0, a = 255 }; // White color
            DisplayText("You Win!", titlePosition, 500, "lazy.ttf", renderer, titleColor); // Adjust the width as needed

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