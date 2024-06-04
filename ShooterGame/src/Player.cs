using System;
using System.Collections.Generic;
using SDL2;
using static System.Net.Mime.MediaTypeNames;

namespace ShooterGame
{
    class Player : LivingEntity
    {


        public Player(List<LTexture> list)
        {
            
            lives = 5;
            spawn((Program.SCREEN_WIDTH / 2) , Program.SCREEN_HEIGHT / 2 );
            width = 90 * s;
            height = 90 * s;
            angle = 90;
            friendly = true;
            textureList = list;
            texture = list[0];
            

        }

        public Bullet shoot(double vecx, double vecy, int direction)
        {
            Bullet bill = new Bullet(textureList[1]);
            bill.angle = direction;
            bill.textureList = textureList;
            bill.textureList.Add(texture);
            bill.textureList.Add(texture);
            bill.spawn(posX, posY);
            bill.vecX = vecx;
            bill.vecY = vecy;
            bill.friendly = true;
            bill.choosenAnim = 1;
            bill.setupAnimation(4);
            bill.repeats = 999;
            bill.animationCounter = 0;
            bill.animationSpeed = 1;
            return bill;
        }

    }
}


