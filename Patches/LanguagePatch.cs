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
            LanguageCode.FR => "Dernière",
            LanguageCode.DE => "Letzter",
            LanguageCode.ES => "Última",
            LanguageCode.ZH => "最后的",
            _ => "Last"
        };
    }
        
    private static string GetLocalizedMainTitle()
    {
        return Language.CurrentLanguage() switch
        {
            LanguageCode.FR => "Bourreau",
            LanguageCode.DE => "Henkerin",
            LanguageCode.ES => "Verdugo",
            LanguageCode.ZH => "刽子手",
            _ => "Executioner"
        };
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