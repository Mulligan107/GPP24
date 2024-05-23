using System;
using SDL2;

namespace ShooterGame
{
    public class Entity
    {
        public float width;
        public float height;

        public float posX;
        public float posY;

        public float vecX;
        public float vecY;

        public float speed;

        public bool alive = true;

        private static LTexture texture = new LTexture();

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

        private static void setTexture(LTexture tex)
        {
            texture = tex;
        }

        public void render()
        {   
            if (alive)
            {
                texture.render(((int)System.Math.Floor(posX)), (int)System.Math.Floor(posY));
            }
            
        }

    }

}