using LastExecutioner.Behavior;
using LastExecutioner.Common;
using LastExecutioner.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LastExecutioner.Manager;

/**
 * Manage the boss scene
 */
public class MatchManager : MonoBehaviour
{
    public static MatchManager Instance { get; private set; }
    public static bool IsRematchActive;
    private GameObject bossSceneObject;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        if (newScene.name == "Menu_Title")
        {
            Plugin.Log.LogInfo("[KR] Player returned to the menu, resetting data");

            if (IsRematchActive)
            {
                if (GameManager._instance != null && GameManager._instance.playerData != null)
                {
                    GameManager._instance.playerData.defeatedLastJudge = true;
                    GameManager._instance.playerData.blackThreadWorld = true;
                }
                IsRematchActive = false;
            }
        }

        if (newScene.name == CommonConstants.SCENE_NAME)
        {
            if (GameManager._instance != null && GameManager._instance.playerData != null && !IsRematchActive)
            {
                InitializeSessionData();
                EnableRematch();
            }
        }
    }
        
    public static void InitializeSessionData()
    {
        if (GameManager._instance == null || GameManager._instance.playerData == null) return;
        var pd = GameManager._instance.playerData;
        if (pd.defeatedLastJudge && pd.blackThreadWorld)
        {
            Plugin.Log.LogInfo("[LE] Verified: Player is in act 3. Activating Enhanced Rematch!");
            IsRematchActive = true;
            pd.defeatedLastJudge = false;
            pd.blackThreadWorld = false;
        }
        else
        {
            Plugin.Log.LogInfo("[LE] Player is NOT in act 3. Mod remains dormant.");
            IsRematchActive = false;
        }
    }

    private void EnableRematch()
    {
        Plugin.Log.LogInfo("[LE] Rematch enabled");
        this.bossSceneObject = GameObject.Find("Boss Scene");
        if (this.bossSceneObject == null) return;
        this.bossSceneObject.AddComponent<JudgeBehavior>();
        JudgeBehavior.BossObject = GameObject.Find("Last Judge");
    }
}