
using System;
using System.Collections.Generic;
using JumperGame.gameEntities;
using SDL2;

namespace JumperGame.systems
{
    public class MenuSystem
    {
        private List<MenuItemEntity> _menuItems;
        private int _selectedIndex;
        private IntPtr _renderer;

        public MenuSystem(IntPtr renderer)
        {
            _menuItems = new List<MenuItemEntity>();
            _selectedIndex = 0;
            _renderer = renderer;
        }

        public void AddMenuItem(MenuItemEntity menuItem)
        {
            menuItem.MenuComponent.Texture.loadFromRenderedText(menuItem.MenuComponent.Text, menuItem.MenuComponent.Color);

            _menuItems.Add(menuItem);
        }

        public void SelectNextItem()
        {
            _selectedIndex = (_selectedIndex + 1) % _menuItems.Count;
            
            Console.WriteLine("Selected Menu Item: " + (_selectedIndex + 1) + " / " + "Out of " + _menuItems.Count);

        }

        public void SelectPreviousItem()
        {
            _selectedIndex = (_selectedIndex - 1 + _menuItems.Count) % _menuItems.Count;
        }

        public void ExecuteSelectedItem()
        {
            _menuItems[_selectedIndex].MenuComponent.Action();
        }

        public void Render()
        {
            foreach (var menuItem in _menuItems)
            {
                var color = menuItem == _menuItems[_selectedIndex] ? menuItem.MenuComponent.SelectedColor : menuItem.MenuComponent.Color;
                menuItem.MenuComponent.Texture.loadFromRenderedText(menuItem.MenuComponent.Text, color);
                menuItem.MenuComponent.Texture.render(menuItem.PositionComponent.Position.x, menuItem.PositionComponent.Position.y);
            }
        }

        private void RenderText(string text, SDL.SDL_Rect position, string fontPath, SDL.SDL_Color color)
        {
            Console.WriteLine($"Rendering text: {text} at position: {position.x}, {position.y}");
            
            IntPtr font = SDL_ttf.TTF_OpenFont(fontPath, 60);
            IntPtr surfaceMessage = SDL_ttf.TTF_RenderText_Solid(font, text, color);
            IntPtr texture = SDL.SDL_CreateTextureFromSurface(_renderer, surfaceMessage);

            SDL.SDL_RenderCopy(_renderer, texture, IntPtr.Zero, ref position);
            SDL.SDL_DestroyTexture(texture);
            SDL.SDL_FreeSurface(surfaceMessage);
            SDL_ttf.TTF_CloseFont(font);
        }
    }
}