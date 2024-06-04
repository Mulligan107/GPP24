using System;
using SDL2;

namespace ShooterGame
{
    class Bullet : LivingEntity
    {
        public Bullet(LTexture tex)
        {
            width = 45 * s;
            height = 45 * s;
            lives = 0;
            angle = 90;
            texture = tex;
            
            
        }

    }
}


