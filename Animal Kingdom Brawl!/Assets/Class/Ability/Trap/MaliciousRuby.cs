using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MaliciousRuby : Ability
{
    public override AbilityType abilityType { get; set; } = AbilityType.TargetAllPlayers;
    public override string description { get; set; } = "";

    public override void ActivateAbility(Player caster, List<Player> targetPlayers)
    {
        List<Player> validPlayers = new List<Player>(targetPlayers);

        // Check self first
        if (caster.activeEffects.Contains(ActiveEffect.Beedle_Levitation))
        {
            return;
        }

        // Check other player
        for (int i = 0; i < targetPlayers.Count; i++)
        {
            // Check valid players first
            if (targetPlayers[i].isKnockedOut || targetPlayers[i].activeEffects.Contains(ActiveEffect.Beedle_Levitation))
            {
                validPlayers.Remove(targetPlayers[i]);
                continue;
            }
        }

        foreach (Player player in validPlayers)
        {
            //player.health = 10;
        }

        // Iterate all valid player to swap health
        List<int> updatedHealth = new List<int>(validPlayers.Count);

        for (int i = 0; i < validPlayers.Count; i++)
        {
            int nextIndex = (i + 1) % validPlayers.Count;

            updatedHealth.Add(validPlayers[nextIndex].health);
        }

        for (int i = 0; i < validPlayers.Count; i++)
        {
            validPlayers[i].health = updatedHealth[i];
        }
    }
}
