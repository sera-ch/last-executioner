using System.Reflection;
using GenericVariableExtension;
using HarmonyLib;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using LastExecutioner.Behavior;
using LastExecutioner.Common;
using LastExecutioner.Manager;
using LastExecutioner.Pools;
using LastExecutioner.Util;
using UnityEngine;
using Object = System.Object;

namespace LastExecutioner.Patches;

public class JudgePatches
{
    private static int flameColumnDistance = 3;
    private static float flameColumnFrequency = 0.15f;
    
    [HarmonyPatch(typeof(PlayMakerFSM), "Awake")]
    [HarmonyPostfix]
    private static void Postfix(PlayMakerFSM __instance)
    {
        var owner = __instance?.gameObject; 
        if (owner == null) return;
        if (owner.name != CommonConstants.BOSS_NAME) return;
        if (__instance == null) return;
        if (__instance.FsmName == null) return;
        if (!string.Equals(__instance.FsmName, "Control", StringComparison.OrdinalIgnoreCase)) return;
        owner.GetComponent<HealthManager>().hp = CommonConstants.MAX_HP;
        Plugin.Log.LogInfo("Max HP set to " + CommonConstants.MAX_HP);
        PatchValues(__instance.Fsm, owner);
        PatchAttacks(__instance.Fsm, owner);
    }

