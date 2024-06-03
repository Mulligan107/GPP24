using System;
using System.Collections.Generic;
using SDL2;

namespace ShooterGame
{
    class Enemy : LivingEntity
    {

        
        
        public Enemy(List<LTexture> textureList)
        {
            
            lives = 4;
            this.textureList = textureList;
            
            width = 30 * s;
            height = 30 * s;
            texture = textureList[0];
            onSpawn();
            
            //   render(tex);
        }

        public void onSpawn()
        {
            repeats = 3;
            setupAnimation(4, 1);
            spawn((Program.SCREEN_WIDTH / 2) * 1.5, Program.SCREEN_HEIGHT / 2);
        }

    }
}


