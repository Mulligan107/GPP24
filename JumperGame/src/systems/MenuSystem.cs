
using System;
using System.Collections.Generic;
using JumperGame.components;
using JumperGame.gameEntities;
using SDL2;

namespace JumperGame.systems
{
    public class MenuSystem
    {
        private List<MenuItemEntity> _menuItems = new();
        private int _selectedIndex;

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
        
        public void InitializeMenu(MenuSystem menuSystem)
        {
            var menuItem1 = new MenuItemEntity(
                new MenuComponent("Start Level 1", new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 }, new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255 }, StartLevel1, "lazy.ttf"),
                new MenuPositionComponent(new SDL.SDL_Rect { x = 200, y = 10, w = 200, h = 50 })
            );

            var menuItem2 = new MenuItemEntity(
                new MenuComponent("Start Level 2", new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 }, new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255 }, StartLevel2, "lazy.ttf"),
                new MenuPositionComponent(new SDL.SDL_Rect { x = 200, y = 20, w = 200, h = 50 })
            );
            
            var menuItem3 = new MenuItemEntity(
                new MenuComponent("Exit", new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 }, new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255 }, ExitGame, "lazy.ttf"),
                new MenuPositionComponent(new SDL.SDL_Rect { x = 200, y = 20, w = 200, h = 50 })
            );

            menuSystem.AddMenuItem(menuItem1);
            menuSystem.AddMenuItem(menuItem2);
            menuSystem.AddMenuItem(menuItem3);
        }
        
        private void StartLevel1()
        {
            JumperGame.Instance.LoadLevel("Level1");
        }
        
        private void StartLevel2()
        {
            JumperGame.Instance.LoadLevel("Level2");
        }

        private void ExitGame()
        {
            Console.WriteLine("Exit Game selected");
            Environment.Exit(0);
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
    }
}