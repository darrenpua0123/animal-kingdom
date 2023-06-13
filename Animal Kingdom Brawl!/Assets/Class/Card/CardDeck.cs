
using System.Collections.Generic;

public class CardDeck
{
    private List<Card> drawableCards = new List<Card>();
    private List<Card> discardCards = new List<Card>();

    public CardDeck(List<Card> cards) 
    { 
        drawableCards = cards;
    }

    public CardDeck() 
    { 

    }

    public void AddDrawableCard(Card card, int numberOfCards = 1) 
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            drawableCards.Add(card);
        }
    }

    public List<Card> GetDrawableCards()
    {
        return drawableCards;
    }

    // ShuffleDrawableCards, return List

    // ShuffleDiscardCards, return List
}
