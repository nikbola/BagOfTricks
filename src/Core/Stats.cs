using Game;

namespace BagOfTricks.Core
{
    internal static class Stats
    {
        public static object[] partyMembers;

        public static object[] GetPartyMembers()
        {
            if (partyMembers != null && partyMembers.Length > 0)
            {
                return partyMembers;
            }
            else
            {
                object[] arr = UnityEngine.Object.FindObjectsOfType(typeof(PartyMemberAI));
                if (arr == null || arr.Length == 0)
                {
                    return null;
                }
                return arr;
            }
        }
    }
}
