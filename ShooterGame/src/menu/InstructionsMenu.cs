namespace ShooterGame
{
    using System;
using SDL2;

namespace ShooterGame
{
    public class InstructionsMenu : Menu
    {
        private string instructionsTextWASD;
        private string instructionsTextR;
        private string InstructionsTextP;
        private string instructionsTextEsc;
        public InstructionsMenu(IntPtr renderer) : base(renderer)
        {
            // Calculate the height for each menu item
            var menuItemSpacing = Program.SCREEN_HEIGHT / 3; // Divide by the number of menu items +1
            var itemWidth = 200;
            
            // Set the instructions text
            instructionsTextWASD = "Use arrow keys for shooting and wasd for movement.";
            instructionsTextR = "Press r to reset the level.";
            InstructionsTextP = "Press p to pause the game.";
            instructionsTextEsc = "Press esc to quit the game.";
            
            var backItem = new MenuItem("Back", () =>
                {
                    Program.CurrentState = GameState.MAIN_MENU;
                    Program.VisibleMenu = new MainMenu(renderer);
                    SDL.SDL_SetRenderDrawColor(Program.gRenderer, 0xFF, 0xFF, 0xFF, 0xFF);
                    SDL.SDL_RenderClear(Program.gRenderer); // Clear the current rendering target with the drawing color
                },
                "Back", "lazy.ttf",
                new SDL.SDL_Rect { x = Program.SCREEN_WIDTH / 2, y = 2 * menuItemSpacing, w = itemWidth, h = 50 },
                new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255 });
            
            AddMenuItem(backItem);
        }

        public override void Render(IntPtr renderer)
        {
            var titlePosition = new Vector2D { X = Program.SCREEN_WIDTH / 2, Y = Program.SCREEN_HEIGHT / 8 };
            var titleColor = new SDL.SDL_Color { r = 255, g = 255, b = 255, a = 255 }; // White color

            DisplayText("Instructions", titlePosition, 250, "lazy.ttf", renderer, titleColor);

            // Display the instructions text
            var instructionsPosition1 = new Vector2D { X = Program.SCREEN_WIDTH / 2, Y = Program.SCREEN_HEIGHT / 4 };
            DisplayText(instructionsTextWASD, instructionsPosition1, 500, "lazy.ttf", renderer, titleColor); // Adjust the width as needed

            var instructionsPosition2 = new Vector2D { X = Program.SCREEN_WIDTH / 2, Y = instructionsPosition1.Y + 100 }; 
            DisplayText(instructionsTextR, instructionsPosition2, 250, "lazy.ttf", renderer, titleColor); 
            
            var instructionsPosition3 = new Vector2D { X = Program.SCREEN_WIDTH / 2, Y = instructionsPosition2.Y + 80 };
            DisplayText(InstructionsTextP, instructionsPosition3, 250, "lazy.ttf", renderer, titleColor);

            var instructionsPosition4 = new Vector2D { X = Program.SCREEN_WIDTH / 2, Y = instructionsPosition3.Y + 80 }; 
            DisplayText(instructionsTextEsc, instructionsPosition4, 300, "lazy.ttf", renderer, titleColor); 

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
}