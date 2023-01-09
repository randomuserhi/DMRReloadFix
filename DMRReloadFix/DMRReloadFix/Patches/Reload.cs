using UnityEngine;
using HarmonyLib;

using API;

// TODO:: Add config value for delay allowed between client hit marker and server

namespace DMRReloadFix.Patches
{
    /*
     * Tracks HP seperately rather than using client side __instance.Health since I'm unsure if messing with that will change client behaviour.
     */

    [HarmonyPatch]
    internal static class Reload
    {

    }
}