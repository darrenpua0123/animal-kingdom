using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;

public class CalamityBomb : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.TargetAllCharacters;
    public override string description { get; set; } = "";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        int selfDamage = 4;
        int charDamage = 2;

        // Check self first
        if (caster.activeEffects.Contains(ActiveEffect.Beedle_Levitation))
        {
            return;
        }

        if (caster.activeEffects.Contains(ActiveEffect.Piggion_ThickSkin) && caster.shield > 0 || caster.activeEffects.Contains(ActiveEffect.Trap_BookOfCorruption))
        {

        }
        else 
        {
            caster.TakeDamage(selfDamage);
        }

        // Check other character
        List<Player> eligibleTargetPlayers = new List<Player>(targetPlayers);
        eligibleTargetPlayers.Remove(caster);

        foreach (Player target in eligibleTargetPlayers) 
        {
            if (target.isKnockedOut) 
            {
                continue;
            }

            if (target.activeEffects.Contains(ActiveEffect.Beedle_Levitation)) 
            {
                continue;
            }

            if (target.activeEffects.Contains(ActiveEffect.Piggion_ThickSkin) && target.shield > 0 || target.activeEffects.Contains(ActiveEffect.Trap_BookOfCorruption))
            {

            }
            else 
            { 
                target.TakeDamage(charDamage);
            }
        }
    }
}
