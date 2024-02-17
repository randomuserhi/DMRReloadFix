using UnityEngine;
using HarmonyLib;

using API;
using Player;

namespace DMRReloadFix.Patches
{
    [HarmonyPatch]
    internal static class Reload
    {
        // TODO:: Fix the issue of this being a static bool prefix making it difficult for other plugins to patch this function

        // Exact same reload implementation from game just rounding up instead of rounding down.
        [HarmonyPatch(typeof(PlayerAmmoStorage), nameof(PlayerAmmoStorage.GetClipBulletsFromPack))]
        [HarmonyPrefix]
        public static bool GetClipBulletsFromPack(PlayerAmmoStorage __instance, ref int __result, int currentClip, AmmoType ammoType)
        {
            InventorySlotAmmo inventorySlotAmmo = __instance.m_ammoStorage[(int)ammoType];
            float costOfBullet = inventorySlotAmmo.CostOfBullet;
            int num = inventorySlotAmmo.BulletClipSize - currentClip;
            if (num < 1)
            {
                __result = currentClip;
                return false;
            }
            float a = (float)num * inventorySlotAmmo.CostOfBullet;
            float ammoInPack = inventorySlotAmmo.AmmoInPack;
            int num2 = Mathf.RoundToInt(Mathf.Min(a, ammoInPack) / costOfBullet); // Changed from FloorToInt
            int num3 = inventorySlotAmmo.BulletsInPack;
            inventorySlotAmmo.AmmoInPack -= (float)num2 * costOfBullet;
            inventorySlotAmmo.OnBulletsUpdateCallback?.Invoke(inventorySlotAmmo.BulletsInPack);
            currentClip += Mathf.Min(num2, num3);
            __instance.NeedsSync = true;
            __instance.UpdateSlotAmmoUI(inventorySlotAmmo, currentClip);
            if (ConfigManager.Debug)
            {
                APILogger.Debug(Module.Name, $"AmmoStorage.GetClipFromPack cost: {costOfBullet} from storage: {ammoInPack} ret : {num2} currentClip: {currentClip} inPack: {__instance.m_ammoStorage[(int)ammoType].AmmoInPack}");
            }
            __result =  currentClip;
            return false;
        }
    }
}