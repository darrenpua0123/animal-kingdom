using System.Collections.Generic;

public class TheMimicMirror : Ability 
{
    public override AbilityType abilityType { get; set; } = AbilityType.AllCharacterSingleTargetable;
    public override string description { get; set; } = "";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        caster.health = targetPlayers[0].health;
    }
}