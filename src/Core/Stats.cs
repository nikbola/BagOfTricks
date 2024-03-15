using Game;
using System.Linq;

namespace BagOfTricks.Core
{
    internal static class Stats
    {
        public static PartyMemberAI[] partyMembers;

        public static PartyMemberAI[] GetPartyMembers()
        {
            if (partyMembers != null && partyMembers.Length > 0)
            {
                return partyMembers;
            }

            return UnityEngine.Object
                .FindObjectsOfType(typeof(PartyMemberAI))
                .Cast<PartyMemberAI>()
                .ToArray();
        }
    }
}
