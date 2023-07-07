using System.Collections.Generic;

public class CatDoWhatCatWant : Ability
{
    public override List<AbilityType> abilityType { get; set; } = new List<AbilityType>() { AbilityType.CardSelectable };
    public override string description { get; set; } = "You can select and steal 1 card from each opponent's hand.";
    
    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        // TODO: Continue here, implement the execution and function
    }
}