using System;
using System.Runtime.InteropServices;
using SDL2;

namespace JumperGame.systems
{
    public class InputSystem
    {
        // This event is triggered whenever a key is pressed.
        public event Action<SDL.SDL_Keycode> KeyPressed;
        public event Action<SDL.SDL_Keycode> KeyReleased;
        public event Action GameQuitRequested;
        public event Action SelectNextMenuItem;
        public event Action SelectPreviousMenuItem;
        public event Action ExecuteMenuItem;

        public SDL.SDL_Keycode PreviousKeyPressed;

        public SDL.SDL_Keycode LastKeyPressed;


        public void ProcessInput(SDL.SDL_Event e)
        {
            switch (e.type)
            {
                case SDL.SDL_EventType.SDL_KEYDOWN:
                {
                    if (e.key.keysym.sym != PreviousKeyPressed)
                    {
                        LastKeyPressed = e.key.keysym.sym;
                    }

                    KeyPressed?.Invoke(e.key.keysym.sym);

                    switch (e.key.keysym.sym)
                    {
                        case SDL.SDL_Keycode.SDLK_ESCAPE:
                            GameQuitRequested?.Invoke();
                            break;
                        case SDL.SDL_Keycode.SDLK_DOWN:
                            SelectNextMenuItem?.Invoke();
                            break;
                        case SDL.SDL_Keycode.SDLK_UP:
                            SelectPreviousMenuItem?.Invoke();
                            break;
                        case SDL.SDL_Keycode.SDLK_RETURN:
                            ExecuteMenuItem?.Invoke();
                            break;
                    }


                    /*
                Console.WriteLine("IS // AKTUELL: " + e.key.keysym.sym + "\n" +
                    "last: " + lastKeyPressed + "\n" +
                    "prev: " + previousKeyPressed);
                */
                    break;
                }
                case SDL.SDL_EventType.SDL_KEYUP:
                {
                    //Console.WriteLine("IS // " + isKeyPressed(SDL.SDL_Keycode.SDLK_a) + isKeyPressed(SDL.SDL_Keycode.SDLK_d));
                    if (!IsKeyPressed(SDL.SDL_Keycode.SDLK_d) && !IsKeyPressed(SDL.SDL_Keycode.SDLK_a))
                    {
                        KeyReleased?.Invoke(e.key.keysym.sym);
                    }
                    else
                    {
                        SDL.SDL_Keycode key;
                        switch (IsKeyPressed(SDL.SDL_Keycode.SDLK_d))
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
                    PreviousKeyPressed = LastKeyPressed;
                    break;
                }
            }
        }

        public bool IsKeyPressed(SDL.SDL_Keycode _keycode)
        {
            int arraySize;
            bool isKeyPressed;
            IntPtr origArray = SDL.SDL_GetKeyboardState(out arraySize);
            byte[] keys = new byte[arraySize];
            byte keycode = (byte)SDL.SDL_GetScancodeFromKey(_keycode);
            Marshal.Copy(origArray, keys, 0, arraySize);
            isKeyPressed = keys[keycode] == 1;
            return isKeyPressed;
        }
    }
}