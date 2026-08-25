using UnityEngine;

namespace LastExecutioner.Pools;

public class ObjectPools
{
    public static List<GameObject> explosionPool { get; set; }
    
    public static List<GameObject> fireColumnPool { get; set; }
    
    public static List<GameObject> fireWavePool { get; set; }

    public static void ResetPools()
    {
        explosionPool = new List<GameObject>();
        fireColumnPool = new List<GameObject>();
        fireWavePool = new List<GameObject>();
    }
}