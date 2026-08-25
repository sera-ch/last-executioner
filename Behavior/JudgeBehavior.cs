using System.Collections;
using HutongGames.PlayMaker;
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
            Phase = 2; // Start from phase 2 (ignited)
            PlayMakerFSM[] fsms = GetComponentsInChildren<PlayMakerFSM>(true);
            DebugUtil.LogAllFsmVariables(fsms);
        }

        private static IEnumerator DisableClone(GameObject clone)
        {
            yield return new WaitForSeconds(3f);
            clone.SetActive(false);
        }

        public static IEnumerator ForceNextState(FsmState state, string eventName, float delay)
        {
            yield return new WaitForSeconds(delay);
            FsmTransition transition = state.Transitions.FirstOrDefault<FsmTransition>((Func<FsmTransition, bool>) (t => t.EventName == eventName));
            if (transition != null)
                state.Fsm.SetState(transition.ToState);
        }
    }
}