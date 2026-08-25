using HarmonyLib;

namespace LastExecutioner.Patches;

/**
 * Manager for all the patches
 */
public class MainPatches
{
    public static void PatchAll()
    {
        {
            try
            {
                Harmony.CreateAndPatchAll(typeof(DataPatches), null);
                Harmony.CreateAndPatchAll(typeof(LanguagePatch), null);
                Harmony.CreateAndPatchAll(typeof(JudgePatches), null);
                #if DEBUG
                Harmony.CreateAndPatchAll(typeof(DebugPatches), null);
                #endif
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError("Harmony Patching Error: " + ex);
            }
        }
    }
}