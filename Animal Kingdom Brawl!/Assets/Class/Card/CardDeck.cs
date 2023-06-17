

using System.Collections.Generic;
using UnityEngine;

public class CardDeck
{
    private List<Card> cards = new List<Card>();

    public CardDeck(List<Card> cards) 
    { 
        this.cards = cards;
    }

    public CardDeck() 
    { 

    }

    public List<Card> GetAllCards()
    {
        return cards;
    }

    // TODO: Refactor: Might need to add RemoveCard, paramter maybe use List<Card> to follow all practice.
    // AddCard maybe need change also
    // TODO: InsertCardAt(Card card, index); For Trap Card, cuz need reinsert back.

    public void AddCard(Card card, int numberOfCards = 1) 
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            cards.Add(card);
        }
    }

    public List<Card> DrawCards(int numberOfDraw) 
    { 
        List<Card> drawnCards = new List<Card>();

        for (int i = 0; i < numberOfDraw; i++)
        {
            Card cardElement = cards[0];
            drawnCards.Add(cardElement);
            cards.RemoveAt(0);
        }

        return drawnCards;
    }

    public void ShuffleCards()
    {
        // Fisher-Yates shuffle algorithm
        int n = cards.Count;

        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Card temp = cards[k];
            cards[k] = cards[n];
            cards[n] = temp;
        }
    }
}
