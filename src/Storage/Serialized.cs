using UnityEngine;

namespace BagOfTricks.Storage
{
    [System.Serializable]
    internal class Serialized
    {
        public static Serialized Instance = new Serialized();

        public bool BlockTelemetry = false;
        public bool GodModeEnabled = false;
        public bool InvisibilityEnabled = false;

        public float RunSpeed = 8f;
        public float WalkSpeed = 4f;
        public float StealthSpeed = 2.5f;
    }
}
