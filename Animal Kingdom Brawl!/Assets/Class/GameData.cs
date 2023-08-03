using Mono.Cecil;
using System.Collections.Generic;

public class GameData
{
    private static readonly GameData instance = new GameData();

    public static readonly int DefaultStartingCurrency = 250;
    public static readonly List<string> DefaultHeroes = new List<string>() { "catomic" };
    public static readonly int PiggionShopCost = 500;
    public static readonly int CatomicShopCost = 500;
    public static readonly int PandragonShopCost = 500;
    public static readonly int BeedleShopCost = 500;
    
    // private constructor to prevent instantiation outside of the class
    private GameData() { } 

    public static GameData Instance
    {
        get { return Instance; }
    }
}
