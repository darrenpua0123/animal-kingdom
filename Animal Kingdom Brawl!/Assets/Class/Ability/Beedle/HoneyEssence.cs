using System.Collections.Generic;

public class HoneyEssence : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.NonTargetable;
    public override string description { get; set; } = "";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        caster.actionPoint += 1;
        caster.shield += 1;
        caster.playerHandDeck.AddCards(caster.cardDeck.DrawCards(1));
    }
}