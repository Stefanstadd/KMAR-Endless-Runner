using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [Header ("References")]
    public WorldGenerator worldGenerator;

    public ChunkManager chunkManager;

    [Header ("Chunk Data")]
    public float generateDistance;

    public float renderDistance = 40;

    public int planeSize = 30;
    public int vertexDensity = 16;

    public GameObject chunkPrefab;
    public GameObject tree;
    public int treesPerChunk = 3;
    public float minDstToRoad = 20;
    public Transform loader;

    public int neighbourCheckAmount = 1;

    [Header ("Terrain Settings")]
    public float noiseScale;

    public float noiseMultiplier;
    public float pathDstDampen;
    public float pathDampMultiplier;

    public Vector3 offset;

    private List<Vector3> openChunks = new ( );
    private List<Vector3> toRemove = new ( );

    private List<WorldNode> nodesToCheck;

    private Dictionary<Vector3, Transform> activePlanes = new ( );

    [Header ("Gizmos")]
    public bool drawGizmos;

    public Color gizmoColor;
    public float gizmoSize;

    public void AddOpenChunk ( Vector3 chunk )
    {
        openChunks.Add (chunk);
    }

    public void OnPlayerMove ( )
    {
        CheckForGenerateChunks ( );

        CheckForRenderChunks ( );
    }

    private void CheckForGenerateChunks ( )
    {
        foreach ( var chunk in openChunks )
        {
            float dst = Vector3.Distance (chunk, loader.position);

            nodesToCheck = chunkManager.NeighboursOf (chunk).SelectMany (c => chunkManager.nodes[c]).ToList ( );

            if ( dst < generateDistance )
            {
                for ( int x = -neighbourCheckAmount; x <= neighbourCheckAmount; x++ )
                {
                    for ( int z = -neighbourCheckAmount; z <= neighbourCheckAmount; z++ )
                    {
                        Vector3 chunkPos = ToPlanePosition (chunk) + new Vector3 (x * planeSize, 0, z * planeSize);

                        if ( !activePlanes.ContainsKey (chunkPos) )
                        {
                            GenerateChunk (chunkPos);
                        }
                    }
                }

                toRemove.Add (chunk);
            }
        }

        foreach ( var remChunk in toRemove )
        {
            openChunks.Remove (remChunk);
        }
        toRemove.Clear ( );
    }

    private void CheckForRenderChunks ( )
    {
        List<Vector3> keysToRemove = new ( );

        foreach ( var chunk in activePlanes )
        {
            float dst = Vector3.Distance (chunk.Key, loader.position);

            if ( dst < renderDistance && !chunk.Value.gameObject.activeSelf )
            {
                chunk.Value.gameObject.SetActive (true);
            }
            else if ( dst > renderDistance && chunk.Value.gameObject.activeSelf )
            {
                keysToRemove.Add (chunk.Key);
            }
        }

        foreach ( var toRemove in keysToRemove )
        {
            Destroy (activePlanes[toRemove].gameObject);

            activePlanes.Remove (toRemove);
        }
    }

    private void GenerateChunk ( Vector3 chunkPosition )
    {
        Vector3 planeCoord = ToPlanePosition (chunkPosition);

        List<Vector3> vertices = new ( );

        int index = 0;

        Vector3 offset = new (planeSize / 2, 0, planeSize / 2);
        for ( int x = 0; x <= vertexDensity; x++ )
        {
            for ( int z = 0; z <= vertexDensity; z++ )
            {
                Vector3 vertex = new Vector3 ((float) x / ( vertexDensity ) * planeSize, 0, (float) z / ( vertexDensity ) * planeSize) - offset;

                vertex.y = GenerateHeight (vertex, chunkPosition);

                vertices.Add (vertex);
                index++;
            }
        }

        int[] triangles = new int[vertexDensity * vertexDensity * 6];
        for ( int ti = 0, vi = 0, y = 0; y < vertexDensity; y++, vi++ )
        {
            for ( int x = 0; x < vertexDensity; x++, ti += 6, vi++ )
            {
                triangles[ti] = vi;
                triangles[ti + 1] = triangles[ti + 4] = vi + 1;
                triangles[ti + 2] = triangles[ti + 3] = vi + vertexDensity + 1;
                triangles[ti + 5] = vi + vertexDensity + 2;
            }
        }

        Mesh mesh = new Mesh ( );

        mesh.SetVertices (vertices);
        mesh.SetTriangles (triangles, 0);

        mesh.RecalculateNormals ( );

        GameObject obj = Instantiate (chunkPrefab, planeCoord + this.offset, Quaternion.identity);
        obj.transform.parent = transform;
        obj.transform.name = planeCoord.ToString ( );

        obj.GetComponent<MeshFilter> ( ).mesh = mesh;
        obj.GetComponent<MeshCollider> ( ).sharedMesh = mesh;

        SpawnTrees (obj);

        activePlanes.Add (planeCoord, obj.transform);
    }

    private void SpawnTrees ( GameObject terrain )
    {
        for ( int i = 0; i < treesPerChunk; i++ )
        {
            Vector3 position = new Vector3
            {
                x = Random.Range (-planeSize, planeSize),
                y = 100,
                z = Random.Range (-planeSize, planeSize),
            };

            position += terrain.transform.position;

            if ( Mathf.Abs (position.x) < minDstToRoad )
                continue;

            if ( Physics.Raycast (position, Vector3.down, out RaycastHit hit) )
            {
                Transform tree = Instantiate (this.tree, hit.point, Quaternion.identity, terrain.transform).transform;

                tree.up = hit.normal;
            }
        }
    }

    public float GenerateHeight ( Vector3 vertexPosition, Vector3 chunk )
    {
        Vector3 vertex = vertexPosition + ToPlanePosition (chunk);

        float noise = Mathf.PerlinNoise (vertex.x * noiseScale, vertex.z * noiseScale) * noiseMultiplier;

        if ( nodesToCheck != null && nodesToCheck.Count > 0 )
        {
            if ( Mathf.Abs(vertex.x) < pathDstDampen )
            {
                float mult = Mathf.InverseLerp (0, pathDstDampen, Mathf.Abs (vertex.x));

                noise *= mult * pathDampMultiplier;
            }
        }

        return noise;
    }

    public Vector3 ToPlanePosition ( Vector3 globalPosition )
    {
        float x = Mathf.RoundToInt (( globalPosition.x + Mathf.Epsilon ) / planeSize) * planeSize;
        float y = Mathf.RoundToInt (( globalPosition.y + Mathf.Epsilon ) / planeSize) * planeSize;
        float z = Mathf.RoundToInt (( globalPosition.z + Mathf.Epsilon ) / planeSize) * planeSize;

        return new (x, y, z);
    }

    public bool PositionInPlane ( Vector3 planePosition, Vector3 position )
    {
        var tilePos = ToPlanePosition (position);

        float half = planeSize / 2;

        return planePosition.x - half > tilePos.x && planePosition.x + half < tilePos.x &&
               planePosition.y - half > tilePos.y && planePosition.y + half < tilePos.y &&
               planePosition.z - half > tilePos.z && planePosition.z + half < tilePos.z;
    }

    private void OnDrawGizmos ( )
    {
        if ( !drawGizmos )
            return;

        Gizmos.color = gizmoColor;
        foreach ( var kvp in activePlanes )
        {
            foreach ( var item in kvp.Value.GetComponent<MeshFilter> ( ).mesh.vertices )
            {
                Gizmos.DrawSphere (item, gizmoSize);
            }
        }
    }
}