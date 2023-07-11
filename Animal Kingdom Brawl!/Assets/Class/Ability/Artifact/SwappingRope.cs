using System;
using System.Collections.Generic;
using System.Diagnostics;

public class SwappingRope : Ability 
{
    public override AbilityType abilityType { get; set; } = AbilityType.SingleTargetable;
    public override string description { get; set; } = "";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        if (caster.playerHandDeck.GetAllCards().Count <= 0 || targetPlayers[0].playerHandDeck.GetAllCards().Count <= 0) 
        {
            return;
        }

        Random rand = new Random();

        List<Card> casterHandCards = caster.playerHandDeck.GetAllCards();
        Card casterRandCard = casterHandCards[rand.Next(0, casterHandCards.Count)];
        caster.playerHandDeck.RemoveSingleCard(casterRandCard);

        List<Card> targetHandCards = targetPlayers[0].playerHandDeck.GetAllCards();
        Card targetRandCard = targetHandCards[rand.Next(0, targetHandCards.Count)];
        targetPlayers[0].playerHandDeck.RemoveSingleCard(targetRandCard);

        caster.playerHandDeck.AddSingleCard(targetRandCard);
        targetPlayers[0].playerHandDeck.AddSingleCard(casterRandCard);
    }
}