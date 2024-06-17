using System;
using System.Collections.Generic;
using SDL2;
using ShooterGame.level;
using ShooterGame.ui;
using static System.Net.Mime.MediaTypeNames;

namespace ShooterGame
{
    class Player : LivingEntity
    {
        public static double lifes;

        public Player(List<LTexture> list)
        {
            lives = 20;
            lifes = lives;
            spawn((Program.SCREEN_WIDTH / 2) , Program.SCREEN_HEIGHT / 2 );
            width = 60 * s;
            height = 60 * s;
            angle = 90;
            friendly = true;
            textureList = list;
            texture = list[0];
            overTexture = list[2];
            overTexture.setColor(0, 255, 0);
            setupAnimation(10, "shield", list[2]);
            checkLives();
            
        }

        public override void hit()
        {
            if (!iframe)
            {
                iframe = true;

                lives = lives - 1;
                lifes = lives;

                SoundHandler.PlaySound(6);
                
                animationHelper(1, 1, "shield");
                checkLives();
                ScoreUI.IncreaseScore(-100);

            }
        
        }
        
        public void checkLives()
        {
            switch (lives)
            {
                case 0:
                    LevelManager.ResetStats();
                    Program.CurrentState = GameState.GAME_OVER;
                    Program.VisibleMenu = new GameOverMenu(Program.gRenderer);
                    ScoreUI.ResetScore();
                    break;
                case 1:
                    this.texture = textureList[5];
                    break;
                case 2:
                    this.texture = textureList[4];
                    break;
                case 3:
                    this.texture = textureList[3];
                    break;
            }
        }
        public override void move(double deltaTime)
        {
            checkLives();
            double proposedPosX = posX + vecX * (deltaTime / 10) * speed;
            double proposedPosY = posY + vecY * (deltaTime / 10) * speed;

            // Check if the proposed position is within the screen boundaries
            if (proposedPosX >= 0 && proposedPosX <= Program.SCREEN_WIDTH - width)
            {
                posX = proposedPosX;
            }
            if (proposedPosY >= 0 && proposedPosY <= Program.SCREEN_HEIGHT - height)
            {
                posY = proposedPosY;
            }

            vecX = vecX * 0.925;
            vecY = vecY * 0.925;
        }

        public Bullet shoot(double vecx, double vecy, int direction, double delta)
        {
            Bullet bill = new Bullet(textureList,4);
            bill.spawn(posX + width/8 , posY + height/8);
            bill.angle = direction;
            bill.vecX = vecx * (delta / 10);
            bill.vecY = vecy * (delta / 10);
            bill.friendly = true;
            return bill;
        }

    }
}


