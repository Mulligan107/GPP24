using ShooterGame.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShooterGame.src
{
    class Fighter : Enemy
    {
        double sinValue = 0;

        public Fighter(List<LTexture> textureList) : base(textureList)
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
    }
}
