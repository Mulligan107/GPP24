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

        public SDL.SDL_Rect sorRect;
        public SDL.SDL_Rect destRect;
        private static SDL.SDL_Rect[] _SpriteClips;

        //Current animation frame
        public double frameTicker;
        public int frame;
        public static int anzahlFrames;

        public bool startAnimation;

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
                SDL.SDL_RenderCopy(Program.gRenderer, texture.getTexture() , ref sorRect, ref destRect);
               // tex.render(((int)System.Math.Floor(posX)), (int)System.Math.Floor(posY));
            }
            
        }

        public void setupAnimation(int anzahlFrames , LTexture tex)
        {
            texture = tex;
            _SpriteClips = new SDL.SDL_Rect[anzahlFrames];
            //Set sprite clips

            for (int i = 0; i < anzahlFrames; i++)
            {

                _SpriteClips[i].x = 0 + ((texture.getWidth() / anzahlFrames) * i);
                _SpriteClips[i].y = 0;
                _SpriteClips[i].w = texture.getWidth()/anzahlFrames;
                _SpriteClips[i].h = texture.getHeight();
            }
            frame = 0;
            frameTicker = 0;
            startAnimation = true;
            
        }

        public void update(double deltatime)
        {
            move(deltatime);
            sorRect = new SDL.SDL_Rect { x = 0, y = 0, w = texture.getWidth(), h = texture.getHeight() };
            destRect = new SDL.SDL_Rect { x = (int)System.Math.Floor(posX), y = (int)System.Math.Floor(posY), w = (int)System.Math.Floor(width), h = (int)System.Math.Floor(height) }; // Skalierung auf dieses Rect
            if (startAnimation)
            {
                sorRect = _SpriteClips[frame];
            }
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