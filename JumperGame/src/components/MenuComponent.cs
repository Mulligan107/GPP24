// src/components/MenuComponent.cs
using System;
using SDL2;


public class MenuComponent
{
    public string Text { get; set; }
    public SDL.SDL_Color Color { get; set; }
    public SDL.SDL_Color SelectedColor { get; set; }
    public Action Action { get; set; }
    public string Font { get; set; }

    public MenuComponent(string text, SDL.SDL_Color color, SDL.SDL_Color selectedColor, Action action, string font)
    {
        Text = text;
        Color = color;
        SelectedColor = selectedColor;
        Action = action;
        Font = font;
    }
}
