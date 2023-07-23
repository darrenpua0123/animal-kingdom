using System;
using System.Collections.Generic;

public class DracosMight : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.TargetAllCharacters;
    public override string description { get; set; } = "Attack everyone for 4 damage.";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        int damage = 4;

        if (caster.activeEffects.Contains(ActiveEffect.Artifact_Expresso)) 
        {
            damage--;
        }

        foreach (Player target in targetPlayers)
        {
            if (target == caster) 
            {
                continue;
            }

            if (target.isKnockedOut)
            {
                continue;
            }

            if (target.activeEffects.Contains(ActiveEffect.Trap_BookOfCorruption) ||
                (target.activeEffects.Contains(ActiveEffect.Piggion_ThickSkin) && target.shield >= 1))
            {
                continue;
            }
            else
            {
                if (caster.activeEffects.Contains(ActiveEffect.Catomic_PiercingClawOfPaw))
                {
                    target.health -= damage;
                }
                else 
                {
                    target.TakeDamage(damage);
                }
            }

            if (target.health <= 0)
            {
                target.isKnockedOut = true;
                caster.knockoutCounter++;

                if (target.relicCounter > 0)
                {
                    caster.relicCounter++;
                    target.relicCounter--;
                }
            }
        }
    }
}