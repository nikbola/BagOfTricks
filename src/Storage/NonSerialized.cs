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
        internal static Mover[] s_Movers = new Mover[0];

        internal static TopBarCategory s_SelectedTopBarCategory = TopBarCategory.Main;

        internal static List<Tuple<string, string, string>> s_AchievementInfo = new List<Tuple<string, string, string>>();

#region Movement
        internal static float DefaultRunSpeed = 8f;
        internal static float DefaultWalkSpeed = 4f;
        internal static float DefaultStealthSpeed = 2.5f;
        internal static readonly float MinMovementSpeed = 1f;
        internal static readonly float MaxMovementSpeed = 25f;
#endregion

#region ToggleDeltaValues
        internal static bool DeltaGodmodeEnabled = false;
        internal static bool DeltaBlockTelemetry = false;
        internal static bool DeltaInvisibilityEnabled = false;
#endregion

        public enum TopBarCategory 
        {
            Main,
            Logs
        }
    }
}
