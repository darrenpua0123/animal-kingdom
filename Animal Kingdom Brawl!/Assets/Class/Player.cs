
using System;
using System.Collections.Generic;

public class Player
{
    public int health;
    public int shield;
    public int actionPoint;

    public int knockoutCounter;
    public int relicCounter;

    public List<string> activeEffects;

    public Player()
    {
        // TODO: Use this for Game's player uses. coins, game card, and win record etc
        
        // animalhero, AnimalHero
        // health, int -> from animlahero.defaulthealth
        // shield, int -> from animalhero.deffaultShield
        // actionPoint, int -> from animalhero.defaultAP
        // cardDeck, CardDeck -> from animalhero.defaultCardDeck, if need to modify like player hide card inside, can modify
        // playerHand, List<Card> -> after drawCard from cardDeck / TrapCardDeck
        // knockout counter, int
        // relic counter, int
        // ActiveEffect, ? maybe enum effects? new List<>(ActiveEffect.Stunned), then no move;
    }

    public Player(AnimalHero animalHero, CardDeck animalHeroCardDeck, List<Card> cardHandDeck) 
    {
        
    
    }
}
