using HarmonyLib;
using LastExecutioner.Manager;

namespace LastExecutioner.Patches;

/**
 * Patch save/load functions
 */
public class DataPatches
{
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.SaveGame), new Type[]{ typeof(Action<bool>) })]
    [HarmonyPrefix]
    public static void PrefixSaveGame(GameManager __instance)
    {
        if (MatchManager.IsRematchActive && __instance.playerData != null)
        {
            ResetData(__instance);
        }
    }

    private static void ResetData(GameManager __instance)
    {
        Plugin.Log.LogInfo("[LE] Save/Quit detected. Intercepting file write to force TRUE on disk.");
        __instance.playerData.defeatedLastJudge = true;
        __instance.playerData.blackThreadWorld = true;
    }
}