using System.Numerics;
using SDL2;
using static SDL2.SDL;

namespace JumperGame.components;

public class RenderComponent
{
    public LTexture Rendertexture { get; set; }
    public SDL.SDL_Rect srcRect;
    public SDL.SDL_Rect dstRect;
    public double angle { get; set; }
    public SDL.SDL_Point centerPoint;
    public SDL.SDL_RendererFlip flip = SDL_RendererFlip.SDL_FLIP_NONE;

    public RenderComponent(LTexture lTexture, SDL.SDL_Rect srcrect, SDL.SDL_Rect destrect)
    {
        Rendertexture = lTexture;
        srcRect = srcrect;
        dstRect = destrect;  //TODO bessere variablen Namen
    }
    
    public void UpdateDstRect(Vector3 position)
    {
        dstRect.x = (int)position.X;
        dstRect.y = (int)position.Y;
    }
}