using System.Collections.Generic;
using UnityEngine;

public class SinisterStare : Ability 
{
    public override AbilityType abilityType { get; set; } = AbilityType.TargetAllPlayers;
    public override string description { get; set; } = "";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        int damage = 2;

        foreach (Player target in targetPlayers)
        {
            if (target.activeEffects.Contains(ActiveEffect.Trap_BookOfCorruption) ||
                (target.activeEffects.Contains(ActiveEffect.Piggion_ThickSkin) && target.shield >= 1))
            {
                continue;
            }
            else if (target.isKnockedOut) 
            {
                continue;
            }
            else
            {
                target.TakeDamage(damage);
            }
        }
    }
}