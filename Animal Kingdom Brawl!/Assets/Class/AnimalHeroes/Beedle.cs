
using UnityEngine;

public class Beedle : AnimalHero
{
    public static string HERO_NAME = "beedle";
    public override int defaultHealth { get; set; } = 16;
    public override int defaultShield { get; set; } = 0;
    public override int startingActionPoint { get; set; } = 2;
    public override Sprite animalHeroImage { get; set; } = Resources.Load<Sprite>("Cards/Beedle/Beedle_Card_Front");
    public string heroTrait = "Beedle plays by manipulating the traps in the field without triggering it! Beedle is agile and can play multiple cards in a turn!\n\nBeedle is an extremely fast and fragile animal hero.";

    public Beedle() : base()
    {

    }
}
