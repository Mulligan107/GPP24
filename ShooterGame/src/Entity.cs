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

        public float vecX = 0;
        public float vecY = 0;

        public float speed = 1;

        public float lives;

        public bool alive = true;

        public LTexture texture;

        public SDL.SDL_Rect destRect;

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


        public void render()
        {
            if (alive)
            {
                SDL.SDL_Rect sorRect = new SDL.SDL_Rect { x = 0, y = 0, w = texture.getWidth(), h = texture.getHeight() };
                destRect = new SDL.SDL_Rect { x = (int)System.Math.Floor(posX), y = (int)System.Math.Floor(posY), w = (int)System.Math.Floor(width), h = (int)System.Math.Floor(height) }; // Skalierung auf dieses Rect


                SDL.SDL_RenderCopy(Program.gRenderer, texture.getTexture() , ref sorRect, ref destRect);
               // tex.render(((int)System.Math.Floor(posX)), (int)System.Math.Floor(posY));
            }
            
        }

        public void update(float deltatime)
        {
            move(deltatime);
            destRect = new SDL.SDL_Rect { x = (int)System.Math.Floor(posX), y = (int)System.Math.Floor(posY), w = (int)System.Math.Floor(width), h = (int)System.Math.Floor(height) }; // Skalierung auf dieses Rect
            Console.WriteLine(posX +" "+ posY);
            if (posX < 0 || posX > Program.SCREEN_WIDTH)
            {
                kill();
            }
            else if (posY < 0 || posY > Program.SCREEN_HEIGHT)
            {
                kill();
            }
            render();
        }

        public void spawn(float x, float y)
        {
            posX = x; 
            posY = y; 
        }

    }

}