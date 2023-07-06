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
    Catomic
}

public class CardDBSchema
{
    private static readonly CardDBSchema instance = new CardDBSchema();
    
    public static CardDeck defaultChestCardDeck;
    public static CardDeck hornterrorDefaultCardDeck;
    public static CardDeck beedleDefaultCardDeck;
    public static CardDeck catomicDefaultCardDeck;
    public static CardDeck pandragonDefaultCardDeck;
    public static CardDeck piggionDefaultCardDeck;

    // private constructor to prevent instantiation outside of the class
    private CardDBSchema() 
    {
        #region Chest
        defaultChestCardDeck = new CardDeck();

        #region Relic
        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Relic)}TheAncientGold"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Relic,
                CardType.Relic_TheAncientGold)
            );

        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Relic)}ClawOfBanshee"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Relic,
                CardType.Relic_ClawOfBanshee)
            );

        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Relic)}QilinsFeather"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Relic,
                CardType.Relic_QilinsFeather)
            );
        #endregion

        #region Artifact
        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Artifact)}BookOfRevealing"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Artifact,
                CardType.Artifact_BookOfRevealing),
                3
            );

        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Artifact)}Expresso"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Artifact,
                CardType.Artifact_Expresso),
                3
            );

        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Artifact)}HealingPotion"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Artifact,
                CardType.Artifact_HealingPotion),
                3
            );

        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Artifact)}TheMimicMirror"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Artifact,
                CardType.Artifact_TheMimicMirror),
                3
            );

        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Artifact)}SwappingRope"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Artifact,
                CardType.Artifact_SwappingRope),
                3
            );
        #endregion

        #region Trap
        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Trap)}AncientContract"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Trap,
                CardType.Trap_AncientContract),
                3
            );

        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Trap)}BearTrap"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Trap,
                CardType.Trap_BearTrap),
                3
            );

        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Trap)}BookOfCorruption"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Trap,
                CardType.Trap_BookOfCorruption),
                3
            );

        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Trap)}CalamityBomb"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Trap,
                CardType.Trap_CalamityBomb),
                3
            );

        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Trap)}MaliciousRuby"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Trap,
                CardType.Trap_MaliciousRuby),
                2
            );

        defaultChestCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Trap)}UnspeakableCurse"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Trap,
                CardType.Trap_TheUnspeakableCurse)
            );
        #endregion
        #endregion

        #region Hornterror
        hornterrorDefaultCardDeck = new CardDeck();

        hornterrorDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Hornterror)}HeatWave"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Hornterror_Cardback"),
                CardCategory.Hornterror,
                CardType.Hornterror_HeatWave),
                3
            );

        hornterrorDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Hornterror)}SinisterStare"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Hornterror_Cardback"),
                CardCategory.Hornterror,
                CardType.Hornterror_SinisterStare),
                4
            );

        hornterrorDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Hornterror)}VenomousSplit"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Hornterror_Cardback"),
                CardCategory.Hornterror,
                CardType.Hornterror_VenomousSplit),
                3
            );
        #endregion

        #region Animal Hero

        // TODO: Add other Hero Card
        #region Beedle
        beedleDefaultCardDeck = new CardDeck();
        // Beedle Card

        #endregion

        #region Catomic
        catomicDefaultCardDeck = new CardDeck();

        catomicDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Catomic)}BestCatFood"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Catomic_Cardback"),
                CardCategory.Catomic,
                CardType.Catomic_BestCatFood),
                2
            );

        catomicDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Catomic)}CatArmor"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Catomic_Cardback"),
                CardCategory.Catomic,
                CardType.Catomic_CatArmor),
                2
            );

        catomicDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Catomic)}CatcatSlash"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Catomic_Cardback"),
                CardCategory.Catomic,
                new CatcatSlash()),
                3
            );

        catomicDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Catomic)}CatDoWhatCatWant"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Catomic_Cardback"),
                CardCategory.Catomic,
                CardType.Catomic_CatDoWhatCatWant),
                2
            );

        catomicDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Catomic)}CatPawPunch"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Catomic_Cardback"),
                CardCategory.Catomic,
                CardType.Catomic_CatPawPunch),
                4
            );

        catomicDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Catomic)}PiercingClawOfPaw"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Catomic_Cardback"),
                CardCategory.Catomic,
                CardType.Catomic_PiercingClawOfPaw),
                2
            );

        catomicDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Catomic)}ThiefInformation"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Catomic_Cardback"),
                CardCategory.Catomic,
                CardType.Catomic_ThiefInformation),
                3
            );

        catomicDefaultCardDeck.AddSingleCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Catomic)}TreasuresRight"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Catomic_Cardback"),
                CardCategory.Catomic,
                CardType.Catomic_TreasuresRight),
                2
            );
        #endregion

        #region Pandragon
        pandragonDefaultCardDeck = new CardDeck();
        // Pandragon card

        #endregion

        #region Piggion
        piggionDefaultCardDeck = new CardDeck();
        // Piggion card

        #endregion

        #endregion
    }

    public static CardDBSchema Instance
    {
        get { return instance; }
    }

    private string GetSpritePath(SpritePath path) 
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

            default:
                spritePathURL = "Unknown Path";
                break;
        }

        return spritePathURL;
    }
}