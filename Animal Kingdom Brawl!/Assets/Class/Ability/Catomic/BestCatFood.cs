using System.Collections.Generic;

public class BestCatFood : Ability
{
    public override List<AbilityType> abilityType { get; set; } = new List<AbilityType>() { AbilityType.NonTargetable };
    public override string description { get; set; } = "";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        caster.health += 3;
        caster.shield++;
        caster.playerHandDeck.AddCards(caster.cardDeck.DrawCards(1));
    }
}