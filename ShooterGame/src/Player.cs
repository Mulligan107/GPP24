using System;
using SDL2;

namespace ShooterGame
{
    class Player : Entity
    {
        
        LTexture bullet;

        public Player(LTexture tex, LTexture bullTex)
        {
            lives = 5;
            spawn((Program.SCREEN_WIDTH / 2) , Program.SCREEN_HEIGHT / 2 );
            width = 30;
            height = 30;
            bullet = bullTex;
            texture = tex;

        }

        public Bullet shoot(float vecx, float vecy)
        {
            Bullet bill = new Bullet(bullet);
            bill.spawn(posX, posY);
            bill.vecX = vecx;
            bill.vecY = vecy;
            return bill;
        }

    }
}


