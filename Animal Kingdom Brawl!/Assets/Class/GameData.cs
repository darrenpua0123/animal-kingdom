using Mono.Cecil;
using System.Collections.Generic;

public class GameData
{
    private static readonly GameData instance = new GameData();

    public static readonly string defaultStartingCurrency = "250";
    public static readonly List<string> defaultHeroes = new List<string>() { "piggion" };
    
    // private constructor to prevent instantiation outside of the class
    private GameData() { } 

    public static GameData Instance
    {
        get { return instance; }
    }
}
