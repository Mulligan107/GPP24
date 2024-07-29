using System;
using System.Runtime.InteropServices;
using JumperGame.src.manager;
using SDL2;

namespace JumperGame.systems
{
    public class InputSystem
    {
        // This event is triggered whenever a key is pressed.
        public event Action<SDL.SDL_Keycode> KeyPressed;
        public event Action<SDL.SDL_Keycode> KeyReleased;
        public event Action InitializePauseRequested;
        public event Action SelectNextMenuItem;
        public event Action SelectPreviousMenuItem;
        public event Action ExecuteMenuItem;

        public SDL.SDL_Keycode previousKeyPressed;

        public SDL.SDL_Keycode lastKeyPressed;

        bool keyStillDown;


        public void ProcessInput(SDL.SDL_Event e, bool IsMenuOpen)
        {
            if (e.type == SDL.SDL_EventType.SDL_KEYDOWN)
            {

                if (e.key.keysym.sym != previousKeyPressed)
                {
                    lastKeyPressed = e.key.keysym.sym;
                }

                KeyPressed?.Invoke(e.key.keysym.sym);

                if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_ESCAPE)
                {
                    InitializePauseRequested?.Invoke();
                    JumperGame.Instance.IsMenuOpen = true;

                }
                else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_DOWN && IsMenuOpen)
                {
                    SelectNextMenuItem?.Invoke();
                    AudioManager.PlaySound(0);

                }
                else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_UP && IsMenuOpen)
                {
                    SelectPreviousMenuItem?.Invoke();
                }
                else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_RETURN && IsMenuOpen)
                {
                    ExecuteMenuItem?.Invoke();
                }


                /*
                Console.WriteLine("IS // AKTUELL: " + e.key.keysym.sym + "\n" +
                    "last: " + lastKeyPressed + "\n" +
                    "prev: " + previousKeyPressed);
                */

            }
            else if (e.type == SDL.SDL_EventType.SDL_KEYUP)
            {
                //Console.WriteLine("IS // " + isKeyPressed(SDL.SDL_Keycode.SDLK_a) + isKeyPressed(SDL.SDL_Keycode.SDLK_d));
                if (!isKeyPressed(SDL.SDL_Keycode.SDLK_d) && !isKeyPressed(SDL.SDL_Keycode.SDLK_a))
                {
                    KeyReleased?.Invoke(e.key.keysym.sym);
                }
                else
                {
                    SDL.SDL_Keycode key;
                    switch (isKeyPressed(SDL.SDL_Keycode.SDLK_d))
                    {
                        case true:
                            key = SDL.SDL_Keycode.SDLK_d;
                            break;
                            case false:
                            key = SDL.SDL_Keycode.SDLK_a;
                            break;

                    }

                    KeyPressed?.Invoke(key);


                }
                previousKeyPressed = lastKeyPressed;

            }
            
        }

        public bool isKeyPressed(SDL.SDL_Keycode _keycode)
        {
            int arraySize;
            bool isKeyPressed = false;
            IntPtr origArray = SDL.SDL_GetKeyboardState(out arraySize);
            byte[] keys = new byte[arraySize];
            byte keycode = (byte)SDL.SDL_GetScancodeFromKey(_keycode);
            Marshal.Copy(origArray, keys, 0, arraySize);
            isKeyPressed = keys[keycode] == 1;
            return isKeyPressed;
        }
    }
}