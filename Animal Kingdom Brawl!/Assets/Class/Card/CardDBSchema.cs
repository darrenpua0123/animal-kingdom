using Mono.Cecil;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SpritePath 
{ 
    Cardback,
    Relic,
    Artifact,
    Trap
}

public class CardDBSchema
{
    private static readonly CardDBSchema instance = new CardDBSchema();
    
    public static CardDeck defaultChestCardDeck;

    public static CardDeck catomicStartingCardDeck;

    // private constructor to prevent instantiation outside of the class
    private CardDBSchema() 
    {
        // TODO: THIS FIRST! Move all artifact/trap card in. Card Prefab is done, try to move DrawCard to cardDeck func.
        // AS EVERY PLAYER HAVE THEIR OWN DECK TO DRAW AND MAY CONTAIN DIFFERENT CARD. DOESN'T MAKE SENSE FOR
        // CATOMIC'S CARDDEC TO HOLD ARTIFACTCARD
        #region Chest
        List<Card> chestCards = new List<Card>();

        #region Relic
        chestCards.Add(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Relic)}AncientGold"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Relic,
                CardType.Relic_TheAncientGold
            ));

        chestCards.Add(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Relic)}ClawOfBanshee"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Relic,
                CardType.Relic_ClawOfBanshee
            ));

        chestCards.Add(new Card(
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Relic)}QilinsFeather"),
                Resources.Load<Sprite>($"{GetSpritePath(SpritePath.Cardback)}Chest_Cardback"),
                CardCategory.Relic,
                CardType.Relic_QilinsFeather
            ));

        defaultChestCardDeck = new CardDeck(chestCards);
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
            case SpritePath.Relic:
                spritePathURL = "Cards/Chest/Relic/";
                break;

            case SpritePath.Artifact:
                spritePathURL = "Cards/Chest/Artifact/";
                break;

            case SpritePath.Trap:
                spritePathURL = "Cards/Chest/Trap/";
                break;

            case SpritePath.Cardback:
                spritePathURL = "Cards/Cardback/";
                break;

            default:
                spritePathURL = "Unknown Path";
                break;
        }

        return spritePathURL;
    }
}