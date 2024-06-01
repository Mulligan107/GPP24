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

        public MenuItem(string label, Action action, IntPtr texture, SDL.SDL_Rect position)
        {
            Label = label;
            Action = action;
            Texture = texture;
            Position = position;
        }
    }

    public class Menu
    {
        private List<MenuItem> menuItems = new List<MenuItem>();
        private int selectedIndex = 0;
        
        public Menu(IntPtr renderer)
        {
            // Load the textures for the menu items
            IntPtr startTexture = SDL_image.IMG_LoadTexture(renderer, "imgs/start.png");
            IntPtr settingsTexture = SDL_image.IMG_LoadTexture(renderer, "imgs/settings.png");
            IntPtr quitTexture = SDL_image.IMG_LoadTexture(renderer, "imgs/quit.png");

            // Create the menu items with their positions
            MenuItem startItem = new MenuItem("Start", () => { /* code to start the game */ }, startTexture, new SDL.SDL_Rect { x = 100, y = 100, w = 200, h = 50 });
            MenuItem settingsItem = new MenuItem("Settings", () => { /* code to open settings */ }, settingsTexture, new SDL.SDL_Rect { x = 100, y = 200, w = 200, h = 50 });
            MenuItem quitItem = new MenuItem("Quit", () => { /* code to quit the game */ }, quitTexture, new SDL.SDL_Rect { x = 100, y = 300, w = 200, h = 50 });

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
                SDL.SDL_RenderCopy(renderer, menuItems[i].Texture, IntPtr.Zero, ref menuItems[i].Position);
            }
        }
    }
}