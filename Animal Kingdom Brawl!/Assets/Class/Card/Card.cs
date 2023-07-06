using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Card
{
    public Sprite CardFrontSprite { get; set; }
    public Sprite CardBackSprite { get; set; }
    public CardCategory CardCategory { get; set; }
    public CardType CardType { get; set; }
    public Ability CardAbility { get; set; }

    public Card(Sprite cardFrontSprite, Sprite cardBackSprite, CardCategory cardCategoryEnum, Ability cardAbility) 
    {
       this.CardFrontSprite = cardFrontSprite;
       this.CardBackSprite = cardBackSprite;
       this.CardCategory = cardCategoryEnum;
       this.CardAbility = cardAbility;
       // Newest
    }

    public Card(Sprite cardFrontSprite, Sprite cardBackSprite, CardCategory cardCategoryEnum, CardType cardType)
    {
        this.CardFrontSprite = cardFrontSprite;
        this.CardBackSprite = cardBackSprite;
        this.CardCategory = cardCategoryEnum;
        this.CardType = cardType;
        // TODO: Remove onced all Ability is completed
    }
}
