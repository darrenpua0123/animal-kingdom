using System.Collections.Generic;

public class TreasuresRight : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.AllCharacterSingleTargetable;
    public override string description { get; set; } = "For each relic you collected, attack a character for 3* that amount. Attack for 1 damage if no relic is collected.";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        caster.shield += 2;
        caster.health += 2;

        int damage = (caster.relicCounter > 0) ? (caster.relicCounter * 3) : 1;

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