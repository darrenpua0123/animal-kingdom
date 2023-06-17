
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Timeline;

public class Player
{
    public AnimalHero animalHero;
    public int health;
    public int shield;
    public int actionPoint;
    public int knockoutCounter = 0;
    public int relicCounter = 0;

    public CardDeck cardDeck;
    public CardDeck discardDeck = new CardDeck();
    public List<Card> playerHand = new List<Card>(); // TODO: Refactor: Maybe change to CardDeck also?

    public List<string> activeEffects = new List<string>(); // TODO: active effect need refactor

    public Player(AnimalHero chosenAnimalHero, CardDeck animalHeroCardDeck) 
    {
        // animalhero, AnimalHero
        // health, int -> from animlahero.defaulthealth
        // shield, int -> from animalhero.deffaultShield
        // actionPoint, int -> from animalhero.defaultAP
        // cardDeck, CardDeck -> from animalhero.defaultCardDeck, if need to modify like player hide card inside, can modify
        // playerHand, List<Card> -> after drawCard from cardDeck / TrapCardDeck
        // knockout counter, int
        // relic counter, int
        // ActiveEffect, ? maybe enum effects? new List<>(ActiveEffect.Stunned), then no move;

        animalHero = chosenAnimalHero;
        health = chosenAnimalHero.defaultHealth;
        shield = chosenAnimalHero.defaultShield;
        actionPoint = chosenAnimalHero.startingActionPoint;

        cardDeck = animalHeroCardDeck;
    }

    public void AddCardsToHand(List<Card> cards) 
    {
        foreach (var card in cards)
        {
            playerHand.Add(card);
        }
    }

    public void RemoveCardsFromHand(List<Card> cards) 
    {
        foreach (var card in cards) 
        { 
            playerHand.Remove(card);
        }
    }
}
