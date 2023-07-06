
using UnityEngine;

public class Beedle : AnimalHero
{
    public static string HERO_NAME = "beedle";
    public override int defaultHealth { get; set; } = 15;
    public override int defaultShield { get; set; } = 0;
    public override int startingActionPoint { get; set; } = 2;
    public override Sprite animalHeroImage { get; set; } = Resources.Load<Sprite>("Cards/Beedle/Beedle_Card_Front");

    public Beedle() : base()
    {

    }
}
