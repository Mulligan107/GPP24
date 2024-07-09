using SDL2;

namespace JumperGame.components;

public class RenderComponent
{
    public LTexture Rendertexture { get; set; }
    public SDL.SDL_Rect srcRect { get; set; }
    public SDL.SDL_Rect dstRect { get; set; }
    
    public RenderComponent(LTexture lTexture, SDL.SDL_Rect srcrect, SDL.SDL_Rect destrect)
    {
        Rendertexture = lTexture;
        srcRect = srcrect;
        dstRect = destrect;  //TODO bessere variablen Namen
    }
}