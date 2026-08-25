using System.Collections;
using HutongGames.PlayMaker;
using LastExecutioner.Common;
using LastExecutioner.Pools;
using LastExecutioner.Util;
using UnityEngine;

namespace LastExecutioner.Behavior
{
    public class JudgeBehavior : MonoBehaviour
    {
        private PlayMakerFSM bossSceneFsm;
        private Fsm fsm;
        public static JudgeBehavior Instance { get; set; }
        public static int Phase { get; set; }
        public static GameObject BossObject { get; set; }
        public static bool InBattle { get; set; }

        private void Awake()
        {
            Instance = this;
            Phase = 1;
            PlayMakerFSM[] fsms = GetComponentsInChildren<PlayMakerFSM>(true);
            //DebugUtil.LogAllFsmVariables(fsms);
        }

        private static IEnumerator DisableClone(GameObject clone, float delay)
        {
            yield return new WaitForSeconds(delay);
            clone.SetActive(false);
        }

        public static IEnumerator ForceNextState(FsmState state, string eventName, float delay)
        {
            yield return new WaitForSeconds(delay);
            FsmTransition transition = state.Transitions.FirstOrDefault<FsmTransition>((Func<FsmTransition, bool>) (t => t.EventName == eventName));
            if (transition != null)
                state.Fsm.SetState(transition.ToState);
        }

        public static IEnumerator ForceState(FsmState state, string newState, float delay)
        {
            yield return new WaitForSeconds(delay);
            state.Fsm.SetState(newState);
        }

        public static IEnumerator SpawnExplosions(GameObject go, int distance, float angle, float delay)
        {
            Vector3 newPosition = new Vector3(
                go.transform.position.x + distance * (float)Math.Cos(angle * Math.PI / 180),
                go.transform.position.y + distance * (float)Math.Sin(angle * Math.PI / 180),
                go.transform.position.z);
            yield return new WaitForSeconds(delay);
            GameObject clone = GetNewExplosion(go);
            clone.transform.position = newPosition;
            clone.transform.rotation = go.transform.rotation;
            clone.transform.localScale = go.transform.localScale;
            clone.SetActive(true);
            Instance.StartCoroutine(DisableClone(clone, 3f));
        }

        private static GameObject GetNewExplosion(GameObject go)
        {
            List<GameObject> relevantPool = FlamePools.explosionPool;
            GameObject newExplosion = null;
            bool flag = false;
            for (int index = 0; index < relevantPool.Count; ++index)
            {
                if (!relevantPool[index].activeSelf)
                {
                    newExplosion = relevantPool[index];
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                newExplosion = Instantiate(go, null);
                newExplosion.name += "_POOLED";
                relevantPool.Add(newExplosion);
            }
            return newExplosion;
        }

    }
}