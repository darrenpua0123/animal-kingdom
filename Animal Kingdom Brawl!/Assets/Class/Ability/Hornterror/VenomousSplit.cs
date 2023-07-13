using System.Collections.Generic;

public class VenomousSplit : Ability 
{
    public override AbilityType abilityType { get; set; } = AbilityType.TargetAllPlayers;
    public override string description { get; set; } = "";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        int damage = 1;

        foreach (Player target in targetPlayers) 
        {
            if (target.activeEffects.Contains(ActiveEffect.Trap_BookOfCorruption))
            {
                continue;
            }
            else 
            {
                target.health -= damage;

                if (target.health <= 0) 
                { 
                    target.isKnockedOut = true;
                }
            }
        }
    }
}