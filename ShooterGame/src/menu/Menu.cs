using System;
using System.Collections.Generic;
using SDL2;

namespace ShooterGame
{
    public class Menu
    {
        public List<MenuItem> menuItems = new List<MenuItem>();
        private int selectedIndex = 0;
        private IntPtr renderer; 

        
        public Menu(IntPtr renderer)
        {
            menuItems = new List<MenuItem>();

            // Calculate the height for each menu item
            int itemHeight = Program.SCREEN_HEIGHT / 4; // Divide by the number of menu items
            int itemWidth = 200;

            MenuItem startItem = new MenuItem("Start", () =>
                {
                    Program.CurrentState = GameState.LEVEL_SELECT;
                    Program.mainMenu = new LevelSelectMenu(renderer);
                    Program.mainMenu.selectedIndex = 0; // Reset the selected index
                    SDL.SDL_RenderClear(Program.gRenderer); // Clear the current rendering target with the drawing color
                },
                "Start", "lazy.ttf", new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = itemHeight, w = itemWidth, h = 50 }, new SDL.SDL_Color { r = 0, g = 255, b = 0, a = 255 });

            MenuItem settingsItem = new MenuItem("Settings", () =>
                {
                    Program.CurrentState = GameState.SETTINGS;
                    Program.mainMenu = new SettingsMenu(renderer);
                    Program.mainMenu.selectedIndex = 0; // Reset the selected index
                    SDL.SDL_RenderClear(Program.gRenderer); // Clear the current rendering target with the drawing color
                },
                "Settings", "lazy.ttf", new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = 2 * itemHeight, w = itemWidth, h = 50 }, new SDL.SDL_Color { r = 255, g = 255, b = 0, a = 255 });

            MenuItem quitItem = new MenuItem("Quit", () =>
                {
                    Program.Close();
                    Environment.Exit(0);
                },
                "Quit", "lazy.ttf", new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = 3 * itemHeight, w = itemWidth, h = 50 }, new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255 });

            // Add the menu items to the menu
            AddMenuItem(startItem);
            AddMenuItem(settingsItem);
            AddMenuItem(quitItem);
        }

        public void AddMenuItem(MenuItem menuItem)
        {
            menuItems.Add(menuItem);
        }

        public void SelectNextItem()
        {
            selectedIndex = (selectedIndex + 1) % menuItems.Count;
            System.Console.WriteLine(selectedIndex +1 + "/" + menuItems.Count);
        }

        public void SelectPreviousItem()
        {
            selectedIndex = (selectedIndex - 1 + menuItems.Count) % menuItems.Count;
            System.Console.WriteLine(selectedIndex +1 + "/" + menuItems.Count);

        }

        public void ExecuteSelectedItem()
        {
            // Execute the action of the selected menu item
            menuItems[selectedIndex].Action();

            // If the selected menu item is a level, change the game state to IN_GAME
            if (menuItems[selectedIndex].Label.StartsWith("Level"))
            {
                Program.CurrentState = GameState.IN_GAME;
            }
        }

        public void Render(IntPtr renderer)
        {
            for (int i = 0; i < menuItems.Count; i++)
            {
                SDL.SDL_Rect position = menuItems[i].Position;

                // If the current menu item is the selected one, change the color to the selected color
                if (i == selectedIndex)
                {
                    DisplayText(menuItems[i].Text, new Vector2D { X = position.x, Y = position.y }, position.w, menuItems[i].Font, renderer, menuItems[i].SelectedColor);
                }
                else
                {
                    // Otherwise, reset the color to white
                    SDL.SDL_Color white = new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 };
                    DisplayText(menuItems[i].Text, new Vector2D { X = position.x, Y = position.y }, position.w, menuItems[i].Font, renderer, white);
                }
            }
        }
        
        private void DisplayText(string scoreText, Vector2D position, int textWidth, string fonttext, IntPtr renderer, SDL.SDL_Color color)
        {
            IntPtr font = SDL_ttf.TTF_OpenFont(fonttext, 20);

            IntPtr surfaceMessage = SDL_ttf.TTF_RenderText_Solid(font, scoreText, color);
            IntPtr texture = SDL.SDL_CreateTextureFromSurface(renderer, surfaceMessage);

            SDL.SDL_Rect destRect = new SDL.SDL_Rect
            {
                x = (int)position.X - textWidth / 2,
                y = (int)position.Y,
                w = textWidth,
                h = textWidth / 4
            };

            SDL.SDL_RenderCopy(renderer, texture, IntPtr.Zero, ref destRect);
            SDL.SDL_DestroyTexture(texture);
            SDL.SDL_FreeSurface(surfaceMessage);
            SDL_ttf.TTF_CloseFont(font);
        }
    }
}