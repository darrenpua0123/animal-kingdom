﻿using System.Collections.Generic;

public class BestCatFood : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.NonTargetable;
    public override string description { get; set; } = "";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        caster.health += 3;
        caster.shield++;

        if (caster.cardDeck.GetAllCards().Count <= 0)
        {
            caster.cardDeck.AddCards(caster.discardDeck.GetAllCards());
            caster.cardDeck.ShuffleCards();
            //TODO: Test
            caster.discardDeck = new CardDeck();
        }
        caster.playerHandDeck.AddCards(caster.cardDeck.DrawCards(1));
    }
}