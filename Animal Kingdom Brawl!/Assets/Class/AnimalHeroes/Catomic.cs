
public class Catomic : AnimalHero
{
    public static string HERO_NAME = "catomic";
    public int defaultHealth = 20;
    public int defaultShield = 0;
    public int startingActionPoint = 1;
    // public List<Card> defaultHeroCardDeck = new List<Card>() ... obtain from CardDBScheme OR from Catomic.GetDefaultCardDeck (which mean Cards is added in constructor)

    public Catomic() : base()
    { 

    }
}
