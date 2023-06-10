
using System.Collections.Generic;

public class CardDeck
{
    private List<Card> drawableCards = new List<Card>();
    private List<Card> discardCards = new List<Card>();

    public CardDeck(List<Card> cards) 
    { 
        // TODO: to be complete after Card.cs!
        drawableCards = cards;
    }

    public List<Card> GetDrawableCards() 
    {  
        return drawableCards; 
    }

    // ShuffleDrawableCards, return List

    // ShuffleDiscardCards, return List
}
