using System;
using SDL2;

namespace ShooterGame
{
    class Bullet : Entity
    {
        public Bullet(LTexture tex)
        {
            lives = 1;
            width = 15;
            height = 15;

            texture = tex;
            
            vecX = -3; 
            vecY = 0;

        }

    }
}


