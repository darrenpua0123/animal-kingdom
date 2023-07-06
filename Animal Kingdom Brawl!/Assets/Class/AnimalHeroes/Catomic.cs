
using UnityEngine;

public class Catomic : AnimalHero
{
    public static string HERO_NAME = "catomic";
    public override int defaultHealth { get; set; } = 20;
    public override int defaultShield { get; set; } = 0;
    public override int startingActionPoint { get; set; } = 1;
    public override Sprite animalHeroImage { get; set; } = Resources.Load<Sprite>("Cards/Catomic/Catomic_Card_Front");


    public Catomic() : base()
    { 

    }
}
