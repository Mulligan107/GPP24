using System;
using JumperGame;
using SDL2;

public class CoinCounterSystem
{
    private static CoinCounterSystem _instance;
    public static CoinCounterSystem Instance => _instance ??= new CoinCounterSystem();

    private int coinCount = 0;
    private LTexture coinCountTexture = new LTexture();
    
    private CoinCounterSystem() { }

    public void IncrementCoinCount(int coinAmount)
    {
        coinCount += coinAmount;
    }

    public void RenderCoinCount()
    {
        string coinCountText = "Coins: " + coinCount.ToString();
        //Console.WriteLine("Coins: " + coinCount.ToString());
        coinCountTexture = changeText(coinCountTexture, coinCountText);
        coinCountTexture.render(10, 50); 
    }

    private static LTexture changeText(LTexture Ltex, String text)
    {
        Ltex.loadFromRenderedText(text, new SDL.SDL_Color());
        return Ltex;
    }
}