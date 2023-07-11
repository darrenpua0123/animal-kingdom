using System.Collections.Generic;

public class BearTrap : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.NonTargetable;
    public override string description { get; set; } = "";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        int damage = 5;

        if (caster.activeEffects.Contains(ActiveEffect.Beedle_Levitation) || caster.activeEffects.Contains(ActiveEffect.Trap_BookOfCorruption))
        {
            return;
        }

        if (caster.activeEffects.Contains(ActiveEffect.Piggion_ThickSkin) && caster.shield > 0)
        {
            return;
        }
        else 
        { 
            caster.TakeDamage(damage);        
        }
    }
}
