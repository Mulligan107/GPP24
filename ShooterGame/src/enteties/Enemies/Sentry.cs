using ShooterGame.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShooterGame.src
{
    class Sentry : Enemy
    {

        double tempvecX;
        double tempvecY;

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
            setupAnimation(8, "death", textureList[2]);
            setupAnimation(20, "shield", textureList[3]); //TODO Automatisieren


            onSpawn();
        }

        public override void movementPattern()
        {
            Player pepe = (Player)Program.entityList[0];
            tempvecX = pepe.posX - posX;
            tempvecY = pepe.posY - posY;
            double angleToXAxis = Math.Atan2(tempvecY, tempvecX);

            double angleToVertical = Math.PI / 2 - angleToXAxis;
            double angleToVerticalInDegrees = angleToVertical * (180 / Math.PI);

            angle = -angleToVerticalInDegrees + 180;
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
        public override void shootTarget()
        {
            Random rand = new Random();
            int randomIndex = rand.Next(0, 3);
            int[] soundIndices = { 5, 8, 9 };
            int soundToPlay = soundIndices[randomIndex];
            tempvecY *= 0.02 * this.s;
            tempvecX *= 0.02 * this.s;

            List<LTexture> list = new List<LTexture>();
            list.Add(textureList[4]); // ANGEPASST AN FIGHTER
            list.Add(textureList[4]);
            Bullet bill = new Bullet(list, 10);
            bill.texture.setColor(255, 0, 0);
            double s = Program.SCREEN_WIDTH / Program.SCREEN_HEIGHT;
            bill.spawn(posX + width / 4, posY + height / 4);
            bill.angle = -90;
            bill.vecX = tempvecX * s;
            bill.vecY = tempvecY;
            bill.friendly = false;
            Program.entityList.Add(bill);

            SoundHandler.PlaySound(soundToPlay);
        }
    }
}
