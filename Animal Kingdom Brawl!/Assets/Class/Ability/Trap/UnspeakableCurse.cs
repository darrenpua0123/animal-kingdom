using System.Collections.Generic;

public class UnspeakableCurse : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.NonTargetable;
    public override string description { get; set; } = "";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        // Only Beedle levitation is immune
        if (caster.activeEffects.Contains(ActiveEffect.Beedle_Levitation))
        {
            return;
        }
        else 
        {
            caster.health = 0;
            caster.isKnockedOut = true;
        }
    }
}
