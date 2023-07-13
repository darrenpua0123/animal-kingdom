using System.Collections.Generic;

public class HeatWave : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.TargetAllPlayers;
    public override string description { get; set; } = "";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        int noShieldDamage = 2;

        foreach (Player target in targetPlayers) 
        {
            if (target.activeEffects.Contains(ActiveEffect.Piggion_ThickSkin) && target.shield >= 1) 
            {
                continue;
            }

            if (target.shield >= 1)
            {
                target.shield = 0;
            }
            else 
            {
                if (target.activeEffects.Contains(ActiveEffect.Trap_BookOfCorruption))
                {
                    continue;
                }
                else 
                { 
                    target.TakeDamage(noShieldDamage);
                }
            }
        }
    }
}