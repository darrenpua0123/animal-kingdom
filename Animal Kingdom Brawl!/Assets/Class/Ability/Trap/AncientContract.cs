using System;
using System.Collections.Generic;
using System.Diagnostics;

public class AncientContract : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.TargetAllOpponents;
    public override string description { get; set; } = "";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        int damage = 3;

        if (caster.activeEffects.Contains(ActiveEffect.Beedle_Levitation))
        {
            return;
        }

        // Cannot give 3 cards randomly, so take damage
        if (caster.playerHandDeck.GetAllCards().Count < 3)
        {
            if (caster.activeEffects.Contains(ActiveEffect.Trap_BookOfCorruption) || 
                (caster.activeEffects.Contains(ActiveEffect.Piggion_ThickSkin) && caster.shield > 0))
            {
                return;
            }
            else 
            {
                caster.TakeDamage(damage);
            }
        }
        // Can give 3 cards randomly
        else
        {
            Random rand = new Random();

            foreach (Player target in targetPlayers) 
            {
                int cardIndex = rand.Next(0, caster.playerHandDeck.GetAllCards().Count);

                Card removedCard = caster.playerHandDeck.GetAllCards()[cardIndex];
                caster.playerHandDeck.RemoveSingleCard(removedCard);
                target.playerHandDeck.AddSingleCard(removedCard);
            }
        }
    }
}
