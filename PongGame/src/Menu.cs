using SDL2;
using System;

namespace PongGame
{
    class Menu
    {
        public LTexture MenuTexture = new LTexture();  
        public LTexture PlayTexture = new LTexture();  
        public LTexture QuitTexture = new LTexture();  
        
        public Menu(int menuWidth, int menuHeight, LTexture menuBackground, LTexture playButton, LTexture quitButton)
        {
            PlayTexture = playButton;
            MenuTexture = menuBackground;
            QuitTexture = quitButton;

            if (PlayTexture == null || MenuTexture == null || QuitTexture == null)
            {
                throw new Exception("Failed to load textures!");
            }
        }
        
        public void RenderMenu()
        {
            //Render background
            MenuTexture.render(0, 0);
            
            //Render buttons
            PlayTexture.render((Program.SCREEN_WIDTH - PlayTexture.GetWidth()) / 2, (Program.SCREEN_HEIGHT - PlayTexture.GetHeight()) / 2);
            QuitTexture.render((Program.SCREEN_WIDTH - QuitTexture.GetWidth()) / 2, (Program.SCREEN_HEIGHT - QuitTexture.GetHeight()) / 2 + 100);
        }
        
        public void HandleInput(SDL.SDL_Event e)
        {
            if (e.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN)
            {
                int x, y;
                SDL.SDL_GetMouseState(out x, out y);

                // Check if Play button was clicked
                if (x > PlayTexture.GetWidth() && x < PlayTexture.GetWidth() + PlayTexture.GetWidth() &&
                    y > PlayTexture.getHeight() && y < PlayTexture.getHeight() + PlayTexture.GetHeight())
                {
                    // Start the game
                }

                // Check if Quit button was clicked
                if (x > QuitTexture.GetWidth() && x < QuitTexture.GetWidth() + QuitTexture.GetWidth() &&
                    y > QuitTexture.getHeight() && y < QuitTexture.getHeight() + QuitTexture.GetHeight())
                {
                    // Quit the application
                }
            }
        }

        public void render()
        {
            SDL.SDL_Rect pannelRect = new SDL.SDL_Rect { x = 0, y=0, w = Program.SCREEN_WIDTH, h = Program.SCREEN_HEIGHT};
            SDL.SDL_RenderCopy(Program.menuRenderer, MenuTexture.getTexture(), IntPtr.Zero, ref pannelRect);
        }
    }
}