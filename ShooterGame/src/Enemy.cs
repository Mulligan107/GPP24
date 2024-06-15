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
            friendly = false;
            width = 90 * s;
            height = 90 * s;
            angle = -90;
            texture = textureList[0];
            overTexture = textureList[3];
            setupAnimation(4, "spawn", textureList[1]);
            setupAnimation(9, "death", textureList[2]);
            setupAnimation(10, "shield", textureList[3]); //TODO Automatisieren


            onSpawn();
            setupOverAnimateion();
            
            //   render(tex);
        }

        public void onSpawn()
        {
            startAnimation = true;
            repeats = 3;
            animationCounter = 0;
            frame = 0;
            frameTicker = 0;
            animationFlag = "spawn";

            spawn((Program.SCREEN_WIDTH / 2) * 1.5, Program.SCREEN_HEIGHT / 2);
        }

        public override void hit()
        {
            if (!iframe)
            {
                iframe = true;
                startAnimation = true;
                frame = 0;
                frameTicker = 0;

                lives = lives - 1;

                if (lives < 0) // DEATH
                {
                    repeats = 1;
                    animationCounter = 0;
                    animationSpeed = 2;
                    animationFlag = "death";
                }
                else // HIT
                {
                    repeats = 3;
                    animationCounter = 0;
                    animationSpeed = 4;
                    animationFlag = "shield";
                }
                Console.WriteLine("Lives after: " + lives);
            }
        }

        public void setupOverAnimateion()
        {
            
        }
    }
}


