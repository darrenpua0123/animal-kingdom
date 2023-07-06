
using UnityEngine;

public class Piggion : AnimalHero
{
    public static string HERO_NAME = "piggion";
    public override int defaultHealth { get; set; } = 20;
    public override int defaultShield { get; set; } = 3;
    public override int startingActionPoint { get; set; } = 1;
    public override Sprite animalHeroImage { get; set; } = Resources.Load<Sprite>("Cards/Piggion/Piggion_Card_Front");



    public Piggion() : base()
    { 
    
    }
}
