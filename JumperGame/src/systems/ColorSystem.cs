using JumperGame.src.components.testComponents;
using SDL2;

namespace JumperGame.systems
{
    public class ColorSystem
    {
        private ColorComponent _colorComponent;

        public ColorSystem(ColorComponent colorComponent)
        {
            _colorComponent = colorComponent;
        }

        public void ChangeColor(SDL.SDL_Keycode keycode)
        {
            switch (keycode)
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
            }
        }
    }
}