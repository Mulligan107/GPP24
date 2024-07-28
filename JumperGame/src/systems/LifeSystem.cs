using System;
using JumperGame;
using JumperGame.src.manager;
using SDL2;

public class LifeSystem
{
    private static LifeSystem _instance;
    public static LifeSystem Instance => _instance ??= new LifeSystem();

    private int lifeCount = 3;

    private LifeSystem() { }

    public void DecrementLife()
    {
        if (lifeCount > 0)
        {
            lifeCount--;
        }
    }

    public void RenderLifeCount(RenderManager renderManager)
    {
        renderManager.RenderLifeCount(lifeCount);
    }
}