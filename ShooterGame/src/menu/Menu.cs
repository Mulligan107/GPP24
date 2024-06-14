using System;
using System.Collections.Generic;
using SDL2;

namespace ShooterGame
{
    public abstract class Menu
    {
        public List<MenuItem> MenuItems = new List<MenuItem>();
        public int SelectedIndex;
        protected IntPtr Renderer;

        protected Menu(IntPtr renderer)
        {
            Renderer = renderer;
        }

        public void AddMenuItem(MenuItem menuItem)
        {
            MenuItems.Add(menuItem);
        }

        public void SelectNextItem()
        {
            SelectedIndex = (SelectedIndex + 1) % MenuItems.Count;
            System.Console.WriteLine("Selected Menu Item: " + (SelectedIndex + 1) + ", " + "Out of: " + MenuItems.Count);
        }

        public void SelectPreviousItem()
        {
            SelectedIndex = (SelectedIndex - 1 + MenuItems.Count) % MenuItems.Count;
            System.Console.WriteLine("Selected Menu Item: " + (SelectedIndex + 1) + " / " + "Out of " + MenuItems.Count);
        }

        public void ExecuteSelectedItem()
        {
            MenuItems[SelectedIndex].Action();
        }

        public abstract void Render(IntPtr renderer);
        
        public void DisplayText(string scoreText, Vector2D position, int textWidth, string fonttext, IntPtr renderer, SDL.SDL_Color color)
        {
            IntPtr font = SDL_ttf.TTF_OpenFont(fonttext, 60);

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
        
        public void UpdateMenuItemPositions()
        {
            var menuItemSpacing = Program.SCREEN_HEIGHT / (MenuItems.Count + 1);
            var itemWidth = 200;

            for (var i = 0; i < MenuItems.Count; i++)
            {
                MenuItems[i].Position = new SDL.SDL_Rect
                {
                    x = Program.SCREEN_WIDTH / 2,
                    y = (i + 1) * menuItemSpacing,
                    w = itemWidth,
                    h = 50
                };
            }
        }
    }
}