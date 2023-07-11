using System.Collections.Generic;

public class PiercingClawOfPaw : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.NonTargetable;
    public override string description { get; set; } = "Your attack ignores all shields this turn.";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        caster.actionPoint++;
        caster.activeEffects.Add(ActiveEffect.Catomic_PiercingClawOfPaw);
    }
}