using System.Collections.Generic;

public class Surveillance : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.ThreeCardViewable;
    public override string description { get; set; } = "Reveal the top 3 cards of the chest card deck.";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {

    }
}