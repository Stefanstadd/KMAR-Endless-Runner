using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ObstacleObject : ScriptableObject
{
    [Header ("Obstacle Data")]
    public float chanceToSpawn;

    public Vector2 minMaxSpawnAmount;

    public GameObject[] obstaclePrefabs;

    protected GameObject RandomPrefab
    {
        get
        {
            return obstaclePrefabs[Random.Range (0, obstaclePrefabs.Length - 1)];
        }
    }
    public abstract IEnumerable<GameObject> GenerateObstacle (WorldNode nodes );
}
