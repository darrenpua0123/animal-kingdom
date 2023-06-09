using Mono.Cecil;
using System.Collections.Generic;
using UnityEngine;

public class CardDBSchema
{
    private static readonly CardDBSchema instance = new CardDBSchema();
    
    public static CardDeck defaultChestCardDeck;
    private readonly string chestCardImagePath = "Cards/Chest/";

    public static CardDeck catomicStartingCardDeck;

    // private constructor to prevent instantiation outside of the class
    private CardDBSchema() 
    {
        // TODO: DO THIS FIRST, NEED TO REDO HOW TO CALL DB. CARDDECK SHOULD NOT BE INSIDE ANIMALHERO,
        // AS EVERY PLAYER HAVE THEIR OWN DECK TO DRAW AND MAY CONTAIN DIFFERENT CARD. DOESN'T MAKE SENSE FOR
        // CATOMIC'S CARDDEC TO HOLD ARTIFACTCARD
        List<Card> chestCards = new List<Card>();

        chestCards.Add(new Card(
                Resources.Load<Sprite>($"{chestCardImagePath}Relic/AncientGold"),
                Resources.Load<Sprite>($"{chestCardImagePath}Chest_Cardback"),
                CardType.Relic_TheAncientGold
            ));

        chestCards.Add(new Card(
                Resources.Load<Sprite>($"{chestCardImagePath}Relic/ClawOfBanshee"),
                Resources.Load<Sprite>($"{chestCardImagePath}Chest_Cardback"),
                CardType.Relic_ClawOfBanshee
            ));

        chestCards.Add(new Card(
                Resources.Load<Sprite>($"{chestCardImagePath}Relic/QilinsFeather"),
                Resources.Load<Sprite>($"{chestCardImagePath}Chest_Cardback"),
                CardType.Relic_QilinsFeather
            ));

        defaultChestCardDeck = new CardDeck(chestCards);
    }

    public static CardDBSchema Instance
    {
        get { return Instance; }
    }
}