using BagOfTricks.Debug;
using BagOfTricks.Extensions;
using BagOfTricks.Storage;
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
            if (GameState.s_playerCharacter == null)
                return;

            Faction playerFaction = GameState.s_playerCharacter.GetComponent<Faction>();
            Faction[] arr = Object.FindObjectsOfType<Faction>();

            int killCount = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                Faction enemyFaction = arr[i];
                if (!enemyFaction.IsHostile(playerFaction))
                    continue;
                
                Health enemyHealth = enemyFaction.GetComponent<Health>();
                enemyHealth?.ApplyDamageDirectly(enemyHealth.CurrentHealth);
                killCount++;
            }

            Debug.Logger.Write<Success>($"Successfully killed {killCount} enemies");
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
            if (NonSerialized.s_PartyMembers.IsNullOrEmpty())
                return;

            PartyMemberAI[] arr = NonSerialized.s_PartyMembers;
            foreach (PartyMemberAI partyMemberAI in arr)
            {
                Health component = partyMemberAI.GetComponent<Health>();
                if (component == null)
                    continue;
                    
                component.TakesDamage = !enabled;
            }

            Debug.Logger.Write<Success>("Successfully toggled god mode");
        }

        /// <summary>
        /// Makes the player "invisible" to enemies, and completely untargetable. This differs from God Mode in the sense that 
        /// in God Mode, you can still be attacked, but don't take any damage.
        /// </summary>
        /// <param name="invisible">Whether to enable or disable</param>
        public static void ToggleInvisibility(bool invisible)
        {
            if (NonSerialized.s_PartyMembers.IsNullOrEmpty())
                return;

            PartyMemberAI[] partyMembers = NonSerialized.s_PartyMembers;
            foreach (var partyMemberAI in partyMembers)
            {
                Health component = partyMemberAI.GetComponent<Health>();
                if (component == null)
                    continue;

                component.Targetable = !invisible;
            }
            Debug.Logger.Write<Success>("Successfully toggled invisibility");
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

            // The audio player needs an item to determine what sound it should play, so we have to create a temporary one
            GameObject tempItem = new GameObject("Temp_Item", typeof(Item));
            Item item = tempItem.GetComponent<Item>();
            item.InventoryDragDropSound = Item.UIDragDropSoundType.Loot_Keys;
            GlobalAudioPlayer.Instance.Play(item, GlobalAudioPlayer.UIInventoryAction.PickUpItem);
            Object.Destroy(tempItem);

            Debug.Logger.Write<Success>($"Successfully added {value} copper");
        }
    }
}
