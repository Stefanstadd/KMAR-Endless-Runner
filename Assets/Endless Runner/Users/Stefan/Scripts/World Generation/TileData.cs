using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData
{
    public GameObject tilePrefab;

    public float obstacleChance;

    public ObstacleObject[] obstacles;
}
