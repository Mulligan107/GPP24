using System;
using SDL2;

namespace ShooterGame
{
    class Enemy : LivingEntity
    {

        
        public Enemy(LTexture tex)
        {
            
            lives = 4;
            spawn((Program.SCREEN_WIDTH / 8) , Program.SCREEN_HEIGHT / 2 );
            width = 30 * s;
            height = 30 * s;
            texture = tex;
            //   render(tex);

        }

    }
}


