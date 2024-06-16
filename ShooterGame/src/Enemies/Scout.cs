using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShooterGame.ui;

namespace ShooterGame.src
{
    class Scout : Enemy
    {

        double s =  Program.SCREEN_WIDTH/Program.SCREEN_HEIGHT;
        double sinValue = 0;
        double radiusX = 20.0;
        double radiusY = 5.0;

        public Scout(List<LTexture> textureList) : base(textureList)
        {
            lives = 0;
            this.textureList = textureList;
            friendly = false;
            width = 60 * s;
            height = 60 * s;
            angle = -90;
            texture = textureList[0];
            setupAnimation(9, "death", textureList[1]);


            onSpawn();
        }

        public override void movementPattern()
        {

            /*
            sinValue -= (0.025);
            angle = (180 / Math.PI) * (sinValue) + 75;
            vecX = radiusX * s * Math.Cos(sinValue);
            vecY = radiusY * s * Math.Sin(sinValue);
            */
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
                    animationHelper(1, 2, "death"); // REPS, SPEED, FLAG

                    //   SoundHandler.PlaySound(1);  TODO - ERSETZEN fucking earrape

                    ScoreUI.IncreaseScore(100);

                }
            }
        }
    }
}
