using System.Collections.Generic;

public class HealingPotion : Ability 
{
    public override AbilityType abilityType { get; set; } = AbilityType.NonTargetable;
    public override string description { get; set; } = "";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        int healPoint = 5;

        caster.health += healPoint;
    }
}