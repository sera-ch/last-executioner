using HarmonyLib;
using HutongGames.PlayMaker;
using LastExecutioner.Common;
using UnityEngine;

namespace LastExecutioner.Patches;

public class JudgePatches
{
    [HarmonyPatch(typeof(PlayMakerFSM), "Awake")]
    [HarmonyPostfix]
    private static void Postfix(PlayMakerFSM __instance)
    {
        var owner = __instance?.gameObject; if (owner == null) return;
        if (owner.name != CommonConstants.BOSS_NAME) return;
        if (__instance == null) return;
        if (__instance.FsmName == null) return;
        if (!string.Equals(__instance.FsmName, "Control", StringComparison.OrdinalIgnoreCase)) return;
        owner.GetComponent<HealthManager>().hp = CommonConstants.MAX_HP;
        Plugin.Log.LogInfo("Max HP set to " + CommonConstants.MAX_HP);
        PatchValues(__instance.Fsm, owner);
    }

    private static void PatchValues(Fsm fsm, GameObject owner)
    {
        // TODO implementation
    }
}