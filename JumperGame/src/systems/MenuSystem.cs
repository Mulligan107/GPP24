
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
        
        public void ClearMenuItems()
        {
            _menuItems.Clear();
            _selectedIndex = 0;
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
            if (_menuItems.Count > 0)
            {
                _menuItems[_selectedIndex].MenuComponent.Action();
            }
        }
        
        public void resume()
        {
            JumperGame.Instance.IsMenuOpen = false;
            ClearMenuItems();
        }
        
        public void StartLevel1()
        {
            CoinCounterSystem.Instance.ResetCoinCount();
            LifeSystem.Instance.LifeCount = 3;
            LifeSystem.Instance.IsGameOverTriggered = false;
            JumperGame.Instance.LoadLevel("Level1");
            JumperGame.Instance.IsMenuOpen = false;
            ClearMenuItems();
        }
        
        public void StartLevel2()
        {
            CoinCounterSystem.Instance.ResetCoinCount();
            LifeSystem.Instance.LifeCount = 5;
            LifeSystem.Instance.IsGameOverTriggered = false;
            JumperGame.Instance.LoadLevel("Level2");
            JumperGame.Instance.IsMenuOpen = false;
            ClearMenuItems();
        }
        
        public void StartLevel3()
        {
            CoinCounterSystem.Instance.ResetCoinCount();
            LifeSystem.Instance.LifeCount = 10;
            LifeSystem.Instance.IsGameOverTriggered = false;
            JumperGame.Instance.LoadLevel("movementTest");
            JumperGame.Instance.IsMenuOpen = false;
            ClearMenuItems();
        }

        public void ExitGame()
        {
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