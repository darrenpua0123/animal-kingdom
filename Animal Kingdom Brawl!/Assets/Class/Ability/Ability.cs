using System.Collections.Generic;
using UnityEngine;

public enum AbilityType 
{ 
    NonTargetable,
    TargetAllCharacters,
    TargetAllPlayers,
    TargetAllOpponents,
    AllCharacterSingleTargetable,
    AllOpponentSingleTargetable,
    CardViewable,
    // unused
    CardSelectable
}

public abstract class Ability
{
    public abstract AbilityType abilityType { get; set; }
    public abstract string description { get; set; }

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

    public abstract void ActivateAbility(Player caster, List<Player> targetPlayers);
}