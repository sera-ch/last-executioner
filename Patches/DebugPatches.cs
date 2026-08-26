using GlobalEnums;
using HarmonyLib;
using UnityEngine;

namespace LastExecutioner.Patches;

public class DebugPatches
{
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.TakeDamage))]
    [HarmonyPostfix]
    public static void TakeDamagePostfix(HeroController __instance,
        GameObject go,
        CollisionSide damageSide,
        int damageAmount,
        HazardType hazardType,
        DamagePropertyFlags damagePropertyFlags)
    {
        // Invincibility for testing
        //__instance.playerData.health += damageAmount;
    }
}