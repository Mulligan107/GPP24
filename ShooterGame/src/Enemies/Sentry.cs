﻿using ShooterGame.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShooterGame.src
{
    class Sentry : Enemy
    {
        double sinValue = 0;

        public Sentry(List<LTexture> textureList) : base(textureList)
        {
            lives = 2;
            this.textureList = textureList;
            friendly = false;
            width = 90 * s;
            height = 90 * s;
            angle = -90;
            texture = textureList[0];
            overTexture = textureList[3];
            overTexture.setColor(255, 0, 0);
            setupAnimation(4, "spawn", textureList[1]);
            setupAnimation(7, "death", textureList[2]);
            setupAnimation(20, "shield", textureList[3]); //TODO Automatisieren


            onSpawn();
        }

        public override void movementPattern()
        {
            sinValue += (0.05); // länge der Amplitude
            vecY = Math.Sin(sinValue);
        }

        public override void onSpawn()
        {
            animationHelper(3, 1, "spawn");
        }


        public override void hit()
        {
            if (!iframe)
            {
                iframe = true;

                lives = lives - 1;

                if (lives < 0) // DEATH
                {
                    animationHelper(1, 2, "death");

                    TotalEnemies--;

                    ScoreUI.IncreaseScore(100);

                }
                else // HIT
                {
                    SoundHandler.PlaySound(4);
                    
                    animationHelper(3, 4, "shield");
                }

            }
        }
    }
}
