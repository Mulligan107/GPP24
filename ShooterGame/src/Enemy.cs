using System;
using System.Collections.Generic;
using SDL2;

namespace ShooterGame
{
    class Enemy : LivingEntity
    {

        
        
        public Enemy(List<LTexture> textureList)
        {
            
            lives = 2;
            this.textureList = textureList;
            
            width = 90 * s;
            height = 90 * s;
            angle = -90;
            texture = textureList[0];
            onSpawn();
            
            //   render(tex);
        }

        public void onSpawn()
        {
            repeats = 3;
            animationCounter = 0;
            choosenAnim = 1;
            setupAnimation(4);
            spawn((Program.SCREEN_WIDTH / 2) * 1.5, Program.SCREEN_HEIGHT / 2);
        }

    }
}


