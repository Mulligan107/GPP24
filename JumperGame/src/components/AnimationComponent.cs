using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TiledCSPlus;

namespace JumperGame.src.components
{
    class AnimationComponent
    {
        TiledTileAnimation[] AnimimationList { get; set; }

        public AnimationComponent(TiledTileAnimation[] animationList) {
            AnimimationList = animationList;
        }
    }
   
}
