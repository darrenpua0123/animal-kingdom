using System.Collections.Generic;
using UnityEngine;

public class FlightAndFight : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.AllCharacterSingleTargetable;
    public override string description { get; set; } = "";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        if (caster.cardDeck.GetAllCards().Count <= 0)
        {
            caster.cardDeck.AddCards(caster.discardDeck.GetAllCards());
            caster.cardDeck.ShuffleCards();

            caster.discardDeck = new CardDeck();
        }
        caster.playerHandDeck.AddCards(caster.cardDeck.DrawCards(1));

        int damage = 3;

        if (caster.activeEffects.Contains(ActiveEffect.Artifact_Expresso))
        {
            damage--;
        }

        foreach (var targetPlayer in targetPlayers)
        {
            if (targetPlayer.activeEffects.Contains(ActiveEffect.Trap_BookOfCorruption))
            {
                return;
            }

            if (targetPlayer.activeEffects.Contains(ActiveEffect.Piggion_ThickSkin) && targetPlayer.shield > 0)
            {
                if (caster.activeEffects.Contains(ActiveEffect.Catomic_PiercingClawOfPaw))
                {
                    targetPlayer.health -= damage;
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (caster.activeEffects.Contains(ActiveEffect.Catomic_PiercingClawOfPaw))
                {
                    targetPlayer.health -= damage;
                }
                else
                {
                    targetPlayer.TakeDamage(damage);
                }
            }

            if (targetPlayer.health <= 0)
            {
                targetPlayer.isKnockedOut = true;
                caster.knockoutCounter++;

                if (targetPlayer.relicCounter > 0)
                {
                    caster.relicCounter++;
                    targetPlayer.relicCounter--;
                }
            }
        }
    }
}