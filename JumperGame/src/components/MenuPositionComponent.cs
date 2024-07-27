using SDL2;

namespace JumperGame.components
{
    public class MenuPositionComponent
    {
        public SDL.SDL_Rect Position { get; set; }

        public MenuPositionComponent(SDL.SDL_Rect position)
        {
            Position = position;
        }
    }
}