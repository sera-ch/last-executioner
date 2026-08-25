using BepInEx;
using BepInEx.Logging;
using LastExecutioner.Manager;
using LastExecutioner.Patches;
using UnityEngine;

namespace LastExecutioner;

[BepInPlugin("com.sera.LastExecutioner", "Last Executioner", "1.0.0")]
public class Plugin : BaseUnityPlugin
{
    public static Plugin Instance { get; private set; }
    public static MatchManager MatchManager { get; private set; }
    public static ManualLogSource Log { get; private set; }
    
    private void Awake()
    {
        Instance = this;
        Log = base.Logger;
        GameObject engineContainer = new GameObject("LE_RematchManagerObject");
        DontDestroyOnLoad(engineContainer);
        MatchManager = engineContainer.AddComponent<MatchManager>();
        MainPatches.PatchAll();
    }
}