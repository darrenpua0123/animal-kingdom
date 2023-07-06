using System.Collections.Generic;

public class CatcatSlash : Ability 
{
    public override List<AbilityType> abilityType { get; set; } = new List<AbilityType>() { AbilityType.SingleTargetable };

    public override void ActivateAbility(Player caster, List<Player> target)
    {
        // TODO: Check ActiveEffect
        target[0].TakeDamage(3);
        // Update ActivateEffect
    }
}