using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DragonsRoar : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.TargetAllCharacters;
    public override string description { get; set; } = "Attack everyone for 1 damage. For every attacked player, heal yourself for that amount. ";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        int damage = 1;
        int healAmount = 0;

        if (caster.activeEffects.Contains(ActiveEffect.Artifact_Expresso))
        {
            damage--;
        }

        foreach (var targetPlayer in targetPlayers)
        {
            if (targetPlayer == caster) 
            {
                continue;
            }

            if (targetPlayer.isKnockedOut)
            {
                continue;
            }

            if (targetPlayer.activeEffects.Contains(ActiveEffect.Trap_BookOfCorruption))
            {
                continue;
            }

            if (targetPlayer.activeEffects.Contains(ActiveEffect.Piggion_ThickSkin) && targetPlayer.shield > 0)
            {
                if (caster.activeEffects.Contains(ActiveEffect.Catomic_PiercingClawOfPaw))
                {
                    targetPlayer.health -= damage;
                    healAmount++;
                }
                else
                {
                    continue;
                }
            }
            else
            {
                if (caster.activeEffects.Contains(ActiveEffect.Catomic_PiercingClawOfPaw))
                {
                    targetPlayer.health -= damage;
                    healAmount++;
                }
                else
                {
                    targetPlayer.TakeDamage(damage);
                    healAmount++;
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

        caster.health += healAmount;
        caster.actionPoint++;
    }
}