    private static void PatchValues(Fsm fsm, GameObject owner)
    {
        if (fsm.Name != "Control") return;
        FsmState state;

        if (owner.name == CommonConstants.BOSS_NAME)
        {
            state = fsm.GetState("Flame Up Check");
            if (state == null)
            {
                Plugin.Log.LogWarning("State Flame Up Check couldn't be found.");
                return;
            }

            var compareHp = state.Actions.OfType<CompareHP>().ToArray()[0];
            compareHp.integer2 = CommonConstants.P2_HP;
            Plugin.Log.LogInfo("Phase 2 HP set to " + CommonConstants.P2_HP);

            state = fsm.GetState("Rage Check");
            if (state == null)
            {
                Plugin.Log.LogWarning("State Rage Check couldn't be found.");
                return;
            }

            compareHp = state.Actions.OfType<CompareHP>().ToArray()[0];
            compareHp.integer2 = CommonConstants.P3_HP;
            Plugin.Log.LogInfo("Phase 3 HP set to " + CommonConstants.P3_HP);

            state = fsm.GetState("Throw Rise");
            if (state == null)
            {
                Plugin.Log.LogWarning("State Throw Rise couldn't be found.");
                return;
            }

            var wait = state.Actions.OfType<Wait>().ToArray().FirstOrDefault();
            if (wait != null)
            {
                wait.time.Value = 0.1f;
                Plugin.Log.LogInfo("Throw Rise wait time set to 0.1f");
            }

            state = fsm.GetState("Flame Spin Antic");
            if (state == null)
            {
                Plugin.Log.LogWarning("State Flame Spin Antic couldn't be found.");
                return;
            }

            wait = state.Actions.OfType<Wait>().ToArray().FirstOrDefault();
            if (wait != null)
            {
                wait.time.Value = 0.1f;
                Plugin.Log.LogInfo("Flame Spin Antic wait time set to 0.1f");
            }

            state = fsm.GetState("Charge Flame");
            if (state == null)
            {
                Plugin.Log.LogWarning("State Charge Flame couldn't be found.");
                return;
            }

            wait = state.Actions.OfType<Wait>().ToArray().FirstOrDefault();
            if (wait != null)
            {
                wait.time.Value = 0.5f;
                Plugin.Log.LogInfo("Charge Flame wait time set to 0.5f");
            }

            state = fsm.GetState("Charge");
            if (state == null)
            {
                Plugin.Log.LogWarning("State Charge couldn't be found.");
                return;
            }

            wait = state.Actions.OfType<Wait>().ToArray().FirstOrDefault();
            if (wait != null)
            {
                wait.time.Value = 0.5f;
                Plugin.Log.LogInfo("Charge wait time set to 0.5f");
            }

            state = fsm.GetState("Charge Antic 2");
            if (state == null)
            {
                Plugin.Log.LogWarning("State Charge Antic 2 couldn't be found.");
                return;
            }

            wait = state.Actions.OfType<Wait>().ToArray().FirstOrDefault();
            if (wait != null)
            {
                wait.time.Value = 0.1f;
                Plugin.Log.LogInfo("Charge Antic 2 wait time set to 0.1f");
            }

            state = fsm.GetState("Stomp Flame Antic");
            if (state == null)
            {
                Plugin.Log.LogWarning("State Stomp Flame Antic couldn't be found.");
                return;
            }

            wait = state.Actions.OfType<Wait>().ToArray().FirstOrDefault();
            if (wait != null)
            {
                wait.time.Value = 0.1f;
                Plugin.Log.LogInfo("Stomp Flame Antic wait time set to 0.1f");
            }

            state = fsm.GetState("Stomp Flames");
            if (state == null)
            {
                Plugin.Log.LogWarning("State Stomp Flames couldn't be found.");
                return;
            }

            wait = state.Actions.OfType<Wait>().ToArray().FirstOrDefault();
            if (wait != null)
            {
                wait.time.Value = 0.1f;
                Plugin.Log.LogInfo("Stomp Flames wait time set to 0.1f");
            }

            state = fsm.GetState("Flame Spin Antic L");
            if (state == null)
            {
                Plugin.Log.LogWarning("State Flame Spin Antic L couldn't be found.");
                return;
            }
            wait = state.Actions.OfType<Wait>().ToArray().FirstOrDefault();
            if (wait != null)
            {
                wait.time.Value = 0.1f;
                Plugin.Log.LogInfo("Flame Spin Antic L wait time set to 0.1f");
            }

            state = fsm.GetState("Spinning");
            if (state == null)
            {
                Plugin.Log.LogWarning("State Spinning couldn't be found.");
                return;
            }
            wait = state.Actions.OfType<Wait>().ToArray().FirstOrDefault();
            if (wait != null)
            {
                wait.time.Value = 0.5f;
                Plugin.Log.LogInfo("Spinning wait time set to 0.5f");
            }

            state = fsm.GetState("Flame Dir");
            if (state == null)
            {
                Plugin.Log.LogWarning("State Flame Dir couldn't be found.");
                return;
            }
            var randomFloat = state.Actions.OfType<RandomFloat>().FirstOrDefault();
            if (randomFloat != null)
            {
                randomFloat.min = 5f;
                randomFloat.max = 15f;
                Plugin.Log.LogInfo("Flame Flame Dir range set to 5f-15f");
            }

            state = fsm.GetState("Charge");
            if (state == null)
            {
                Plugin.Log.LogWarning("State Charge couldn't be found.");
                return;
            }
            SpawnObjectFromGlobalPoolOverTime spawnObject = state.Actions.OfType<SpawnObjectFromGlobalPoolOverTime>().FirstOrDefault();
            if (spawnObject != null)
            {
                spawnObject.frequency.Value = flameColumnFrequency;
                Plugin.Log.LogInfo("Fire column spawning frequency set to 0.15f");
            }
        }
    }

