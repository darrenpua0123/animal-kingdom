using System.Collections.Generic;

public class SuitUp : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.NonTargetable;
    public override string description { get; set; } = "";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        int shieldValue = 3;

        caster.shield += shieldValue;
    }
}