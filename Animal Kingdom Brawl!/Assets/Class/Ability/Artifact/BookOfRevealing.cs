﻿using System.Collections.Generic;

public class BookOfRevealing : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.FiveCardViewable;
    public override string description { get; set; } = "";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        
    }
}