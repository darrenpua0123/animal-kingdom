using System.Collections.Generic;

public class BookOfCorruption : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.NonTargetable;
    public override string description { get; set; } = "";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        if (caster.activeEffects.Contains(ActiveEffect.Beedle_Levitation))
        {
            return;
        }

        caster.health = 1;
        caster.activeEffects.Add(ActiveEffect.Trap_BookOfCorruption);
    }
}
