using System;
using SDL2;

namespace JumperGame.components
{
    public class MenuRenderComponent
    {
        public IntPtr Texture { get; set; }

        public MenuRenderComponent(IntPtr texture)
        {
            Texture = texture;
        }
    }
}


