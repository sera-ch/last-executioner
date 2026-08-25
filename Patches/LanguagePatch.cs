using HarmonyLib;
using LastExecutioner.Common;
using LastExecutioner.Manager;
using LastExecutioner.Common;
using LastExecutioner.Manager;
using TeamCherry.Localization;

namespace LastExecutioner.Patches;

/**
 * Patch the name cards
 */
public static class LanguagePatch
{
        
    [HarmonyPatch(typeof(Language), "Get")]
    [HarmonyPatch(new[] { typeof(string), typeof(string) })]
    public static bool Prefix(ref string key, ref string sheetTitle, ref string __result)
    {
        if (MatchManager.Instance != null && MatchManager.IsRematchActive)
        {
            if (key == CommonConstants.BOSS_NAME_SUPER)
            {
                __result = GetLocalizedSmallSupertitle();
                return false;
            }
            if (key == CommonConstants.BOSS_NAME_MAIN)
            {
                __result = GetLocalizedMainTitle();
                return false;
            }
            if (key == CommonConstants.BOSS_NAME_SUB)
            {
                __result = GetLocalizedSmallSubtitle();
                return false;
            }
        }
        return true;
    }

    private static string GetLocalizedSmallSupertitle()
    {
        return Language.CurrentLanguage() switch
        {
            LanguageCode.FR => "Conquérant du Corail",
            LanguageCode.DE => "Koralleneroberer",
            LanguageCode.ES => "Conquistador de Corales",
            LanguageCode.ZH => "珊瑚征服者",
            _ => "Coral Conqueror"
        };
    }
        
    private static string GetLocalizedMainTitle()
    {
        return "Khann";
    }
        
    private static string GetLocalizedSmallSubtitle()
    {
        return Language.CurrentLanguage() switch
        {
            LanguageCode.FR => "",
            LanguageCode.DE => "",
            LanguageCode.ES => "",
            LanguageCode.ZH => "",
            _ => ""
        };
    }
}