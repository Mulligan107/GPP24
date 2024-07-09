using System.Numerics;
using SDL2;

namespace JumperGame.components;

public class RenderComponent
{
    public LTexture Rendertexture { get; set; }
    public SDL.SDL_Rect srcRect;
    public SDL.SDL_Rect dstRect;
    
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