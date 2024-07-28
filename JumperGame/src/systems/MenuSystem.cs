
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        
        public void StartLevel(string levelName, int initialLifeCount)
        {
            if (!JumperGame.Instance.LevelProgressionSystem.IsLevelUnlocked(levelName))
            {
                Console.WriteLine($"Level {levelName} is locked!");
                return;
            }

            JumperGame.Instance.CurrentLevel = levelName;

            CoinCounterSystem.Instance.ResetCoinCount();
            JumperGame.Instance.LifeSystem.LifeCount = initialLifeCount;
            JumperGame.Instance.LifeSystem.IsGameOverTriggered = false;
            JumperGame.Instance.LoadLevel(levelName);
            JumperGame.Instance.IsMenuOpen = false;
            ClearMenuItems();
        }

        public void LoadNextLevel()
        {
            string currentLevel = JumperGame.Instance.CurrentLevel;
            string nextLevel = currentLevel switch
            {
                "Level1" => "Level2",
                "Level2" => "Level3",
                _ => "Level1" // Loop back to Level1 or handle as needed
            };

            // Mark the current level as completed
            JumperGame.Instance.LevelProgressionSystem.MarkLevelAsCompleted(currentLevel);

            // Unlock the next level
            unlockLevel(nextLevel);

            // Start the next level
            StartLevel(nextLevel, JumperGame.Instance.LifeSystem.LifeCount);
        }
        
        public void unlockLevel(string levelName)
        {
            JumperGame.Instance.LevelProgressionSystem.MarkLevelAsCompleted(levelName);
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