using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static SDL2.SDL;

namespace JumperGame.src.manager
{
    internal class AnimationManager
    {
        public LTexture Rendertexture { get; set; }
        public SDL.SDL_Rect srcRect;
        public int duration;
        public int animationID;

        public AnimationManager()
        {
            
        }

        public void UpdateSrcRect()
        {
            
        }
    }
}
