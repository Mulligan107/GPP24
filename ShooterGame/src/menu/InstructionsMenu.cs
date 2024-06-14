namespace ShooterGame
{
    using System;
using SDL2;

namespace ShooterGame
{
    public class InstructionsMenu : Menu
    {
        private string instructionsText1;
        private string instructionsText2;
        private string instructionsText3;
        public InstructionsMenu(IntPtr renderer) : base(renderer)
        {
            // Calculate the height for each menu item
            var menuItemSpacing = Program.SCREEN_HEIGHT / 3; // Divide by the number of menu items +1
            var itemWidth = 200;
            
            // Set the instructions text
            instructionsText1 = "Use arrow keys for shooting and wasd for movement.";
            instructionsText2 = "Press r to reset the level.";
            instructionsText3 = "Press 'esc' to quit the game or pause.";
            
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
            var titlePosition = new Vector2D { X = Program.SCREEN_WIDTH / 2, Y = Program.SCREEN_HEIGHT / 8 }; // Adjust the Y value as needed
            var titleColor = new SDL.SDL_Color { r = 0, g = 0, b = 0, a = 255 }; // White color

            DisplayText("Instructions", titlePosition, 250, "lazy.ttf", renderer, titleColor);

            // Display the instructions text
            var instructionsPosition1 = new Vector2D { X = Program.SCREEN_WIDTH / 2, Y = Program.SCREEN_HEIGHT / 4 }; // Adjust the Y value as needed
            DisplayText(instructionsText1, instructionsPosition1, 400, "lazy.ttf", renderer, titleColor); // Adjust the width as needed

            var instructionsPosition2 = new Vector2D { X = Program.SCREEN_WIDTH / 2, Y = instructionsPosition1.Y + 100 }; // Adjust the Y value as needed
            DisplayText(instructionsText2, instructionsPosition2, 250, "lazy.ttf", renderer, titleColor); // Adjust the width as needed

            var instructionsPosition3 = new Vector2D { X = Program.SCREEN_WIDTH / 2, Y = instructionsPosition2.Y + 80 }; // Adjust the Y value as needed
            DisplayText(instructionsText3, instructionsPosition3, 300, "lazy.ttf", renderer, titleColor); // Adjust the width as needed

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