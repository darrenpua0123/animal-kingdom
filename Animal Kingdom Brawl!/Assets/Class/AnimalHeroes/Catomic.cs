
public class Catomic : AnimalHero
{
    public static string HERO_NAME = "catomic";
    public override int defaultHealth { get; set; } = 20;
    public override int defaultShield { get; set; } = 0;
    public override int startingActionPoint { get; set; } = 1;

    public Catomic() : base()
    { 

    }
}
