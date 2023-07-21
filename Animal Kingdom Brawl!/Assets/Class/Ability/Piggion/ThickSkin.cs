using System.Collections.Generic;

public class ThickSkin : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.NonTargetable;
    public override string description { get; set; } = "Your shield until the next turn is undestroyable.";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        caster.activeEffects.Add(ActiveEffect.Piggion_ThickSkin);
    }
}