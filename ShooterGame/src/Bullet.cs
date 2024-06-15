using System;
using System.Collections.Generic;
using SDL2;

namespace ShooterGame
{
    class Bullet : LivingEntity
    {
        public Bullet(List<LTexture> textureList)
        {
            width = 45 * s;
            height = 45 * s;
            lives = 0;
            angle = 90;
            this.textureList = textureList;
            texture = textureList[1];

            setupAnimation(4, "move", texture);
            repeats = 999;
            animationCounter = 0;
            animationSpeed = 1;
            animationFlag = "move";
            startAnimation = true;


        }

        public override void hit()
        {
            Console.WriteLine("Lives befor: " + lives);
            lives = lives - 1;
            if (lives < 0)
            {
                kill();
            }
            Console.WriteLine("Lives after: " + lives);
            
        }

    }
}


