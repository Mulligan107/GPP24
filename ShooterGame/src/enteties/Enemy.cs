﻿using System;
using System.Collections;
using System.Collections.Generic;
using SDL2;
using ShooterGame.ui;

namespace ShooterGame
{
    class Enemy : LivingEntity
    {
        public static int TotalEnemies { get; set; }

        public Enemy(List<LTexture> textureList)
        {
            TotalEnemies++;
            onSpawn();
        }
        public override void move(double deltaTime)
        {
            movementPattern(deltaTime);
            posX += vecX * (deltaTime / 10) * speed;
            posY += vecY * (deltaTime / 10) * speed;
        }

        public virtual void movementPattern(double delta)
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

        public override void shootEnemy(int flipX, double delta)
        {
            Random rand = new Random();
            int randomIndex = rand.Next(0, 3);
            int[] soundIndices = {5, 8, 9};
            int soundToPlay = soundIndices[randomIndex];
            
            List<LTexture> list = new List<LTexture>();
            list.Add(textureList[4]); // ANGEPASST AN FIGHTER
            list.Add(textureList[4]);
            Bullet bill = new Bullet(list, 10);
            bill.texture.setColor(255,0,0);
            double s = Program.SCREEN_WIDTH / Program.SCREEN_HEIGHT;
            bill.spawn(posX + width / 4, posY + height / 4);
            bill.angle = -90;
            bill.vecX = -15 * s * flipX * (delta / 10);
            bill.vecY = 0;
            bill.friendly = false;
            Program.entityList.Add(bill);
            
            SoundHandler.PlaySound(soundToPlay);
        }

        public override void bulletFan(double delta)
        {
            double start = -5;
            double end = 5;
            int numSteps = 8;

            for (int i = 1; i < numSteps+1; i++)
            {
                Console.WriteLine(i);
                List<LTexture> list = new List<LTexture>();
                list.Add(textureList[3]); //ANGEPASST AN DREAD
                list.Add(textureList[3]);

                double stepSize = (end - start) / (numSteps - 1);

                Bullet bill = new Bullet(list, 10);
                bill.texture.setColor(255, 0, 0);
                double s = Program.SCREEN_WIDTH / Program.SCREEN_HEIGHT;
                bill.spawn(posX + width / 4, posY + height / 4);
                bill.angle = -90;
                bill.speed = 0.5 * (delta / 10);
                bill.vecX = -15 * s * (delta / 10);
                bill.vecY = (start + i) * stepSize * s * (delta / 10);
                bill.friendly = false;
                Program.entityList.Add(bill);
            }
        }

    }
}


