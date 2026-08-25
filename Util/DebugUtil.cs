using HutongGames.PlayMaker;

namespace LastExecutioner.Util;

public class DebugUtil
{
        public static void LogAllFsmVariables(PlayMakerFSM[]? fsms)
        {
            #if !DEBUG
                return;
            #endif

            if (fsms == null || fsms.Length == 0)
            {
                Plugin.Log.LogWarning("[KR] No PlayMakerFSM components found on Last Judge GameObject!");
                return;
            }

            Plugin.Log.LogInfo($"[KR] Found {fsms.Length} FSMs on Last Judge. Analyzing...");

            foreach (PlayMakerFSM fsm in fsms)
            {
                Plugin.Log.LogInfo($"==================================================");
                Plugin.Log.LogInfo($"FSM NAME: {fsm.FsmName} | On GameObject: {fsm.gameObject.name}");
                Plugin.Log.LogInfo($"==================================================");
                FsmVariables variables = fsm.FsmVariables;

                if (variables.FloatVariables != null && variables.FloatVariables.Length > 0)
                {
                    Plugin.Log.LogInfo("--- FLOAT VARIABLES ---");
                    foreach (FsmFloat fsmFloat in variables.FloatVariables)
                    {
                        Plugin.Log.LogInfo($"  [Float] {fsmFloat.Name} = {fsmFloat.Value}");
                    }
                }

                if (variables.IntVariables != null && variables.IntVariables.Length > 0)
                {
                    Plugin.Log.LogInfo("--- INT VARIABLES ---");
                    foreach (FsmInt fsmInt in variables.IntVariables)
                    {
                        Plugin.Log.LogInfo($"  [Int] {fsmInt.Name} = {fsmInt.Value}");
                    }
                }

                if (variables.BoolVariables != null && variables.BoolVariables.Length > 0)
                {
                    Plugin.Log.LogInfo("--- BOOL VARIABLES ---");
                    foreach (FsmBool fsmBool in variables.BoolVariables)
                    {
                        Plugin.Log.LogInfo($"  [Bool] {fsmBool.Name} = {fsmBool.Value}");
                    }
                }

                if (variables.Vector2Variables != null && variables.Vector2Variables.Length > 0)
                {
                    Plugin.Log.LogInfo("--- VECTOR2 VARIABLES ---");
                    foreach (FsmVector2 fsmVec2 in variables.Vector2Variables)
                    {
                        Plugin.Log.LogInfo($"  [Vector2] {fsmVec2.Name} = {fsmVec2.Value}");
                    }
                }

                Plugin.Log.LogInfo("--- STATE ACTION BRIEFING ---");
                foreach (FsmState state in fsm.Fsm.States)
                {
                    Plugin.Log.LogInfo($"  State [{state.Name}] has {state.Actions.Length} actions.");
                    foreach (FsmStateAction action in state.Actions)
                    {
                        Plugin.Log.LogInfo($"    -> Action Type: {action.GetType().Name}");
                    }
                }
            }
            Plugin.Log.LogInfo($"==================================================");
        }
}