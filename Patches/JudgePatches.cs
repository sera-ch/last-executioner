using System.Reflection;
using GenericVariableExtension;
using HarmonyLib;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using LastExecutioner.Behavior;
using LastExecutioner.Common;
using LastExecutioner.Manager;
using LastExecutioner.Pools;
using UnityEngine;
using Object = System.Object;

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
                wait.time.Value = 0.25f;
                Plugin.Log.LogInfo("Throw Rise wait time set to 0.25f");
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
                wait.time.Value = 0.25f;
                Plugin.Log.LogInfo("Flame Spin Antic wait time set to 0.25f");
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
                wait.time.Value = 1f;
                Plugin.Log.LogInfo("Charge wait time set to 1f");
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
                wait.time.Value = 0.25f;
                Plugin.Log.LogInfo("Charge Antic 2 wait time set to 0.25f");
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
                wait.time.Value = 0.25f;
                Plugin.Log.LogInfo("Stomp Flame Antic wait time set to 0.25f");
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
                wait.time.Value = 0.25f;
                Plugin.Log.LogInfo("Stomp Flames wait time set to 0.25f");
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
                spawnObject.frequency.Value = 0.15f;
                Plugin.Log.LogInfo("Fire column spawning frequency set to 0.15f");
            }

            SpawnObjectFromGlobalPool spawnObject2 = state.Actions.OfType<SpawnObjectFromGlobalPool>().FirstOrDefault();
            
            var state2 = fsm.GetState("Stomp Flame Antic");
            if (state2 == null)
            {
                Plugin.Log.LogWarning("State Stomp Flame Antic couldn't be found.");
                return;
            }
            
            var actions = state2.Actions.ToList();
            for (int i = 0; i < 6; i++)
            {
                SpawnObjectFromGlobalPool clone = CloneAction(spawnObject2);
                clone.gameObject.Value.transform.parent = null;
                actions.Add(clone);
            }

            state2.Actions = actions.ToArray();
            Plugin.Log.LogInfo("Appended SpawnObjectFromGlobalPool to Stomp Flame Antic state");
        }
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

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FsmState), "OnEnter")]
    private static void OnFsmStateEntered(FsmState __instance)
    {
        //Plugin.Log.LogInfo("Entering " + __instance.Name);
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
                var actions = __instance.Actions.OfType<SpawnObjectFromGlobalPool>().ToList();
                for (var i = 0; i < actions.Count; i++)
                {
                    actions[i].spawnPoint = JudgeBehavior.BossObject;
                    if (i < 3)
                    {
                        actions[i].position = Vector3.right * i * 3;
                    }
                    else
                    {
                        actions[i].position = Vector3.left * (i - 3) * 3;
                    }
                }
                return;
            }
            case "Hornet Dead":
            case "Death":
            {
                JudgeBehavior.InBattle = false;
                FlamePools.ResetPools();
                return;
            }
        }
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