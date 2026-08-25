using LastExecutioner.Behavior;
using UnityEngine;

namespace LastExecutioner.Pools;

public class FlamePools
{
    public static List<GameObject> explosionPool { get; set; }
    
    public static List<GameObject> fireColumnPool { get; set; }
    
    public static List<GameObject> flameWavePool { get; set; }

    public static List<GameObject> stompWavePool { get; set; }
        
    public static void ResetPools()
    {
        explosionPool = new List<GameObject>();
        fireColumnPool = new List<GameObject>();
        flameWavePool = new List<GameObject>();
        stompWavePool = new List<GameObject>();
    }
}