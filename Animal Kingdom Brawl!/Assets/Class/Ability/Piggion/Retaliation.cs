using System.Collections.Generic;

public class Retaliation : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.AllCharacterSingleTargetable;
    public override string description { get; set; } = "Destroy all your shield and deal the destroyed shield amount to a character (Thick Skin is not affected).";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        caster.health++;

        int shieldDamage = 0;

        // Calculate and check the shield damage
        if (caster.activeEffects.Contains(ActiveEffect.Piggion_ThickSkin))
        {
            shieldDamage += caster.shield;
        }
        else 
        {
            shieldDamage += caster.shield;
            caster.shield = 0;
        }

        // Check caster's active effect
        if (caster.activeEffects.Contains(ActiveEffect.Artifact_Expresso))
        {
            if (!(shieldDamage <= 0)) 
            {
                shieldDamage--;
            }
        }

        // Check all target player's active effect
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
                    targetPlayer.health -= shieldDamage;
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
                    targetPlayer.health -= shieldDamage;
                }
                else
                {
                    targetPlayer.TakeDamage(shieldDamage);
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