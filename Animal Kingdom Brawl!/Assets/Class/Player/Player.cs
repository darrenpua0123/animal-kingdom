
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using UnityEngine.Timeline;

public class Player
{
    public AnimalHero animalHero;
    public bool isKnockedOut = false;
    public int health;
    public int shield;
    public int actionPoint;
    public int knockoutCounter = 0;
    public int relicCounter = 0;

    public CardCategory heroCardCategory;
    public CardDeck cardDeck;
    public CardDeck discardDeck = new CardDeck();
    public CardDeck playerHandDeck = new CardDeck();

    public List<ActiveEffect> activeEffects = new List<ActiveEffect>();
    
    public Player(AnimalHero chosenAnimalHero, CardDeck animalHeroCardDeck) 
    {
        animalHero = chosenAnimalHero;
        SetHeroCardCategory();
        health = chosenAnimalHero.defaultHealth;
        shield = chosenAnimalHero.defaultShield;
        actionPoint = chosenAnimalHero.startingActionPoint;

        cardDeck = animalHeroCardDeck;
    }

    private void SetHeroCardCategory() 
    {
        if (animalHero is Beedle)
        {
            heroCardCategory = CardCategory.Beedle;
        }
        else if (animalHero is Catomic)
        {
            heroCardCategory = CardCategory.Catomic;
        }
        else if (animalHero is Pandragon)
        {
            heroCardCategory = CardCategory.Pandragon;
        }   
        else if (animalHero is Piggion)
        {
            heroCardCategory = CardCategory.Piggion;
        }
        else if (animalHero is Hornterror) 
        {
            heroCardCategory = CardCategory.Hornterror;
        }
    }

    public void TakeDamage(int damage) 
    {
        int actualDamage = damage - shield;

        if (actualDamage > 0) // Damage value is HIGHER than shield
        {
            shield = 0;
            health -= actualDamage;
        }
        else // Damage value is LOWER than shield
        {
            shield -= damage;
        }

        if (health <= 0) 
        {
            health = 0;
            isKnockedOut = true;
        }
    }

    public void OnRevive() 
    {
        isKnockedOut = false;
        activeEffects.Clear();
        health = animalHero.defaultHealth;
        shield = animalHero.defaultShield;

        cardDeck.ShuffleCards();
        discardDeck = new CardDeck();

        playerHandDeck.AddCards(cardDeck.DrawCards(3));
    }
}
