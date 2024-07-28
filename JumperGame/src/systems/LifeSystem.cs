using System;
using JumperGame;
using JumperGame.src.manager;
using SDL2;

public class LifeSystem
{
    private static LifeSystem _instance;
    public static LifeSystem Instance => _instance ??= new LifeSystem();

    private int lifeCount = 3;
    private bool isGameOverTriggered = false;

    private LifeSystem() { }

    public void DecrementLife(int lifeAmount)
    {
        if (lifeCount > 0)
        {
            lifeCount -= lifeAmount;
        }
    }
    
    public bool IsGameOver()
    {
        if (lifeCount <= 0 && !isGameOverTriggered)
        {
            isGameOverTriggered = true;
            return true;
        }
        return false;
    }
    
    public void RenderLifeCount(RenderManager renderManager)
    {
        renderManager.RenderLifeCount(lifeCount);
    }
}