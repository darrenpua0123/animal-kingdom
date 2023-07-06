using System.Collections.Generic;

public enum AbilityType 
{ 
    SingleTargetable,
    MultiTargetable,
    NonTargetable,
    CardViewable,
    CardSelectable,
    Reinsertable
}

public abstract class Ability 
{
    public abstract List<AbilityType> abilityType { get; set; }

    protected Ability() 
    {
        //when play
        //1) Check card's ability type ^
        //2) Prompt appropriate UI selection
        //3) activate card ability
        //    1) when activate, check player active effect
        //    2) execute effect
        //    3) update player effect again
    }

    public abstract void ActivateAbility(Player caster, List<Player> targets);
}