    private static void PatchAttacks(Fsm fsm, GameObject owner)
    {
        if (fsm.Name != "Control" || owner.name != CommonConstants.BOSS_NAME) return;
        var stompFlameState = fsm.GetState("Stomp Flame Antic");
        if (stompFlameState == null)
        {
            Plugin.Log.LogWarning("State Stomp Flame Antic couldn't be found.");
            return;
        }
        var actions = stompFlameState.Actions.ToList();
        var chargeState = fsm.GetState("Charge");
        if (chargeState == null)
        {
            Plugin.Log.LogWarning("State Charge couldn't be found.");
            return;
        }
        SpawnObjectFromGlobalPool spawnObject = chargeState.Actions.OfType<SpawnObjectFromGlobalPool>().FirstOrDefault();
        for (int i = 1; i < 4; i++)
        {
            SpawnObjectFromGlobalPool clone = CloneAction(spawnObject);
            clone.gameObject.Value.transform.parent = null;
            clone.spawnPoint = owner;
            clone.position = Vector3.right * flameColumnDistance * i;
            actions.Add(clone);
            clone = CloneAction(spawnObject);
            clone.gameObject.Value.transform.parent = null;
            clone.spawnPoint = owner;
            clone.position = Vector3.left * flameColumnDistance * i;
            actions.Add(clone);
        }
        stompFlameState.Actions = actions.ToArray();
        Plugin.Log.LogInfo("Appended SpawnObjectFromGlobalPool to Stomp Flame Antic state");
        
        var homingFlameState = new FsmState(stompFlameState);
        var homingFlameActions = new List<FsmStateAction>();
        homingFlameState.Name = "Homing Flame Antic";
        var singState = fsm.GetState("Sing");
        if (singState == null)
        {
            Plugin.Log.LogWarning("State Sing couldn't be found.");
            return;
        }
        var animation = singState.Actions.OfType<Tk2dPlayAnimation>().FirstOrDefault();
        homingFlameActions.Add(animation);
        SpawnObjectFromGlobalPoolOverTime spawnObject2 = chargeState.Actions.OfType<SpawnObjectFromGlobalPoolOverTime>().FirstOrDefault();
        SpawnObjectFromGlobalPoolOverTime clone2 = CloneAction(spawnObject2);
        clone2.gameObject.Value.transform.parent = null;
        clone2.spawnPoint = owner;
        clone2.position = Vector3.zero;
        clone2.frequency = 0.5f;
        homingFlameActions.Add(clone2);
        homingFlameState.Actions = homingFlameActions.ToArray();
        homingFlameState.Transitions = singState.Transitions;
        List<FsmState> states = fsm.States.ToList();
        states.Add(homingFlameState);
        fsm.States = states.ToArray();
        Plugin.Log.LogInfo("Appended state Homing Flame Antic");
    }
    private static SpawnObjectFromGlobalPool CloneAction(SpawnObjectFromGlobalPool original)
    {
        SpawnObjectFromGlobalPool clone = Activator.CreateInstance(typeof(SpawnObjectFromGlobalPool)) as SpawnObjectFromGlobalPool;
        var fields = typeof(SpawnObjectFromGlobalPool).GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        foreach (var field in fields)
        {
            field.SetValue(clone, field.GetValue(original));
        }
        return clone;
    }
    private static SpawnObjectFromGlobalPoolOverTime CloneAction(SpawnObjectFromGlobalPoolOverTime original)
    {
        SpawnObjectFromGlobalPoolOverTime clone = Activator.CreateInstance(typeof(SpawnObjectFromGlobalPoolOverTime)) as SpawnObjectFromGlobalPoolOverTime;
        var fields = typeof(SpawnObjectFromGlobalPoolOverTime).GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        foreach (var field in fields)
        {
            field.SetValue(clone, field.GetValue(original));
        }
        return clone;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FsmState), "OnEnter")]
    private static void OnFsmStateEntered(FsmState __instance)
    {
        switch (__instance.Name)
        {
            case "Battle":
            {
                JudgeBehavior.InBattle = true;
                FlamePools.ResetPools();
                return;
            }
            case "Flame Roar 3":
            {
                if (JudgeBehavior.Phase == 1)
                {
                    Plugin.Log.LogInfo("Boss enters phase 2");
                    JudgeBehavior.Phase = 2;
                }
                return;
            }
            case "Rage Roar 1":
            {
                if (JudgeBehavior.Phase == 2)
                {
                    Plugin.Log.LogInfo("Boss enters phase 3");
                    JudgeBehavior.Phase = 3;
                }
                return;
            }
            case "Stomp Flame Antic":
            {
                if (JudgeBehavior.Phase == 3 && flameColumnDistance < 5)
                {
                    flameColumnDistance = 5;
                    var chargeState = __instance.Fsm.GetState("Charge");
                    if (chargeState == null)
                    {
                        Plugin.Log.LogWarning("State Charge couldn't be found.");
                        return;
                    }

                    var actions = __instance.Actions.ToList();
                    SpawnObjectFromGlobalPool spawnObject =
                        chargeState.Actions.OfType<SpawnObjectFromGlobalPool>().FirstOrDefault();
                    for (int i = 1; i < 4; i++)
                    {
                        SpawnObjectFromGlobalPool clone = CloneAction(spawnObject);
                        clone.gameObject.Value.transform.parent = null;
                        clone.spawnPoint = JudgeBehavior.BossObject;
                        clone.position = Vector3.right * flameColumnDistance * i;
                        actions.Add(clone);
                        clone = CloneAction(spawnObject);
                        clone.gameObject.Value.transform.parent = null;
                        clone.spawnPoint = JudgeBehavior.BossObject;
                        clone.position = Vector3.left * flameColumnDistance * i;
                        actions.Add(clone);
                    }
                }
                return;
            }
            case "Charge End":
            {
                JudgeBehavior.Instance.StartCoroutine(JudgeBehavior.ForceState(__instance, "Homing Flame Antic", 1f));
                return;
            }
            case "Homing Flame Antic":
            {
                SpawnObjectFromGlobalPoolOverTime spawnObjects =
                    __instance.Actions.OfType<SpawnObjectFromGlobalPoolOverTime>().FirstOrDefault();
                spawnObjects.spawnPoint = HeroController.instance.gameObject;
                JudgeBehavior.Instance.StartCoroutine(JudgeBehavior.ForceNextState(__instance, "FINISHED", 5f));
                return;
            }
            case "Charge":
            {
                if (JudgeBehavior.Phase == 3 && flameColumnFrequency > 0.1f)
                {
                    flameColumnFrequency = 0.1f;
                    SpawnObjectFromGlobalPoolOverTime spawnObject = __instance.Actions.OfType<SpawnObjectFromGlobalPoolOverTime>().FirstOrDefault();
                    if (spawnObject != null)
                    {
                        spawnObject.frequency.Value = flameColumnFrequency;
                        Plugin.Log.LogInfo("Fire column spawning frequency set to 0.1f");
                    }
                    
                }
                return;
            }
            case "Hornet Dead":
            case "Gate Open Scene":
            {
                JudgeBehavior.InBattle = false;
                MatchManager.IsRematchActive = false;
                FlamePools.ResetPools();
                ResetData(GameManager.instance);
                return;
            }
        }
    }
    
    private static void ResetData(GameManager __instance)
    {
        Plugin.Log.LogInfo("[LE] Player defeated Last Executioner or died, resetting data");
        __instance.playerData.defeatedLastJudge = true;
        __instance.playerData.blackThreadWorld = true;
    }

    [HarmonyPatch(typeof(ActivateGameObject), "OnEnter")]
    [HarmonyPostfix]
    private static void OnActivateGameObjectStateEntered(ActivateGameObject __instance)
    {
        if (!JudgeBehavior.InBattle) return;
        GameObject activatedGameObject = Traverse.Create(__instance).Field("activatedGameObject").GetValue<GameObject>();
        if (activatedGameObject == null) return;
        if (activatedGameObject.name == CommonConstants.EXPLOSION)
        {
            for (int i = 0; i < 6; i++)
            {
                JudgeBehavior.Instance.StartCoroutine(JudgeBehavior.SpawnExplosions(activatedGameObject, 6, i * 360f/6,
                    0.5f));
            }
            if (JudgeBehavior.Phase == 3) {
                for (int i = 0; i < 8; i++)
                {
                    JudgeBehavior.Instance.StartCoroutine(JudgeBehavior.SpawnExplosions(activatedGameObject, 11, i * 360f/8,
                        1f));
                }
            }
            return;
        }
    }
}