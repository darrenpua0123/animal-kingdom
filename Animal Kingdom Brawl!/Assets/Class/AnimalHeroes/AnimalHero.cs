
using UnityEngine;

public class AnimalHero
{
    public virtual int defaultHealth { get; set; } = 0;
    public virtual int defaultShield { get; set; } = 0;
    public virtual int startingActionPoint { get; set; } = 0;
    public virtual Sprite animalHeroImage { get; set; }

    public AnimalHero() 
    { 

    }
}
