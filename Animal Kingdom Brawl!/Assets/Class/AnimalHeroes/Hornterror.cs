
using UnityEngine;

public class Hornterror : AnimalHero
{
    public static string HERO_NAME = "hornterror";
    public override int defaultHealth { get; set; } = 20;
    public override int defaultShield { get; set; } = 0;
    public override int startingActionPoint { get; set; } = 1;
    public override Sprite animalHeroImage { get; set; } = Resources.Load<Sprite>("Cards/Hornterror/Hornterror_Card_Front");


    public Hornterror() : base()
    {

    }
}
