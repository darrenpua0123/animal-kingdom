using System;
using System.Collections.Generic;
using System.Diagnostics;

public class CatDoWhatCatWant : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.TargetAllOpponents;
    public override string description { get; set; } = "You randomly steal 1 card from each opponent’s hand.";
    
    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        caster.actionPoint++;

        List<Card> stolenCards = new List<Card>();
        Random rand = new Random();

        foreach (Player player in targetPlayers) 
        {
            List<Card> handCards = player.playerHandDeck.GetAllCards();
            
            if (player.isKnockedOut || handCards.Count <= 0) 
            {
                continue;
            }

            int randNo = rand.Next(1, handCards.Count);
            
            stolenCards.Add(handCards[randNo]);
            player.playerHandDeck.RemoveSingleCard(handCards[randNo]);
        }

        caster.playerHandDeck.AddCards(stolenCards);
    }
}