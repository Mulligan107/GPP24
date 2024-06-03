using System;
using SDL2;

namespace ShooterGame
{
    class Player : LivingEntity
    {
        
        LTexture bullet;

        public Player(LTexture tex, LTexture bullTex)
        {
            
            lives = 5;
            spawn((Program.SCREEN_WIDTH / 2) , Program.SCREEN_HEIGHT / 2 );
            width = 30 * s;
            height = 30 * s;
            friendly = true;
            bullet = bullTex;
            texture = tex;

        }

        public Bullet shoot(double vecx, double vecy)
        {
            Bullet bill = new Bullet(bullet);
            bill.spawn(posX, posY);
            bill.vecX = vecx;
            bill.vecY = vecy;
            bill.friendly = true;
            return bill;
        }

    }
}


