namespace BagOfTricks.Storage
{
    [System.Serializable]
    internal static class Serialized
    {
        public static bool BlockTelemetry = false;
        public static bool GodModeEnabled = false;
        public static bool InvisibilityEnabled = false;
        
        public static float RunSpeed = 8f;
        public static float DefaultRunSpeed = 8f;

        public static float WalkSpeed = 4f;
        public static float DefaultWalkSpeed = 4f;

        public static float StealthSpeed = 2.5f;
        public static float DefaultStealthSpeed = 2.5f;

        public static readonly float MinMovementSpeed = 1f;
        public static readonly float MaxMovementSpeed = 25f;
    }
}
