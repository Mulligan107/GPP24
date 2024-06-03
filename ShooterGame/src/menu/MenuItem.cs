using System;
using SDL2;

namespace ShooterGame
{
    public class MenuItem
    {
        public string Label { get; set; }
        public Action Action { get; set; }
        public IntPtr Texture { get; set; }
        public SDL.SDL_Rect Position { get; set; }
        public SDL.SDL_Color SelectedColor { get; set; } 

        public MenuItem(string label, Action action, IntPtr texture, SDL.SDL_Rect position, SDL.SDL_Color selectedColor)
        {
            Label = label;
            Action = action;
            Texture = texture;
            Position = position;
            SelectedColor = selectedColor;
        }
    }
}