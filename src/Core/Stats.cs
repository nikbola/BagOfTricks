using BagOfTricks.Debug;
using BagOfTricks.Extensions;
using Game;
using System.Linq;

namespace BagOfTricks.Core
{
    internal static class Stats
    {
        public static PartyMemberAI[] partyMembers;

        public static void ClearPartyMembers()
        {
            partyMembers = new PartyMemberAI[0];
        }

        public static PartyMemberAI[] GetPartyMembers()
        {
            if (partyMembers != null && partyMembers.Length > 0)
                return partyMembers;

            partyMembers = UnityEngine.Object
                .FindObjectsOfType(typeof(PartyMemberAI))
                .Cast<PartyMemberAI>()
                .ToArray();

            string[] names = partyMembers.GetNames();
            for (int i = 0; i < names.Length; i++)
            {
                Debug.Logger.Write<Info>($"Found party member: {names}");   
            }

            return partyMembers;
        }

        public static int GetBaseAttributeScore(CharacterStats.AttributeScoreType type, PartyMemberAI partyMember)
        {
            CharacterStats stats = partyMember.GetComponent<CharacterStats>();
            return stats.GetBaseAttributeScore(type);
        }

        public static void SetBaseAttributeScore(CharacterStats.AttributeScoreType type, PartyMemberAI partyMember, int value)
        {
            CharacterStats stats = partyMember.GetComponent<CharacterStats>();
            switch (type)
            {
                case CharacterStats.AttributeScoreType.Might:
                    stats.BaseMight = value;
                    break;
                case CharacterStats.AttributeScoreType.Resolve:
                    stats.BaseResolve = value;
                    break;
                case CharacterStats.AttributeScoreType.Finesse:
                    stats.BaseFinesse = value;
                    break;
                case CharacterStats.AttributeScoreType.Quickness:
                    stats.BaseQuickness = value;
                    break;
                case CharacterStats.AttributeScoreType.Wits:
                    stats.BaseWits = value;
                    break;
                case CharacterStats.AttributeScoreType.Vitality:
                    stats.BaseVitality = value;
                    break;
                case CharacterStats.AttributeScoreType.Count:
                    break;
                default:
                    break;
            }
        }
    }
}
