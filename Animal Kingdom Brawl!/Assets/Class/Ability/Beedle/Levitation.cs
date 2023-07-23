using System.Collections.Generic;

public class Levitation : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.NonTargetable;
    public override string description { get; set; } = "You take no effects from any Trap card until your next turn.";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        caster.activeEffects.Add(ActiveEffect.Beedle_Levitation);
        caster.actionPoint += 1;
    }
}