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
    
    public void RenderLifeCount(RenderManager renderManager)
    {
        renderManager.RenderLifeCount(LifeCount);
    }
}