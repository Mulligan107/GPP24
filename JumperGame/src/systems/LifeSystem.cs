using System;
using JumperGame;
using JumperGame.src.manager;
using SDL2;

public class LifeSystem
{
    private static LifeSystem _instance;
    public static LifeSystem Instance => _instance ??= new LifeSystem();

    public int LifeCount { get; set; } = 3;
    public bool IsGameOverTriggered { get; set; }
    private LTexture lifeCountTexture = new LTexture();

    private LifeSystem() { }

    public void DecrementLife(int lifeAmount)
    {
        if (LifeCount > 0)
        {
            LifeCount -= lifeAmount;
        }
    }
    
    public bool IsGameOver()
    {
        if (LifeCount <= 0 && !IsGameOverTriggered)
        {
            IsGameOverTriggered = true;
            return true;
        }
        return false;
    }
    
    public void RenderLifeCount()
    {
        string lifeCountText = "Lives: " + LifeCount.ToString();
        lifeCountTexture = changeText(lifeCountTexture, lifeCountText);
        lifeCountTexture.render(10, 35); 
    }
    
    static LTexture changeText(LTexture Ltex, String text)
    {
        Ltex.loadFromRenderedText(text, new SDL.SDL_Color());
        return Ltex;
    }
}