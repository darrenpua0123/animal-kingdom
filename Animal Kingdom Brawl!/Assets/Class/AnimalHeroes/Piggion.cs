
using UnityEngine;

public class Piggion : AnimalHero
{
    public static string HERO_NAME = "piggion";
    public override int defaultHealth { get; set; } = 18;
    public override int defaultShield { get; set; } = 2;
    public override int startingActionPoint { get; set; } = 1;
    public override Sprite animalHeroImage { get; set; } = Resources.Load<Sprite>("Cards/Piggion/Piggion_Card_Front");
    public string heroTrait = "Piggion focuses on stacking up unbreakable shields and releasing them to deal a huge burst of damage! \n\nPiggion is a highly defensive and slow animal hero.";

    public Piggion() : base()
    { 
    
    }
}
