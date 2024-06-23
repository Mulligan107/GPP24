using JumperGame.src.components.testComponents;
using SDL2;

namespace JumperGame.src.components
{
    internal class InputComponent
    {
        private ColorComponent _colorComponent;
        private JumperGame _game;


        public InputComponent(ColorComponent colorComponent, JumperGame game) // Modify this line
        {
            _colorComponent = colorComponent;
            _game = game;
        }

        public void ProcessInput(SDL.SDL_Event e)
        {
            if (e.type == SDL.SDL_EventType.SDL_KEYDOWN)
            {
                switch (e.key.keysym.sym)
                {
                    case SDL.SDL_Keycode.SDLK_UP:
                        _colorComponent.CurrentColor = new SDL.SDL_Color { r = 255, g = 0, b = 0, a = 255 }; // Red for Up
                        break;
                    case SDL.SDL_Keycode.SDLK_DOWN:
                        _colorComponent.CurrentColor = new SDL.SDL_Color { r = 0, g = 255, b = 0, a = 255 }; // Green for Down
                        break;
                    case SDL.SDL_Keycode.SDLK_LEFT:
                        _colorComponent.CurrentColor = new SDL.SDL_Color { r = 0, g = 0, b = 255, a = 255 }; // Blue for Left
                        break;
                    case SDL.SDL_Keycode.SDLK_RIGHT:
                        _colorComponent.CurrentColor = new SDL.SDL_Color { r = 255, g = 255, b = 0, a = 255 }; // Yellow for Right
                        break;
                    case SDL.SDL_Keycode.SDLK_ESCAPE:
                        _game.IsRunning = false;
                        break;
                }
            }
        }
    }
}
