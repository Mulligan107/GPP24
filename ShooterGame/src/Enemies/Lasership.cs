using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShooterGame.ui;

namespace ShooterGame.src
{
    class Lasership : Enemy
    {

        double sinValue = 0;
        double radiusX = 20.0;
        double radiusY = 5.0;

        public Lasership(List<LTexture> textureList) : base(textureList)
        {
            /*
            list.Add("Lasership\\Lasership");
            list.Add("Lasership\\Lasership_death");
            list.Add("Lasership\\Lasership_shield");
            list.Add("Lasership\\Lasership_bullet");

             */

            lives = 1;
            this.textureList = textureList;
            friendly = false;
            width = 90 * s;
            height = 90 * s;
            angle = -90;
            texture = textureList[0];
            overTexture = textureList[2];
            overTexture.setColor(255, 0, 0);
            setupAnimation(36, "shield", textureList[2]);
            setupAnimation(9, "death", textureList[1]);

            onSpawn();
        }

        public override void movementPattern()
        {

            /*
            
            
            if (posX > (Program.SCREEN_WIDTH/8) * 7 ) {
                vecX = -8;
            }
            else
            {
                vecX = 0;
            }

            if (posY > (Program.SCREEN_HEIGHT - height)){
                vecY = -3;
            }
            if (posY < (height))
            {
                vecY = +3;
            }
            */

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
                    animationHelper(3, 4, "shield");
                }
            }
        }

        public override void deathray()
        {
            List<LTexture> list = new List<LTexture>();
            list.Add(textureList[3]); // ANGEPASST AN LASERSHIP
            list.Add(textureList[3]);
            Bullet bill = new Bullet(list, 4);
            bill.texture.setColor(255, 0, 0);
            double s = Program.SCREEN_WIDTH / Program.SCREEN_HEIGHT;
            bill.spawn(posX + width / 4, posY + height / 4);
            bill.angle = -90;
            bill.vecX = -10 * s;
            bill.friendly = false;
            Program.entityList.Add(bill);
        }

    }
}
