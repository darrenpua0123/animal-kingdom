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
    ThreeCardViewable,
    FiveCardViewable
}

public abstract class Ability
{
    public abstract AbilityType abilityType { get; set; }
    public abstract string description { get; set; }

    protected Ability() 
    {

    }

    public abstract void ActivateAbility(Player caster, List<Player> targetPlayers);
}