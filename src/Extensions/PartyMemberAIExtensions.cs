using BagOfTricks.Storage;
using Game;

namespace BagOfTricks.Extensions
{
    internal static class PartyMemberAIExtensions
    {
        public static string[] GetNames(this PartyMemberAI[] partyMemberAIs)
        {
            string[] cNames = new string[partyMemberAIs.Length];
            for (int i = 0; i < partyMemberAIs.Length; i++)
            {
                var cStats = partyMemberAIs[i].GetComponent<CharacterStats>();
                cNames[i] = partyMemberAIs[i].name.StartsWith("Comp") ? cStats.DisplayName.ToString() : cStats.OverrideName;
            }
            return cNames;
        }
    }
}
