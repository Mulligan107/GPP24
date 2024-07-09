using System;
using SDL2;

namespace JumperGame.systems
{
    public class InputSystem
    {
        // This event is triggered whenever a key is pressed.
        public event Action<SDL.SDL_Keycode> KeyPressed;
        public event Action<SDL.SDL_Keycode> KeyReleased;
        public event Action GameQuitRequested;
        
        public void ProcessInput(SDL.SDL_Event e)
        {
            if (e.type == SDL.SDL_EventType.SDL_KEYDOWN)
            {
                KeyPressed?.Invoke(e.key.keysym.sym);

                if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_ESCAPE)
                {
                    GameQuitRequested?.Invoke();
                }
            }
            else if (e.type == SDL.SDL_EventType.SDL_KEYUP)
            {
                KeyReleased?.Invoke(e.key.keysym.sym);
            }
        }
    }
}