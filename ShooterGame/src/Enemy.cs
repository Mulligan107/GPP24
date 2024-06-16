using System;
using System.Collections.Generic;
using SDL2;
using ShooterGame.ui;

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
            
            //   render(tex);
        }

        public void onSpawn()
        {
            animationHelper(3, 1, "spawn");

            spawn((Program.SCREEN_WIDTH / 2) * 1.5, Program.SCREEN_HEIGHT / 2);
        }

        public void animationHelper(int reps, int aniSpeed, string flag)
        {
            startAnimation = true;
            frame = 0;
            frameTicker = 0;
            animationCounter = 0;
            repeats = reps;
            animationSpeed = aniSpeed;
            animationFlag = flag;

        }

        public override void hit()
        {
            if (!iframe)
            {
                iframe = true;
                
                lives = lives - 1;

                if (lives < 0) // DEATH
                {
                    animationHelper(1,2,"death");
                    
                 //   SoundHandler.PlaySound(1);  TODO - ERSETZEN fucking earrape
                    
                    ScoreUI.IncreaseScore(100);
        
                }
                else // HIT
                {
                    animationHelper(3, 4, "shield");
                }
                Console.WriteLine("Lives after: " + lives);
            }
        }

        public override void shootEnemy()
        {
            List<LTexture> list = new List<LTexture>();
            list.Add(textureList[4]);
            list.Add(textureList[4]);
            Bullet bill = new Bullet(list, 10);
            bill.texture.setColor(255,0,0);
            double s = Program.SCREEN_WIDTH / Program.SCREEN_HEIGHT;
            bill.spawn(posX + width / 4, posY + height / 4);
            bill.angle = -90;
            bill.vecX = -15 * s;
            bill.vecY = 0;
            bill.friendly = false;
            Program.entityList.Add(bill);
        }
    }
}


