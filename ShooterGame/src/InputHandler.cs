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

        public static (int,int, string) handleUserInput()
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

                if (Program.CurrentState != GameState.IN_GAME)
                {
                    if (e.type == SDL.SDL_EventType.SDL_KEYDOWN)
                    {
                        switch (e.key.keysym.sym)
                        {
                            case SDL.SDL_Keycode.SDLK_UP:
                                Program.VisibleMenu.SelectPreviousItem();
                                break;
                            case SDL.SDL_Keycode.SDLK_DOWN:
                                Program.VisibleMenu.SelectNextItem();
                                break;
                            case SDL.SDL_Keycode.SDLK_RETURN:
                                Program.VisibleMenu.ExecuteSelectedItem();
                                break;
                        }
                    }
                }
                
                //If a key was pressed
                if (e.type == SDL.SDL_EventType.SDL_KEYDOWN && e.key.repeat == 0 && Program.CurrentState == GameState.IN_GAME)
                {
                    //Adjust the velocity
                    switch (e.key.keysym.sym)
                    {
                        case SDL.SDL_Keycode.SDLK_UP: return (0,-8, "shoot");
                        case SDL.SDL_Keycode.SDLK_DOWN: return (0, 8, "shoot");
                        case SDL.SDL_Keycode.SDLK_LEFT: return (-8, 0, "shoot");
                        case SDL.SDL_Keycode.SDLK_RIGHT: return (8, 0, "shoot");
                        case SDL.SDL_Keycode.SDLK_w: return (0, -4, "move");
                        case SDL.SDL_Keycode.SDLK_s: return (0, 4, "move");
                        case SDL.SDL_Keycode.SDLK_a: return (-4, 0, "move");
                        case SDL.SDL_Keycode.SDLK_d: return (4, 0, "move");

                    }
                }
            }
            return (0,0,"noAction");
        }
    }



}
