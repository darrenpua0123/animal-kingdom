using System.Collections.Generic;

public class CatArmor : Ability
{
    public override List<AbilityType> abilityType { get; set; } = new List<AbilityType>() { AbilityType.NonTargetable };
    public override string description { get; set; } = "";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        caster.shield++;
        caster.actionPoint++;
    }
}