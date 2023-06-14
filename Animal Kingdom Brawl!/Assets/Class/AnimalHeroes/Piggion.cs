
public class Piggion : AnimalHero
{
    public static string HERO_NAME = "piggion";
    public override int defaultHealth { get; set; } = 20;
    public override int defaultShield { get; set; } = 3;
    public override int startingActionPoint { get; set; } = 1;


    public Piggion() : base()
    { 
    
    }
}
