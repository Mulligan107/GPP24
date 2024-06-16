using System;
using System.Collections.Generic;
using SDL2;
using ShooterGame.ui;
using static System.Net.Mime.MediaTypeNames;

namespace ShooterGame
{
    class Player : LivingEntity
    {
        public static double lifes;

        public Player(List<LTexture> list)
        {
            lives = 5;
            lifes = lives;
            spawn((Program.SCREEN_WIDTH / 2) , Program.SCREEN_HEIGHT / 2 );
            width = 90 * s;
            height = 90 * s;
            angle = 90;
            friendly = true;
            textureList = list;
            texture = list[0];
        }
        
        public override void hit()
        {
            if (!iframe)
            {
                iframe = true;
                
                lives = lives - 1;
                lifes = lives;

                if (lives < 0) // DEATH
                {
                    //animationHelper(1,2,"death");
                    
                    SoundHandler.PlaySound(1);
        
                }
                else // HIT
                {
                    //animationHelper(3, 4, "shield");
                    
                    ScoreUI.IncreaseScore(-100);
                }
                Console.WriteLine("Lives after: " + lives);
            }
        }

        public override void move(double deltaTime)
        {
            if (posX > 0 || posX < Program.SCREEN_WIDTH && posY > 0 || posY < Program.SCREEN_HEIGHT) //begrenzung für Spieler, nur im Fenster bewegen TODO bringt nix
            {
                vecX = vecX * 0.925;
                vecY = vecY * 0.925;
                posX += vecX * (deltaTime / 10) * speed;
                posY += vecY * (deltaTime / 10) * speed;
            }
            
        }

        public Bullet shoot(double vecx, double vecy, int direction)
        {
            Bullet bill = new Bullet(textureList,4);
            bill.spawn(posX + width/4 , posY + height/4);
            bill.angle = direction;
            bill.vecX = vecx;
            bill.vecY = vecy;
            bill.friendly = true;
            return bill;
        }

    }
}


