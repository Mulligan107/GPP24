using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShooterGame
{
    class InputHandler
    {
        //Event handler
        static SDL.SDL_Event e;

        public InputHandler() { 
        }

        public static (int,int) handleUserInput()
        {
            while (SDL.SDL_PollEvent(out e) != 0)
            {

                //User requests quit via closing the window or pressing esc
                if (e.type == SDL.SDL_EventType.SDL_QUIT || e.key.keysym.sym == SDL.SDL_Keycode.SDLK_ESCAPE)
                {
                    Program.quit = true;
                }

                //Switch screen size mode if 'F' key was pressed
                if (e.type == SDL.SDL_EventType.SDL_KEYDOWN) //ToDo könnte ein switch case sein
                {
                    // Change screen size
                    if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_f)
                    {
                        Program.changeWindowSize();
                    }
                }
                //If a key was pressed
                if (e.type == SDL.SDL_EventType.SDL_KEYDOWN && e.key.repeat == 0)
                {
                    //Adjust the velocity
                    switch (e.key.keysym.sym)
                    {
                        case SDL.SDL_Keycode.SDLK_UP: return (0,-6);
                        case SDL.SDL_Keycode.SDLK_DOWN: return (0, 6);
                        case SDL.SDL_Keycode.SDLK_LEFT: return (-6, 0);
                        case SDL.SDL_Keycode.SDLK_RIGHT: return (6, 0);
                    }
                }
            }
            return (0,0);
        }
    }



}
