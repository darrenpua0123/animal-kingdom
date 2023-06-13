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
    Hornterror
}

public class CardDBSchema
{
    private static readonly CardDBSchema instance = new CardDBSchema();
    
    public static CardDeck defaultChestCardDeck;
    public static CardDeck hornterrorDefaultCardDeck;
    public static CardDeck catomicDefaultCardDeck;

    // private constructor to prevent instantiation outside of the class
    private CardDBSchema() 
    {
        // TODO: THIS FIRST! Move all artifact/trap card in. Card Prefab is done, try to move DrawCard to cardDeck func.
        // AS EVERY PLAYER HAVE THEIR OWN DECK TO DRAW AND MAY CONTAIN DIFFERENT CARD. DOESN'T MAKE SENSE FOR
        // CATOMIC'S CARDDEC TO HOLD ARTIFACTCARD

        #region Chest
        defaultChestCardDeck = new CardDeck();

        #region Relic
        defaultChestCardDeck.AddDrawableCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Relic)}TheAncientGold"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Relic,
                CardType.Relic_TheAncientGold)
            );

        defaultChestCardDeck.AddDrawableCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Relic)}ClawOfBanshee"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Relic,
                CardType.Relic_ClawOfBanshee)
            );

        defaultChestCardDeck.AddDrawableCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Relic)}QilinsFeather"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Relic,
                CardType.Relic_QilinsFeather)
            );
        #endregion

        #region Artifact
        defaultChestCardDeck.AddDrawableCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Artifact)}BookOfRevealing"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Artifact,
                CardType.Artifact_BookOfRevealing),
                3
            );

        defaultChestCardDeck.AddDrawableCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Artifact)}Expresso"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Artifact,
                CardType.Artifact_Expresso),
                3
            );

        defaultChestCardDeck.AddDrawableCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Artifact)}HealingPotion"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Artifact,
                CardType.Artifact_HealingPotion),
                3
            );

        defaultChestCardDeck.AddDrawableCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Artifact)}TheMimicMirror"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Artifact,
                CardType.Artifact_TheMimicMirror),
                3
            );

        defaultChestCardDeck.AddDrawableCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Artifact)}SwappingRope"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Artifact,
                CardType.Artifact_SwappingRope),
                3
            );
        #endregion

        #region Trap
        defaultChestCardDeck.AddDrawableCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Trap)}AncientContract"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Trap,
                CardType.Trap_AncientContract),
                3
            );

        defaultChestCardDeck.AddDrawableCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Trap)}BearTrap"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Trap,
                CardType.Trap_BearTrap),
                3
            );

        defaultChestCardDeck.AddDrawableCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Trap)}BookOfCorruption"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Trap,
                CardType.Trap_BookOfCorruption),
                3
            );

        defaultChestCardDeck.AddDrawableCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Trap)}CalamityBomb"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Trap,
                CardType.Trap_CalamityBomb),
                3
            );

        defaultChestCardDeck.AddDrawableCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Trap)}MaliciousRuby"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Trap,
                CardType.Trap_MaliciousRuby),
                2
            );

        defaultChestCardDeck.AddDrawableCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Trap)}UnspeakableCurse"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Trap,
                CardType.Trap_TheUnspeakableCurse)
            );
        #endregion
        #endregion

        #region Hornterror
        hornterrorDefaultCardDeck = new CardDeck();

        hornterrorDefaultCardDeck.AddDrawableCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Hornterror)}HeatWave"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Hornterror_Cardback"),
                CardCategory.Hornterror,
                CardType.Hornterror_HeatWave),
                3
            );

        hornterrorDefaultCardDeck.AddDrawableCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Hornterror)}SinisterStare"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Hornterror_Cardback"),
                CardCategory.Hornterror,
                CardType.Hornterror_SinisterStare),
                4
            );

        hornterrorDefaultCardDeck.AddDrawableCard(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Hornterror)}VenomousSplit"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Hornterror_Cardback"),
                CardCategory.Hornterror,
                CardType.Hornterror_VenomousSplit),
                3
            );
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

            default:
                spritePathURL = "Unknown Path";
                break;
        }

        return spritePathURL;
    }
}