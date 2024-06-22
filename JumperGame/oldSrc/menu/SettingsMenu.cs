using System;
using System.Collections.Generic;
using SDL2;

namespace ShooterGame
{
    public class SettingsMenu : Menu
    {
        private bool _isMusicPlaying;
        private bool _isSoundMuted = true;
        
        public SettingsMenu(IntPtr renderer) : base(renderer)
        {
            MenuItems = new List<MenuItem>();

            // Calculate the height for each menu item
            var menuItemSpacing = Program.SCREEN_HEIGHT / 10; 
            var initialYPosition = Program.SCREEN_HEIGHT / 4; // Adjust this value as needed
            var itemWidth = 200;

            // Create the settings items with their positions
            var changeWindowSizeItem = new MenuItem("Change Window Size", () => { Program.changeWindowSize(); },
                "Change Window Size", "lazy.ttf",
                new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = initialYPosition, w = itemWidth, h = 50 },
                new SDL.SDL_Color { r = 255, g = 255, b = 0, a = 255 }); //color when selected
            
            var toggleMusicItem = new MenuItem("Start Music", () => { ToggleMusic(); },
                "Start Music", "lazy.ttf",
                new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = initialYPosition + menuItemSpacing, w = itemWidth, h = 50 },
                new SDL.SDL_Color { r = 255, g = 255, b = 0, a = 255 });
            
            var toggleSoundItem = new MenuItem("Resume Sounds", ToggleSound,
                "Resume Sounds", "lazy.ttf",
                new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = initialYPosition + 2 * menuItemSpacing, w = itemWidth, h = 50 },
                new SDL.SDL_Color { r = 255, g = 255, b = 0, a = 255 });
            
            var toggleDebugModeItem = new MenuItem("Enable Debug Mode", () => { ToggleDebugMode(); },
                "Enable Debug Mode", "lazy.ttf",
                new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = initialYPosition + 3 * menuItemSpacing, w = itemWidth, h = 50 },
                new SDL.SDL_Color { r = 255, g = 255, b = 0, a = 255 });
            
            var toggleDifficulty = new MenuItem("Dificulty Toggle", () => { ToggleDifficulty(); },
                "Medium Difficulty", "lazy.ttf",
                new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = initialYPosition + 4 * menuItemSpacing, w = itemWidth, h = 50 },
                new SDL.SDL_Color { r = 255, g = 255, b = 0, a = 255 });

            var backItem = new MenuItem("Back", () =>
                {
                    Program.CurrentState = GameState.MAIN_MENU;
                    Program.VisibleMenu = new MainMenu(renderer);
                    SDL.SDL_SetRenderDrawColor(Program.gRenderer, 0xFF, 0xFF, 0xFF, 0xFF);
                    SDL.SDL_RenderClear(Program.gRenderer); // Clear the current rendering target with the drawing color
                },
                "Back", "lazy.ttf",
                new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = initialYPosition + 5 * menuItemSpacing, w = itemWidth, h = 50 },
                new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255 });

            AddMenuItem(changeWindowSizeItem);
            AddMenuItem(toggleMusicItem);
            AddMenuItem(toggleSoundItem);
            AddMenuItem(toggleDebugModeItem);
            AddMenuItem(toggleDifficulty);
            AddMenuItem(backItem);
        }
        
        private void ToggleMusic()
        {
            if (_isMusicPlaying)
            {
                SoundHandler.StopMusic();
                _isMusicPlaying = false;
                MenuItems[1].Text = "Start Music";
            }
            else
            {
                SoundHandler.PlayMusic();
                _isMusicPlaying = true;
                MenuItems[1].Text = "Stop Music";
            }
        }

        private void ToggleSound()
        {
            if (_isSoundMuted)
            {
                SoundHandler.SoundVolume = 1;
                SoundHandler.LoadMedia();
                _isSoundMuted = false;
                MenuItems[2].Text = "Mute Sounds";
            }
            else
            {
                SoundHandler.SoundVolume = 0;
                SoundHandler.LoadMedia();
                _isSoundMuted = true;
                MenuItems[2].Text = "Resume Sounds";
            }
        }
        
        private void ToggleDebugMode()
        {
            if (Program.debugMode)
            {
                Program.debugMode = false;
                MenuItems[3].Text = "Enable Debug Mode"; 
            }
            else
            {
                Program.debugMode = true;
                MenuItems[3].Text = "Disable Debug Mode"; 
            }
        }
        
        private void ToggleDifficulty()
        {
            // Get the current value of Player.lifes
            var currentLifes = Player.lifes;

            // Switch between the three difficulty levels
            if (currentLifes == 100) // Currently easy, switch to medium
            {
                Player.lifes = 20;
                MenuItems[4].Text = "Medium Difficulty";
            }
            else if (currentLifes == 20) // Currently medium, switch to hard
            {
                Player.lifes = 10;
                MenuItems[4].Text = "Hard Difficulty";
            }
            else // Currently hard, switch to easy
            {
                Player.lifes = 100;
                MenuItems[4].Text = "Easy Difficulty";
            }
        }

        public override void Render(IntPtr renderer)
        {
            var titlePosition = new Vector2D { X = Program.SCREEN_WIDTH / 2, Y = Program.SCREEN_HEIGHT / 8 }; // Adjust the Y value as needed
            var titleColor = new SDL.SDL_Color { r = 0, g = 0, b = 0, a = 255 }; // White color
            DisplayText("Settings", titlePosition, 250, "lazy.ttf", renderer, titleColor); // Adjust the width as needed

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