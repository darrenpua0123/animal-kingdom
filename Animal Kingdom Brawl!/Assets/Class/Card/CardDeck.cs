

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

    public void AddSingleCard(Card card, int insertAmount = 1) 
    {
        for (int i = 0; i < insertAmount; i++)
        {
            cards.Add(card);
        }
    }

    public void AddCards(List<Card> addedCards)
    {
        foreach (var card in addedCards)
        {
            cards.Add(card);
        }
    }

    public void RemoveSingleCard(Card card, int removeAmount = 1)
    {
        for (int i = 0; i < removeAmount; i++)
        {
            cards.Remove(card);
        }
    }

    public void RemoveCards(List<Card> removedCards)
    {
        foreach (var card in removedCards)
        {
            cards.Remove(card);
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

    public void InsertCardAt(Card card, int index) 
    {
        cards.Insert(index, card);
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
