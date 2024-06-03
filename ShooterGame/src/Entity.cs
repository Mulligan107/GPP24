using System;
using System.Runtime.InteropServices;
using SDL2;

namespace ShooterGame
{
    class Entity : ICloneable
    {
        public double width;
        public double height;

        public double posX;
        public double posY;

        public double vecX = 0;
        public double vecY = 0;

        public double speed = 1;

        public double lives;

        public bool alive = true;

        public double s = Program.SCREEN_WIDTH / Program.SCREEN_HEIGHT;

        public LTexture texture;

        public SDL.SDL_Rect destRect;

        public Entity()
        {
        }

        public void move(double deltaTime)
        {
            posX += vecX * (deltaTime / 10) * speed;
            posY += vecY * (deltaTime / 10) * speed;
        }

        public void kill()
        {
            alive = false;
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

        public void update(double deltatime)
        {
            move(deltatime);
            destRect = new SDL.SDL_Rect { x = (int)System.Math.Floor(posX), y = (int)System.Math.Floor(posY), w = (int)System.Math.Floor(width), h = (int)System.Math.Floor(height) }; // Skalierung auf dieses Rect
            render();
        }

        public void spawn(double x, double y)
        {
            posX = x; 
            posY = y; 
        }

        public object Clone()
        {
           return this.MemberwiseClone();
        }
    }

}