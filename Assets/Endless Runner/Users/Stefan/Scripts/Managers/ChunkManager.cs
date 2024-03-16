using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public TerrainGenerator terrain;

    private Dictionary<Vector3, GameObject> chunks = new ( );

    public Dictionary<Vector3, List<WorldNode>> nodes = new ( );
    public float renderDistance = 30f;

    public Transform player;

    public void AddNewNode ( WorldNode newNode )
    {
        TileData tile = newNode.type switch
        {
            TileType.FORWARD => newNode.biome.forwardTile,
            TileType.FORWARD_SIDEWAYS => newNode.biome.forwardSidewaysTile,
            TileType.LEFT => newNode.biome.leftTile,
            TileType.RIGHT => newNode.biome.rightTile,
            _ => newNode.biome.forwardTile,
        };

        float turnAngle = newNode.type switch
        {
            TileType.LEFT => 90,
            TileType.RIGHT => -90,
            _ => 0,
        };

        GameObject spawned = null;
        if ( tile != null )
        {
            Vector3 rotation = Vector3.up * ( WorldGenerator.AngleFromDirection (newNode.direction) + turnAngle );

            spawned = Instantiate (tile.prefabs.Random(), newNode.position, Quaternion.identity);
            spawned.transform.localEulerAngles = rotation;
        }

        float r = Random.Range (0, 100);

        if ( r < newNode.biome.generalObstacleChance && r < tile.obstacleChance )
        {
            foreach ( ObstacleObject obstacleGenerator in tile.obstacles )
            {
                float r2 = Random.Range (0, 100);

                if(r2 < obstacleGenerator.chanceToSpawn )
                {
                    foreach ( GameObject spawnedObstacle in obstacleGenerator.GenerateObstacle (newNode))
                    {
                        spawnedObstacle.transform.SetParent (spawned.transform);
                    }
                }
            }
        }

        spawned.SetActive (false);

        chunks[newNode.position] = spawned;

        Vector3 nodeChunkPos = WorldGenerator.ToTilePosition (newNode.position);

        if ( !nodes.ContainsKey (nodeChunkPos) )
        {
            nodes.Add (nodeChunkPos, new ( ));
        }

        nodes[nodeChunkPos].Add (newNode);

        terrain.AddOpenChunk (nodeChunkPos);
    }

    public void ManageChunkLoading ( )
    {
        foreach ( var item in chunks )
        {
            if ( item.Value != null )
            {
                if ( Vector3.Distance (item.Key, player.position) < renderDistance )
                {
                    item.Value.SetActive (true);
                }
                else
                {
                    item.Value.SetActive (false);
                }
            }
        }
    }

    public List<Vector3> NeighboursOf ( Vector3 targetChunk )
    {
        List<Vector3> result = new ( );

        const int neighbourCheckAmount = 5;

        for ( int x = -neighbourCheckAmount; x < neighbourCheckAmount; x++ )
        {
            for ( int z = -neighbourCheckAmount; z < neighbourCheckAmount; z++ )
            {
                Vector3 key = targetChunk - new Vector3 (WorldGenerator.TILE_DIMENSION * x, 0, WorldGenerator.TILE_DIMENSION * z);

                if ( nodes.ContainsKey (key) )
                {
                    result.Add (key);
                }
            }
        }

        return result;
    }
}