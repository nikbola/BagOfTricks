using Game;
using System.Collections.Generic;
using UnityEngine;

namespace BagOfTricks.Storage
{
    internal static class NonSerialized
    {
        internal static string s_AddCurrencyAmount = "500";
        internal static Vector2 s_ScrollPosition = Vector2.zero;

        internal static PartyMemberAI[] s_PartyMembers = new PartyMemberAI[0];

        internal static TopBarCategory s_SelectedTopBarCategory = TopBarCategory.Main;

        internal static List<Tuple<string, string, string>> s_AchievementInfo = new();

        public enum TopBarCategory 
        {
            Main,
            Logs
        }
    }
}
