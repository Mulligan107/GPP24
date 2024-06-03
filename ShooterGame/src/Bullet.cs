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
            lives = 0;
            texture = tex;

        }

    }
}


