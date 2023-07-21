using System.Collections.Generic;
using UnityEngine;

public class PainAndGain : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.NonTargetable;
    public override string description { get; set; } = "Gain shield amount for every card you have in your hand.";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        int shieldAmount = 0;

        foreach (Card card in caster.playerHandDeck.GetAllCards()) 
        {
            shieldAmount++;
        }

        caster.shield += shieldAmount;

        caster.actionPoint++;
    }
}