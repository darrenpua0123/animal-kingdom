using System.Collections.Generic;

public class WarriorsShield : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.NonTargetable;
    public override string description { get; set; } = "";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        int shieldValue = 4;

        caster.shield += shieldValue;
    }
}