using HarmonyLib;
using Player;

namespace DMRReloadFix.Patches {
    [HarmonyPatch]
    public static class Reload {
        public static bool callOriginal = false;
        [HarmonyPatch(typeof(PlayerAmmoStorage), nameof(PlayerAmmoStorage.GetClipBulletsFromPack))]
        [HarmonyPrefix]
        public static bool Prefix_GetClipBulletsFromPack(PlayerAmmoStorage __instance, ref int __result, int currentClip, AmmoType ammoType) {
            if (!callOriginal) {
                callOriginal = true;
                __result = __instance.GetClipBulletsFromPack(__instance.GetClipBulletsFromPack(currentClip, ammoType), ammoType);
                callOriginal = false;
                return false;
            }
            return true;
        }
    }
}