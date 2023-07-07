using System.Collections.Generic;

public class ThiefInformation : Ability
{
    public override List<AbilityType> abilityType { get; set; } = new List<AbilityType>() { AbilityType.SingleTargetable };
    public override string description { get; set; } = "";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        caster.playerHandDeck.AddCards(caster.cardDeck.DrawCards(3));

        int damage = 1;

        if (caster.activeEffects.Contains(ActiveEffect.Artifact_Expresso))
        {
            damage--;
        }

        foreach (var targetPlayer in targetPlayers)
        {
            if (targetPlayer.activeEffects.Contains(ActiveEffect.Trap_BookOfCorruption))
            {
                return;
            }

            if (targetPlayer.activeEffects.Contains(ActiveEffect.Piggion_ThickSkin) && targetPlayer.shield > 0)
            {
                if (caster.activeEffects.Contains(ActiveEffect.Catomic_PiercingClawOfPaw))
                {
                    targetPlayer.health -= damage;
                }
                else
                {
                    return;
                }
            }
            else
            {
                targetPlayer.TakeDamage(damage);
            }

            if (targetPlayer.health <= 0)
            {
                targetPlayer.isKnockedOut = true;
                caster.knockoutCounter++;
            }
        }
    }
}