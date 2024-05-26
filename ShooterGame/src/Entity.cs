using System;
using SDL2;

namespace ShooterGame
{
    class Entity
    {
        public float width;
        public float height;

        public float posX;
        public float posY;

        public float vecX;
        public float vecY;

        public float speed;

        public float lives;

        public bool alive = true;

        public Entity()
        {
        }

        public void move(float deltaTime)
        {
            posX += vecX * (deltaTime / 10) * speed;
            posY += vecY * (deltaTime / 10) * speed;
        }

        public void kill()
        {
            alive = false;
        }

        public void hit()
        {
            lives += lives - 1;
            if (lives < 0)
            {
                kill();
            }
        }

        public void render(LTexture tex)
        {
            if (alive)
            {
                tex.render(((int)System.Math.Floor(posX)), (int)System.Math.Floor(posY));
            }
            
        }

        public void spawn(float x, float y)
        {
            posX = x; 
            posY = y; 
        }

    }

}