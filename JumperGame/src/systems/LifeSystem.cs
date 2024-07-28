using System;
using JumperGame;
using SDL2;

public class LifeSystem
{
    private static LifeSystem _instance;
    public static LifeSystem Instance => _instance ??= new LifeSystem();

    private int lifeCount = 3; 
    private LTexture lifeCountTexture = new LTexture();
    
    private LifeSystem() { }

    public void DecrementLife()
    {
        if (lifeCount > 0)
        {
            lifeCount--;
        }
    }

    public void RenderLifeCount()
    {
        string lifeCountText = "Lives: " + lifeCount.ToString();
        Console.WriteLine("Lives: " + lifeCount.ToString());
        lifeCountTexture = changeText(lifeCountTexture, lifeCountText);
        lifeCountTexture.render(10, 100); 
    }

    private static LTexture changeText(LTexture Ltex, String text)
    {
        Ltex.loadFromRenderedText(text, new SDL.SDL_Color());
        return Ltex;
    }
}