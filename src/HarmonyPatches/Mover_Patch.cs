using BagOfTricks.Storage;
using HarmonyLib;

namespace BagOfTricks.Patches
{
    [HarmonyPatch(typeof(Mover), "Update", MethodType.Normal)]
    public class Mover_Patch
    {
        [HarmonyPrefix]
        static void UpdatePrefix(Mover __instance)
        {
            string moverName = __instance.gameObject.name;
            if (!moverName.StartsWith("Player") && !moverName.StartsWith("Comp"))
            {
                return;
            }

            __instance.SetRunSpeed(Serialized.RunSpeed);
            __instance.SetWalkSpeed(Serialized.WalkSpeed);
            __instance.StealthSpeed = Serialized.StealthSpeed;
        }
    }
}