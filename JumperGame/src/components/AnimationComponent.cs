using SDL2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TiledCSPlus;

namespace JumperGame.src.components
{
    class AnimationComponent
    {
        public TiledTileAnimation[] AnimimationList { get; set; }
        public int animationFrame { get; set; }
        public SDL.SDL_Rect srcRect { get; set; }
        public int duration { get; set; }
        public double outerTimer { get; set; }

        

        public AnimationComponent(TiledTileAnimation[] animationList, SDL.SDL_Rect src) {
            AnimimationList = animationList;
            duration = AnimimationList[animationFrame].Duration; 
            srcRect = src;
            // Console.WriteLine("Anzahl: " + AnimimationList.Length);
        }

        public SDL.SDL_Rect Update(double time)
        {
            SDL.SDL_Rect loopRect = new SDL.SDL_Rect();
            loopRect = srcRect;
            loopRect.x = srcRect.w * animationFrame;

            return loopRect;
        }
        

    }
   
}
