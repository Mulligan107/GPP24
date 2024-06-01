using System;
using System.Collections.Generic;
using SDL2;

namespace ShooterGame
{
    public class MenuItem
    {
        public string Label { get; set; }
        public Action Action { get; set; }
        public IntPtr Texture { get; set; }
        public SDL.SDL_Rect Position { get; set; }
        public SDL.SDL_Color SelectedColor { get; set; } 

        public MenuItem(string label, Action action, IntPtr texture, SDL.SDL_Rect position, SDL.SDL_Color selectedColor)
        {
            Label = label;
            Action = action;
            Texture = texture;
            Position = position;
            SelectedColor = selectedColor;
        }
    }

    public class Menu
    {
        public List<MenuItem> menuItems = new List<MenuItem>();
        private int selectedIndex = 0;
        private IntPtr renderer; 

        
        public Menu(IntPtr renderer)
        {
            
            // Load the textures for the menu items
            IntPtr startTexture = SDL_image.IMG_LoadTexture(renderer, "imgs/start.png");
            IntPtr settingsTexture = SDL_image.IMG_LoadTexture(renderer, "imgs/settings.png");
            IntPtr quitTexture = SDL_image.IMG_LoadTexture(renderer, "imgs/quit.png");

            // Create the menu items with their positions
            MenuItem startItem = new MenuItem("Start", () => 
                { 
                    Program.CurrentState = GameState.InGame; 
                    SDL.SDL_SetRenderDrawColor(Program.gRenderer, 0xFF, 0xFF, 0xFF, 0xFF); // Set the draw color to white
                    SDL.SDL_RenderClear(Program.gRenderer); // Clear the current rendering target with the drawing color
                }, 
                startTexture, new SDL.SDL_Rect { x = 100, y = 100, w = 200, h = 50 }, new SDL.SDL_Color { r = 255, g = 255, b = 0, a = 255});
            MenuItem settingsItem = new MenuItem("Settings", () =>
                {
                     /* code to open settings */
                }, settingsTexture, new SDL.SDL_Rect { x = 100, y = 200, w = 200, h = 50 }, new SDL.SDL_Color { r = 255, g = 255, b = 0, a = 255});
            MenuItem quitItem = new MenuItem("Quit", () =>
            {
                Program.Close();
                Environment.Exit(0);
            }, quitTexture, new SDL.SDL_Rect { x = 100, y = 300, w = 200, h = 50 }, new SDL.SDL_Color { r = 255, g = 255, b = 0, a = 255});

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
        }

        public void SelectPreviousItem()
        {
            selectedIndex = (selectedIndex - 1 + menuItems.Count) % menuItems.Count;
        }

        public void ExecuteSelectedItem()
        {
            menuItems[selectedIndex].Action();
        }

        public void Render(IntPtr renderer)
        {
            for (int i = 0; i < menuItems.Count; i++)
            {
                SDL.SDL_Rect position = menuItems[i].Position;

                // If the current menu item is the selected one, change the color to the selected color
                if (i == selectedIndex)
                {
                    SDL.SDL_SetTextureColorMod(menuItems[i].Texture, menuItems[i].SelectedColor.r, menuItems[i].SelectedColor.g, menuItems[i].SelectedColor.b);
                }
                else
                {
                    // Otherwise, reset the color to white
                    SDL.SDL_SetTextureColorMod(menuItems[i].Texture, 255, 255, 255);
                }

                SDL.SDL_RenderCopy(renderer, menuItems[i].Texture, IntPtr.Zero, ref position);
            }
        }
    }
}