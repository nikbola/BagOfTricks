using BagOfTricks.Extensions;
using Game;
using UnityEngine;

namespace BagOfTricks.Core
{
    internal static class Cheats
    {
        /// <summary>
        /// Finds all nearby enemies that are hostile towards the player and kills them
        /// </summary>
        public static void KillAllEnemies()
        {
            if (!GameState.s_playerCharacter)
                return;

            Faction playerFaction = GameState.s_playerCharacter.GetComponent<Faction>();
            Faction[] arr = Object.FindObjectsOfType<Faction>();
            for (int i = 0; i < arr.Length; i++)
            {
                Faction enemyFaction = arr[i];
                if (!enemyFaction.IsHostile(playerFaction))
                    continue;
                
                Health enemyHealth = enemyFaction.GetComponent<Health>();
                enemyHealth.KillCheat();
            }
        }

        /// <summary>
        /// Removes the fog of war from the current map
        /// </summary>
        public static void ClearFogOfWar() => FogOfWar.Instance?.QueueDisable();

        /// <summary>
        /// Enables or disables God Mode. In God Mode the character can not take damage.
        /// </summary>
        /// <param name="enabled">Whether to enable or disable</param>
        public static void ToggleGodMode(bool enabled)
        {
            if (Stats.partyMembers.IsNullOrEmpty())
                return;

            PartyMemberAI[] arr = Stats.partyMembers;
            foreach (PartyMemberAI partyMemberAI in arr)
            {
                Health component = partyMemberAI.GetComponent<Health>();
                if (component == null)
                    continue;
                    
                component.TakesDamage = !enabled;
            }
        }

        /// <summary>
        /// Makes the player "invisible" to enemies, and completely untargetable. This differs from God Mode in the sense that 
        /// in God Mode, you can still be attacked, but don't take any damage.
        /// </summary>
        /// <param name="invisible">Whether to enable or disable</param>
        public static void ToggleInvisibility(bool invisible)
        {
            if (Stats.partyMembers.IsNullOrEmpty())
                return;

            PartyMemberAI[] partyMembers = Stats.partyMembers;
            foreach (var partyMemberAI in partyMembers)
            {
                Health component = partyMemberAI.GetComponent<Health>();
                if (component == null)
                    return;

                component.Targetable = !invisible;
            }
        }

        /// <summary>
        /// Adds specified amount of currency to the player's inventory
        /// </summary>
        /// <param name="value">The amount of currency to add</param>
        public static void AddCurrency(int value)
        {
            Player playerCharacter = GameState.s_playerCharacter;
            if (playerCharacter == null)
                return;

            PlayerInventory component = playerCharacter.GetComponent<PlayerInventory>();
            if (component == null)
                return;

            component.currencyTotalValue.amount += value;
        }
    }
}
