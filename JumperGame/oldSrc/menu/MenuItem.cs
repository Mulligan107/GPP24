using System;
using SDL2;

namespace ShooterGame
{
    public class MenuItem
    {
        public string Label { get; set; }
        public Action Action { get; set; }
        public string Text { get; set; }
        public string Font { get; set; }
        public SDL.SDL_Rect Position { get; set; }
        public SDL.SDL_Color SelectedColor { get; set; }

        public MenuItem(string label, Action action, string text, string font, SDL.SDL_Rect position, SDL.SDL_Color selectedColor)
        {
            Label = label;
            Action = action;
            Text = text;
            Font = font;
            Position = position;
            SelectedColor = selectedColor;
        }
    }
    
    public class Vector2D
    {
        public float X { get; set; }
        public float Y { get; set; }
    }
}