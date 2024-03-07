using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Tree Obstacle", menuName = "Runner/Obstacles/Tree Obstacles")]
public class TreeObstacle : ObstacleObject
{
    public override IEnumerable<GameObject> GenerateObstacle (WorldNode node)
    {
        for ( int i = 0; i < Random.Range (minMaxSpawnAmount.x, minMaxSpawnAmount.y); i++ )
        {
            Vector3 checkPos = new Vector3(Random.Range(-1f,1f) * WorldGenerator.TILE_DIMENSION, 10, Random.Range (-1f, 1f) * WorldGenerator.TILE_DIMENSION) + node.position;

            if ( Physics.Raycast (checkPos, Vector3.down, out RaycastHit hit) )
            {
                yield return Instantiate (RandomPrefab, hit.point, Random.rotation);

            }
        }
    }
}
