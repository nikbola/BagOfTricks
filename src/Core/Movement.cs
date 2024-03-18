using BagOfTricks.Extensions;
using BagOfTricks.Storage;
using Game;

namespace BagOfTricks.Core
{
    internal static class Movement
    {
        public static Mover[] GetMovers(PartyMemberAI[] partyMembers)
        {
            Mover[] movers = new Mover[partyMembers.Length];
            for (int i = 0; i < partyMembers.Length; i++)
            {
                if (partyMembers[i].TryGetComponent<Mover>(out var mover))
                {
                    movers[i] = mover;
                }
            }
            return movers;
        }

        public static void ApplySettings()
        {
            for (int i = 0; i < NonSerialized.s_Movers.Length; i++)
            {
                Mover mover = NonSerialized.s_Movers[i];
                mover.SetRunSpeed(Serialized.RunSpeed);
                mover.SetWalkSpeed(Serialized.WalkSpeed);
                mover.StealthSpeed = Serialized.StealthSpeed;
            }
        }
    }
}
