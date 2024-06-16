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
        double sinValue = 0;
        double radiusX = 5.0; 
        double radiusY = 1.0; 
        double boxLength = 5 * Program.SCREEN_WIDTH/Program.SCREEN_HEIGHT;

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
            sinValue -= (0.05);
            angle = (180 / Math.PI) * (sinValue) + 75;
            vecX = radiusX * Math.Cos(sinValue);
            vecY = radiusY * Math.Sin(sinValue);
            */
            speed = 10;
            vecX = -5;

            if (posX < Program.SCREEN_WIDTH / 8 && posY > Program.SCREEN_HEIGHT - Program.SCREEN_HEIGHT / 8) {
                vecX = 0;
                vecY = +5;   
            }

            if (posY < Program.SCREEN_HEIGHT / 8)
            {
                vecX = +5;
                vecY = 0;
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
