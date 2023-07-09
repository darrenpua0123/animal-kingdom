
using UnityEngine;

public class Pandragon : AnimalHero
{
    public static string HERO_NAME = "pandragon";
    public override int defaultHealth { get; set; } = 22;
    public override int defaultShield { get; set; } = 1;
    public override int startingActionPoint { get; set; } = 1;
    public override Sprite animalHeroImage { get; set; } = Resources.Load<Sprite>("Cards/Pandragon/Pandragon_Card_Front");


    public Pandragon() : base()
    { 

    }
}
