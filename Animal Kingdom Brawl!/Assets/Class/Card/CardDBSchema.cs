using Mono.Cecil;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SpritePath 
{ 
    Cardback,
    Relic,
    Artifact,
    Trap,
    Hornterror,
    Catomic,
    Piggion,
    Pandragon,
    Beedle
}

public class CardDBSchema
{
    private static readonly CardDBSchema instance = new CardDBSchema();
    
    // private constructor to prevent instantiation outside of the class
    private CardDBSchema() 
    {
        // TODO: Add other Hero Card Function
    }

    public static CardDBSchema Instance
    {
        get { return instance; }
    }

    public static CardDeck GetDefaultChestDeck() 
    {
        CardDeck defaultChestCardDeck = new CardDeck();

        #region Relic
        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Relic)}TheAncientGold"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Chest,
                CardType.Relic,
                new TheAncientGold())
            );

        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Relic)}ClawOfBanshee"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Chest,
                CardType.Relic,
                new ClawOfBanshee())
            );

        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Relic)}QilinsFeather"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Chest,
                CardType.Relic,
                new QilinsFeather())
            );
        #endregion

        #region Artifact
        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Artifact)}BookOfRevealing"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Chest,
                CardType.Artifact,
                new BookOfRevealing()),
                3
            );

        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Artifact)}Expresso"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Chest,
                CardType.Artifact,
                new Expresso()),
                3
            );

        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Artifact)}HealingPotion"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Chest,
                CardType.Artifact,
                new HealingPotion()),
                3
            );

        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Artifact)}TheMimicMirror"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Chest,
                CardType.Artifact,
                new TheMimicMirror()),
                3
            );

        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Artifact)}SwappingRope"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Chest,
                CardType.Artifact,
                new SwappingRope()),
                3
            );
        #endregion

        #region Trap
        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Trap)}AncientContract"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Chest,
                CardType.Trap,
                new AncientContract()),
                3
            );

        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Trap)}BearTrap"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Chest,
                CardType.Trap,
                new BearTrap()),
                3
            );

        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Trap)}BookOfCorruption"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Chest,
                CardType.Trap,
                new BookOfCorruption()),
                3
            );

        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Trap)}CalamityBomb"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Chest,
                CardType.Trap,
                new CalamityBomb()),
                3
            );

        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Trap)}MaliciousRuby"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Chest,
                CardType.Trap,
                new MaliciousRuby()),
                2
            );

        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Trap)}UnspeakableCurse"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Chest,
                CardType.Trap,
                new UnspeakableCurse())
            );
        #endregion

        return defaultChestCardDeck;
    }

    public static CardDeck GetPiggionDefaultCardDeck()
    {
        CardDeck piggionDefaultCardDeck = new CardDeck();

        piggionDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Piggion)}PiggyPunch"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Piggion_Cardback"),
                CardCategory.Piggion,
                CardType.Piggion,
                new PiggyPunch()),
                3
            );

        piggionDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Piggion)}SuitUp"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Piggion_Cardback"),
                CardCategory.Piggion,
                CardType.Piggion,
                new SuitUp()),
                3
            );

        piggionDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Piggion)}LateNoonNap"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Piggion_Cardback"),
                CardCategory.Piggion,
                CardType.Piggion,
                new LateNoonNap()),
                2
            );

        piggionDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Piggion)}WarriorsShield"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Piggion_Cardback"),
                CardCategory.Piggion,
                CardType.Piggion,
                new WarriorsShield()),
                2
            );

        piggionDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Piggion)}CounterPhalanx"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Piggion_Cardback"),
                CardCategory.Piggion,
                CardType.Piggion,
                new CounterPhalanx()),
                4
            );

        piggionDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Piggion)}ThickSkin"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Piggion_Cardback"),
                CardCategory.Piggion,
                CardType.HeroAbility,
                new ThickSkin()),
                2
            );

        piggionDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Piggion)}Retaliation"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Piggion_Cardback"),
                CardCategory.Piggion,
                CardType.HeroAbility,
                new Retaliation()),
                2
            );

        piggionDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Piggion)}PainAndGain"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Piggion_Cardback"),
                CardCategory.Piggion,
                CardType.HeroAbility,
                new PainAndGain()),
                2
            );

        return piggionDefaultCardDeck;
    }

    public static CardDeck GetCatomicDefaultCardDeck() 
    {
        CardDeck catomicDefaultCardDeck = new CardDeck();

        catomicDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Catomic)}BestCatFood"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Catomic_Cardback"),
                CardCategory.Catomic,
                CardType.Catomic,
                new BestCatFood()),
                2
            );

        catomicDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Catomic)}CatArmor"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Catomic_Cardback"),
                CardCategory.Catomic,
                CardType.Catomic,
                new CatArmor()),
                2
            );

        catomicDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Catomic)}CatcatSlash"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Catomic_Cardback"),
                CardCategory.Catomic,
                CardType.Catomic,
                new CatcatSlash()),
                3
            );

        catomicDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Catomic)}CatDoWhatCatWant"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Catomic_Cardback"),
                CardCategory.Catomic,
                CardType.HeroAbility,
                new CatDoWhatCatWant()),
                2
            );

        catomicDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Catomic)}CatPawPunch"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Catomic_Cardback"),
                CardCategory.Catomic,
                CardType.Catomic,
                new CatPawPunch()),
                4
            );

        catomicDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Catomic)}PiercingClawOfPaw"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Catomic_Cardback"),
                CardCategory.Catomic,
                CardType.HeroAbility,
                new PiercingClawOfPaw()),
                2
            );

        catomicDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Catomic)}ThiefInformation"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Catomic_Cardback"),
                CardCategory.Catomic,
                CardType.Catomic,
                new ThiefInformation()),
                3
            );

        catomicDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Catomic)}TreasuresRight"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Catomic_Cardback"),
                CardCategory.Catomic,
                CardType.HeroAbility,
                new TreasuresRight()),
                2
            );

        return catomicDefaultCardDeck;
    }

    public static CardDeck GetPandragonDefaultCardDeck() 
    {
        CardDeck pandragonDefaultCardDeck = new CardDeck();

        pandragonDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Pandragon)}DragonClaw"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Pandragon_Cardback"),
                CardCategory.Pandragon,
                CardType.Pandragon,
                new DragonClaw()),
                4
            );

        pandragonDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Pandragon)}MeatFeast"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Pandragon_Cardback"),
                CardCategory.Pandragon,
                CardType.Pandragon,
                new MeatFeast()),
                2
            );

        pandragonDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Pandragon)}SharpenClaw"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Pandragon_Cardback"),
                CardCategory.Pandragon,
                CardType.Pandragon,
                new SharpenClaw()),
                2
            );

        pandragonDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Pandragon)}WindGustAttack"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Pandragon_Cardback"),
                CardCategory.Pandragon,
                CardType.Pandragon,
                new WindGustAttack()),
                3
            );

        pandragonDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Pandragon)}FlightAndFight"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Pandragon_Cardback"),
                CardCategory.Pandragon,
                CardType.Pandragon,
                new FlightAndFight()),
                3
            );

        pandragonDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Pandragon)}DragonsScale"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Pandragon_Cardback"),
                CardCategory.Pandragon,
                CardType.HeroAbility,
                new DragonsScale()),
                2
            );

        pandragonDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Pandragon)}DragonsRoar"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Pandragon_Cardback"),
                CardCategory.Pandragon,
                CardType.HeroAbility,
                new DragonsRoar()),
                2
            );

        pandragonDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Pandragon)}DracosMight"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Pandragon_Cardback"),
                CardCategory.Pandragon,
                CardType.HeroAbility,
                new DracosMight()),
                2
            );

        return pandragonDefaultCardDeck;
    }

    public static CardDeck GetBeedleDefaultCardDeck() 
    {
        CardDeck beedleDefaultCardDeck = new CardDeck();

        beedleDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Beedle)}HoneyUp"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Beedle_Cardback"),
                CardCategory.Beedle,
                CardType.Beedle,
                new HoneyUp()),
                2
            );

        beedleDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Beedle)}StingerPoke"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Beedle_Cardback"),
                CardCategory.Beedle,
                CardType.Beedle,
                new StingerPoke()),
                4
            );

        beedleDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Beedle)}HoneyEssence"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Beedle_Cardback"),
                CardCategory.Beedle,
                CardType.Beedle,
                new HoneyEssence()),
                2
            );

        beedleDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Beedle)}BeeSwarm"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Beedle_Cardback"),
                CardCategory.Beedle,
                CardType.Beedle,
                new BeeSwarm()),
                2
            );

        beedleDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Beedle)}Rejuvenate"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Beedle_Cardback"),
                CardCategory.Beedle,
                CardType.Beedle,
                new Rejuvenate()),
                2
            );

        beedleDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Beedle)}Levitation"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Beedle_Cardback"),
                CardCategory.Beedle,
                CardType.HeroAbility,
                new Levitation()),
                2
            );

        beedleDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Beedle)}Surveillance"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Beedle_Cardback"),
                CardCategory.Beedle,
                CardType.HeroAbility,
                new Surveillance()),
                3
            );

        beedleDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Beedle)}LastResort"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Beedle_Cardback"),
                CardCategory.Beedle,
                CardType.HeroAbility,
                new LastResort()),
                3
            );

        return beedleDefaultCardDeck;
    }

    public static CardDeck GetHornterrorDefaultCardDeck() 
    {
        CardDeck hornterrorDefaultCardDeck = new CardDeck();

        hornterrorDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Hornterror)}HeatWave"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Hornterror_Cardback"),
                CardCategory.Hornterror,
                CardType.Hornterror,
                new HeatWave()),
                3
            );

        hornterrorDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Hornterror)}SinisterStare"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Hornterror_Cardback"),
                CardCategory.Hornterror,
                CardType.Hornterror,
                new SinisterStare()),
                4
            );

        hornterrorDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Hornterror)}VenomousSplit"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Hornterror_Cardback"),
                CardCategory.Hornterror,
                CardType.Hornterror,
                new VenomousSplit()),
                3
            );

        return hornterrorDefaultCardDeck;
    }

    private static string GetSpritePath(SpritePath path) 
    {
        string spritePathURL;

        switch (path)
        {
            case SpritePath.Cardback:
                spritePathURL = "Cards/Cardback/";
                break;

            case SpritePath.Relic:
                spritePathURL = "Cards/Chest/Relic/";
                break;

            case SpritePath.Artifact:
                spritePathURL = "Cards/Chest/Artifact/";
                break;

            case SpritePath.Trap:
                spritePathURL = "Cards/Chest/Trap/";
                break;

            case SpritePath.Hornterror:
                spritePathURL = "Cards/Hornterror/";
                break;

            case SpritePath.Catomic:
                spritePathURL = "Cards/Catomic/";
                break;

            case SpritePath.Piggion:
                spritePathURL = "Cards/Piggion/";
                break;

            case SpritePath.Pandragon:
                spritePathURL = "Cards/Pandragon/";
                break;

            case SpritePath.Beedle:
                spritePathURL = "Cards/Beedle/";
                break;

            default:
                spritePathURL = "Unknown Path";
                break;
        }

        return spritePathURL;
    }
}