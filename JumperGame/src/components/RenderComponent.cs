namespace JumperGame.components;

public class RenderComponent
{
    public LTexture Rendertexture { get; set; }
    
    public RenderComponent(LTexture lTexture)
    {
        Rendertexture = lTexture;
    }
}