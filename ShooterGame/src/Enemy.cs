﻿using System;
using System.Collections.Generic;
using SDL2;
using ShooterGame.ui;

namespace ShooterGame
{
    class Enemy : LivingEntity
    {
        public static int TotalEnemies { get; protected set; }

        public Enemy(List<LTexture> textureList)
        {
            TotalEnemies++;
            onSpawn();
        }
        public override void move(double deltaTime)
        {
            movementPattern();
            posX += vecX * (deltaTime / 10) * speed;
            posY += vecY * (deltaTime / 10) * speed;
        }

        public virtual void movementPattern()
        {
        }

        public virtual void onSpawn()
        {
            animationHelper(3, 1, "spawn");
            iframe = true;

            spawn((Program.SCREEN_WIDTH / 2) * 1.5, Program.SCREEN_HEIGHT / 2);
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
            Random rand = new Random();
            int randomIndex = rand.Next(0, 3);
            int[] soundIndices = {5, 8, 9};
            int soundToPlay = soundIndices[randomIndex];
            
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
            
            SoundHandler.PlaySound(soundToPlay);
        }
    }
}


