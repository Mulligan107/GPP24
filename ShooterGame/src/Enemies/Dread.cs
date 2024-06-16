using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShooterGame.ui;

namespace ShooterGame.src
{
    class Dread : Enemy
    {

        double sinValue = 0;
        double radiusX = 20.0;
        double radiusY = 5.0;

        public Dread(List<LTexture> textureList) : base(textureList)
        {
            /*
            list.Add("Dread\\Dread");
            list.Add("Dread\\Dread_shield");
            list.Add("Dread\\Dread_death");
            list.Add("Dread\\Dread_bullet");
            list.Add("Dread\\Ray");
             */

            lives = 4;
            this.textureList = textureList;
            friendly = false;
            width = 200 * s;
            height = 200 * s;
            angle = -90;
            texture = textureList[0];
            overTexture = textureList[1];
            overTexture.setColor(255, 0, 0);
            setupAnimation(20, "shield", textureList[1]);
            setupAnimation(12, "death", textureList[2]);
            
            spawn(Program.SCREEN_WIDTH, Program.SCREEN_HEIGHT/2 - height/2);

            onSpawn();
        }

        public override void movementPattern()
        {

            /*
            sinValue -= (0.025);
            angle = (180 / Math.PI) * (sinValue) + 75;
            vecX = radiusX * s * Math.Cos(sinValue);
            vecY = radiusY * s * Math.Sin(sinValue);
            
            sinValue -= (0.05);
            vecY = radiusY * s * Math.Sin(sinValue);
            vecX = -8;

            if (posX < -width)
            {
                posX = Program.SCREEN_WIDTH;
                posY += Program.SCREEN_HEIGHT/10;
            }
            if (posY > Program.SCREEN_HEIGHT + height)
            {
                posY = height * 2;
            }
            */
            if (posX > (Program.SCREEN_WIDTH/8) * 7 ) {
                vecX = -8;
            }
            else
            {
                vecX = 0;
            }
            
        }

        public override void onSpawn()
        {
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
                    //   SoundHandler.PlaySound(1);  TODO - ERSETZEN fucking earrape
                    ScoreUI.IncreaseScore(100);

                }
                else // HIT
                {
                    animationHelper(20, 4, "shield");
                }
            }
        }
    }
}
