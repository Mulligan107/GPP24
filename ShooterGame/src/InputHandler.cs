using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ShooterGame
{
    class InputHandler
    {
        //Event handler
        static SDL.SDL_Event e;

        public InputHandler() { 
        }

        public static (double, double, int ,string) handleUserInput()
        {
            while (SDL.SDL_PollEvent(out e) != 0)
            {

                //User requests quit via closing the window or pressing esc
                if (e.type == SDL.SDL_EventType.SDL_QUIT || e.key.keysym.sym == SDL.SDL_Keycode.SDLK_ESCAPE)
                {
                    Program.reset = true;
                    Program.quit = true;
                }

                //User requests quit via closing the window or pressing esc
                if (e.type == SDL.SDL_EventType.SDL_QUIT || e.key.keysym.sym == SDL.SDL_Keycode.SDLK_r)
                {
                    Program.reset = true;
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
                    double s = Program.SCREEN_WIDTH / Program.SCREEN_HEIGHT;
                    double bulletspeed = 20;
                    double movementspeed = 8;
                    //Adjust the velocity
                    switch (e.key.keysym.sym)
                    {
                        case SDL.SDL_Keycode.SDLK_UP: return (0, (-bulletspeed * s),0, "shoot");
                        case SDL.SDL_Keycode.SDLK_DOWN: return (0, bulletspeed * s, 180, "shoot");
                        case SDL.SDL_Keycode.SDLK_LEFT: return (-bulletspeed * s, 0,-90, "shoot");
                        case SDL.SDL_Keycode.SDLK_RIGHT: return (bulletspeed * s, 0, 90, "shoot");
                        case SDL.SDL_Keycode.SDLK_w: return (0, -movementspeed * s, 90, "move");
                        case SDL.SDL_Keycode.SDLK_s: return (0, movementspeed * s, 90, "move");
                        case SDL.SDL_Keycode.SDLK_a: return (-movementspeed * s, 0, 90, "move");
                        case SDL.SDL_Keycode.SDLK_d: return (movementspeed * s, 0, 90, "move"); //Keine Rotation für das Schiff

                    }
                }
            }
            return (0, 0, 0, "noAction");
        }
    }



}
