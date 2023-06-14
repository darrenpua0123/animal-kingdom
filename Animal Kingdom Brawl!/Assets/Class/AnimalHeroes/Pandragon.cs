
public class Pandragon : AnimalHero
{
    public static string HERO_NAME = "pandragon";
    public override int defaultHealth { get; set; } = 25;
    public override int defaultShield { get; set; } = 1;
    public override int startingActionPoint { get; set; } = 1;

    public Pandragon() : base()
    { 

    }
}
