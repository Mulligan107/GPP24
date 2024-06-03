using System;
using SDL2;

namespace ShooterGame
{
    class Bullet : LivingEntity
    {
        public Bullet(LTexture tex)
        {
            width = 15 * s;
            height = 15 * s;
            lives = 1;
            texture = tex;
            
            vecX = -3; 
            vecY = 0;

        }

    }
}


