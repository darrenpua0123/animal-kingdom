using System.Collections.Generic;

public class DragonsScale : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.NonTargetable;
    public override string description { get; set; } = "Heal yourself for each knockout you have.";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        int healAmount = caster.knockoutCounter;
        caster.health += healAmount;

        caster.shield += 2;
    }
}