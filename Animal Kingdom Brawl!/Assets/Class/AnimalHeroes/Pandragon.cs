
using UnityEngine;

public class Pandragon : AnimalHero
{
    public static string HERO_NAME = "pandragon";
    public override int defaultHealth { get; set; } = 22;
    public override int defaultShield { get; set; } = 0;
    public override int startingActionPoint { get; set; } = 1;
    public override Sprite animalHeroImage { get; set; } = Resources.Load<Sprite>("Cards/Pandragon/Pandragon_Card_Front");
    public string heroTrait = "Pandragon has high survivability and can deal huge amounts of damage! Pandragon can also deal damage that targets everyone!\n\nPandragon is a high damage and slow animal hero.";

    public Pandragon() : base()
    { 

    }
}
