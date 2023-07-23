using System.Collections.Generic;

public class Rejuvenate : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.NonTargetable;
    public override string description { get; set; } = "";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        caster.playerHandDeck.AddCards(caster.cardDeck.DrawCards(2));
        caster.health += 2;
    }
}