using System.Collections.Generic;

public class Expresso : Ability 
{
    public override AbilityType abilityType { get; set; } = AbilityType.NonTargetable;
    public override string description { get; set; } = "";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        caster.actionPoint += 3;

        caster.activeEffects.Add(ActiveEffect.Artifact_Expresso);
    }
}