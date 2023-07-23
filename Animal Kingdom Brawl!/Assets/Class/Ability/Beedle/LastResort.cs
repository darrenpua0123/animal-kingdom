using System.Collections.Generic;

public class LastResort : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.AllCharacterSingleTargetable;
    public override string description { get; set; } = "Attack 8 damage to a character, then attack 5 damage to yourself.";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        int damage = 8;
        int selfDamage = 5;

        if (caster.activeEffects.Contains(ActiveEffect.Artifact_Expresso))
        {
            damage--;
            selfDamage--;
        }

        // Damage enemy first
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

        // Damage self
        if (caster.activeEffects.Contains(ActiveEffect.Trap_BookOfCorruption))
        {
            return;
        }
        else if (caster.activeEffects.Contains(ActiveEffect.Piggion_ThickSkin) && caster.shield > 0)
        {
            return;
        }
        else 
        {
            caster.TakeDamage(selfDamage);

            if (caster.health <= 0) 
            {
                caster.isKnockedOut = true;
            }
        }
    }
}