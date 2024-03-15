using BagOfTricks.Debug;
using BagOfTricks.UI;
using Game;
using UnityEngine;

namespace BagOfTricks.Storage
{
    internal static class NonSerialized
    {
        internal static string AddCurrencyAmount = "500";
        internal static Vector2 ScrollPosition = Vector2.zero;

        internal static PartyMemberAI[] partyMembers = new PartyMemberAI[0];

        internal static TopBarCategory SelectedTopBarCategory = TopBarCategory.Main;

        public enum TopBarCategory 
        {
            Main,
            Logs
        }
    }
}
