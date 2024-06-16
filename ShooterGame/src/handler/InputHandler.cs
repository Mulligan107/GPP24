using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShooterGame.ShooterGame;
using static System.Net.Mime.MediaTypeNames;

namespace ShooterGame
{
    class InputHandler
    {
        //Event handler
        private static SDL.SDL_Event _e;
        
        // Set of keys currently being pressed
        static HashSet<SDL.SDL_Keycode> pressedKeys = new HashSet<SDL.SDL_Keycode>();


        public InputHandler() { 
            // Load the sound files
            SoundHandler.LoadMedia();
        }

        public static (double, double, int ,string) handleUserInput()
        {
            double s = Program.SCREEN_WIDTH / Program.SCREEN_HEIGHT;
            
            while (SDL.SDL_PollEvent(out _e) != 0)
            {
                // SDL_KEYDOWN ARE SDL_KEYUP ARE IMPORTANT FOR PLAYER MOVEMENT!!!
                // Handle key press event
                if (_e.type == SDL.SDL_EventType.SDL_KEYDOWN)
                {
                    pressedKeys.Add(_e.key.keysym.sym);
                }

                // Handle key release event
                if (_e.type == SDL.SDL_EventType.SDL_KEYUP)
                {
                    pressedKeys.Remove(_e.key.keysym.sym);
                }

                //User requests quit via closing the window or pressing esc
                if (_e.type == SDL.SDL_EventType.SDL_QUIT || _e.key.keysym.sym == SDL.SDL_Keycode.SDLK_ESCAPE)
                {
                    Program.reset = true;
                    Program.quit = true;
                }

                //User requests quit via closing the window or pressing esc
                if (_e.type == SDL.SDL_EventType.SDL_QUIT || _e.key.keysym.sym == SDL.SDL_Keycode.SDLK_r)
                {
                    Program.reset = true;
                }
                
                //User requests pause via pressing 'p'
                if (_e.key.keysym.sym == SDL.SDL_Keycode.SDLK_p)
                {
                    if (Program.CurrentState == GameState.IN_GAME)
                    {
                        Program.CurrentState = GameState.PAUSED;
                        Program.VisibleMenu = new PauseMenu(Program.gRenderer);
                    }
                }

                //Switch screen size mode if 'F' key was pressed
                // if (e.type == SDL.SDL_EventType.SDL_KEYDOWN) //ToDo könnte ein switch case sein
                // {
                //     // Change screen size
                //     if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_f)
                //     {
                //         Program.changeWindowSize();
                //     }
                // }

                if (Program.CurrentState != GameState.IN_GAME)
                {
                    if (_e.type == SDL.SDL_EventType.SDL_KEYDOWN)
                    {
                        switch (_e.key.keysym.sym)
                        {
                            case SDL.SDL_Keycode.SDLK_UP:
                                Program.VisibleMenu.SelectPreviousItem();
                                SoundHandler.PlaySound(0);
                                break;
                            case SDL.SDL_Keycode.SDLK_DOWN:
                                Program.VisibleMenu.SelectNextItem();
                                SoundHandler.PlaySound(0);
                                break;
                            case SDL.SDL_Keycode.SDLK_RETURN:
                                Program.VisibleMenu.ExecuteSelectedItem();
                                SoundHandler.PlaySound(0);
                                break;
                        }
                    }
                }
                
                //If a key was pressed
                if (_e.type == SDL.SDL_EventType.SDL_KEYDOWN && _e.key.repeat == 0 && Program.CurrentState == GameState.IN_GAME)
                {
                    double bulletspeed = 20;
                    //double movementspeed = 32;
                    //Adjust the velocity
                    switch (_e.key.keysym.sym)
                    {
                        case SDL.SDL_Keycode.SDLK_UP: return (0, (-bulletspeed * s),0, "shoot");
                        case SDL.SDL_Keycode.SDLK_DOWN: return (0, bulletspeed * s, 180, "shoot");
                        case SDL.SDL_Keycode.SDLK_LEFT: return (-bulletspeed * s, 0,-90, "shoot");
                        case SDL.SDL_Keycode.SDLK_RIGHT: return (bulletspeed * s, 0, 90, "shoot");
                        // case SDL.SDL_Keycode.SDLK_w: return (0, -movementspeed * s, 90, "move");
                        // case SDL.SDL_Keycode.SDLK_s: return (0, movementspeed * s, 90, "move");
                        // case SDL.SDL_Keycode.SDLK_a: return (-movementspeed * s, 0, 90, "move");
                        // case SDL.SDL_Keycode.SDLK_d: return (movementspeed * s, 0, 90, "move"); //Keine Rotation für das Schiff
                
                    }
                }
            }
            // Check for movement keys and return corresponding movement
            double movementspeed = 16;
            //double bulletspeed = 20;

            if (pressedKeys.Contains(SDL.SDL_Keycode.SDLK_w)) return (0, -movementspeed * s, 90, "move");
            if (pressedKeys.Contains(SDL.SDL_Keycode.SDLK_s)) return (0, movementspeed * s, 90, "move");
            if (pressedKeys.Contains(SDL.SDL_Keycode.SDLK_a)) return (-movementspeed * s, 0, 90, "move");
            if (pressedKeys.Contains(SDL.SDL_Keycode.SDLK_d)) return (movementspeed * s, 0, 90, "move");

            // // Check for shooting keys and return corresponding command
            // if (pressedKeys.Contains(SDL.SDL_Keycode.SDLK_UP)) return (0, -bulletspeed * s, 0, "shoot");
            // if (pressedKeys.Contains(SDL.SDL_Keycode.SDLK_DOWN)) return (0, bulletspeed * s, 180, "shoot");
            // if (pressedKeys.Contains(SDL.SDL_Keycode.SDLK_LEFT)) return (-bulletspeed * s, 0, -90, "shoot");
            // if (pressedKeys.Contains(SDL.SDL_Keycode.SDLK_RIGHT)) return (bulletspeed * s, 0, 90, "shoot");

            return (0, 0, 0, "noAction");
        }
    }



}
