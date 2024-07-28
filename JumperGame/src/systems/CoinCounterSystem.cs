using System;
using JumperGame;
using JumperGame.src.manager;
using SDL2;

public class CoinCounterSystem
{
    private static CoinCounterSystem _instance;
    public static CoinCounterSystem Instance => _instance ??= new CoinCounterSystem();

    private int coinCount = 0;

    private CoinCounterSystem() { }

    public void IncrementCoinCount(int coinAmount)
    {
        coinCount += coinAmount;
    }
    
    public void ResetCoinCount()
    {
        coinCount = 0;
    }

    public void RenderCoinCount(RenderManager renderManager)
    {
        renderManager.RenderCoinCount(coinCount);
    }
}