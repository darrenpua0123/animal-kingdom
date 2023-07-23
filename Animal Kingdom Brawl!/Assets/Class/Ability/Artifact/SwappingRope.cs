using System;
using System.Collections.Generic;
using System.Diagnostics;

public class SwappingRope : Ability 
{
    public override AbilityType abilityType { get; set; } = AbilityType.AllOpponentSingleTargetable;
    public override string description { get; set; } = "";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        if (caster.playerHandDeck.GetAllCards().Count <= 0 || targetPlayers[0].playerHandDeck.GetAllCards().Count <= 0) 
        {
            // Do not execute if either of the player's hand doesn't contain any cards
            return;
        }

        Random rand = new Random();

        // Get random card from caster's hand
        List<Card> casterHandCards = caster.playerHandDeck.GetAllCards();
        Card casterRandCard = casterHandCards[rand.Next(0, casterHandCards.Count)];
        caster.playerHandDeck.RemoveSingleCard(casterRandCard);

        // Get random card from target's hand
        List<Card> targetHandCards = targetPlayers[0].playerHandDeck.GetAllCards();
        Card targetRandCard = targetHandCards[rand.Next(0, targetHandCards.Count)];
        targetPlayers[0].playerHandDeck.RemoveSingleCard(targetRandCard);

        // Swap them together
        caster.playerHandDeck.AddSingleCard(targetRandCard);
        targetPlayers[0].playerHandDeck.AddSingleCard(casterRandCard);
    }
}