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
    public CardType CardTypeEnum { get; set; }

    public Card(Sprite cardFrontSprite, Sprite cardBackSprite, CardType cardTypeEnum) 
    {
       this.CardFrontSprite = cardFrontSprite;
       this.CardBackSprite = cardBackSprite;
       this.CardTypeEnum = cardTypeEnum;
    }
}
