using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class WorldGenerator : MonoBehaviour
{
    public const int TILE_DIMENSION = 5;

    [Header("World data")]

    public Biome[] biomes;

    public bool randomSeed;
    public int seed;

    [Header ("Generation Settings")]

    [Tooltip("The object that can load certain parts of the map")]
    public Transform loader;

    [Tooltip("Can make the path turn left or right")]
    public bool useTurns = true;

    public float rotationAdaptTreshold = 45;

    [Tooltip("The min and max amount of times a tile can be generated forward before the path will go to the left or right")]
    public Vector2Int tileDirectionChangeAmount;

    [Tooltip ("The min and max amount of times a tile can be generated of a specific biome before the biome changes")]
    public Vector2Int biomeChangeAmount;

    [Tooltip("The minimun distance for the tail of the world to generate in front of the player")]
    public int loadDistance = 20;

    public Vector3 offset;

    [Header("Gizmos")]
    public bool drawGizmos;
    public float gizmoSize;

    //Private variables
    System.Random random;
    Vector3 lastPlayerTilePos;
    Vector3 positionsSum;

    int currentBiomeIndex;

    WorldNode head;
    WorldNode tail;

    Vector3 minBounds, maxBounds;

    //Generation values
    int tileCount;
    int tilesForBiomeSwitch;
    int currentBiomeCounter;

    int tilesForDirectionSwitch;
    int currentDirectionCounter;
    Direction currentDirection;

    public Biome CurrentBiome
    {
        get
        {
            return biomes[currentBiomeIndex];
        }
    }


    private void Start ( )
    {
        Initialize ( );
    }

    void Initialize ( )
    {
        if ( randomSeed )
            seed = Random.Range (0, 1000000);

        currentDirection = Direction.NORTH;

        random = new (seed);

        positionsSum = offset;

        tilesForBiomeSwitch = random.Next (biomeChangeAmount.x, biomeChangeAmount.y );

        tilesForDirectionSwitch = random.Next (tileDirectionChangeAmount.x, tileDirectionChangeAmount.y);

        head = CreateNode (offset, CurrentBiome, currentDirection);

        tileCount = 1;

        tail = head;
    }

    private void FixedUpdate ( )
    {
        if ( loader == null ) return;

        var playerTilePos = ToTilePosition (loader.position);

        if(playerTilePos != lastPlayerTilePos ) // The player has moved, might load a new chunk
        {
            float distance = Vector3.Distance (tail.position, loader.position);

            lastPlayerTilePos = playerTilePos;

            if(distance <= loadDistance )
            {
                ContinueGeneration ( );
            }

        }
    }

    public void ContinueGeneration ( )
    {
        do
        {
            tail.next = GenerateNode ( );

            tail = tail.next;
        }
        while ( Vector3.Distance (tail.position, loader.position) <= loadDistance );
    }

    #region Generation
    public WorldNode GenerateNode ( )
    {
        currentDirectionCounter++;

        if ( currentDirectionCounter >= tilesForDirectionSwitch )
            ChangeDirection (tail.position );

        currentBiomeCounter++;
        if ( currentBiomeCounter >= tilesForBiomeSwitch )
            ChangeBiome ( );


        Vector3 position = GenerateNodePosition ( );
        tileCount++;

        return CreateNode (position,CurrentBiome,currentDirection );
    }

    Vector3 GenerateNodePosition ( )
    {
        return tail.position + VectorFromDirection (currentDirection) * TILE_DIMENSION;
    }

    void ChangeBiome ( )
    {
        currentBiomeCounter = 0;
        tilesForBiomeSwitch = random.Next (biomeChangeAmount.x, biomeChangeAmount.y);

        currentBiomeIndex++;

        if(currentBiomeIndex > biomes.Length - 1 )
        {
            currentBiomeIndex = 0;
        }

    }

    void ChangeDirection (Vector3 currentPos )
    {
        currentDirectionCounter = 0;
        tilesForDirectionSwitch = random.Next (tileDirectionChangeAmount.x, tileDirectionChangeAmount.y);

        Vector3 mid = positionsSum / tileCount;

        float angle = Vector3.Angle ( mid, currentPos );

        currentDirection = GetNewDirection (currentDirection);

        //if(Mathf.Abs(angle - 180) <= rotationAdaptTreshold)
        //{
        //    currentDirection = DirectionFromAngle (360 - angle);
        //}
        //else
        //{
        //    currentDirection = GetNewDirection (currentDirection);
        //}

    }

    Direction GetNewDirection (Direction currentDirection)
    {
        return currentDirection switch
        {
            Direction.NORTH => NextDirection (Direction.WEST, Direction.EAST),
            Direction.EAST => Direction.NORTH,
            //Direction.SOUTH => NextDirection (Direction.WEST, Direction.EAST),
            Direction.WEST => Direction.NORTH,
            _ => NextDirection (Direction.WEST, Direction.EAST),

        };

        Direction NextDirection(Direction a, Direction b )
        {
            return random.Next (0, 100) <= 50 ? a : b;
        }
    }

    #endregion

    public WorldNode CreateNode (Vector3 position, Biome biome, Direction direction )
    {
        return new WorldNode
        {
            position = position,
            biome = biome,
            direction = direction,
        };
    }


    #region Gizmos

    private void OnDrawGizmos ( )
    {
        if ( !drawGizmos )
            return;

        Sample (head);

        void Sample ( WorldNode node )
        {
            if ( node == null )
                return;

            if ( node.next != null)
            {
                Gizmos.color = node.biome.biomeColor;
                Gizmos.DrawLine (node.position, node.next.position);
                Gizmos.DrawSphere (node.position, gizmoSize);
                Sample (node.next);
            }
        }
    }


    #endregion


    /// <summary>
    /// Converts a position in worldspace to the coordinate of a tile
    /// </summary>
    /// <param name="globalPosition"></param>
    /// <returns></returns>
    public static Vector3 ToTilePosition(Vector3 globalPosition )
    {
        float x = Mathf.RoundToInt (globalPosition.x + Mathf.Epsilon / TILE_DIMENSION) * TILE_DIMENSION;
        float y = Mathf.RoundToInt (globalPosition.y + Mathf.Epsilon / TILE_DIMENSION) * TILE_DIMENSION;
        float z = Mathf.RoundToInt (globalPosition.z + Mathf.Epsilon / TILE_DIMENSION) * TILE_DIMENSION;

        return new (x, y, z);
    }

    public static Direction DirectionFromAngle(float angle )
    {
        Direction result = Direction.NORTH;

        if(angle >= 90 )
        {
            result = Direction.EAST;
        }
        if(angle >= 180 )
        {
            result = Direction.SOUTH;
        }
        if(angle >= 270 )
        {
            result = Direction.WEST;
        }
        return result;
    }

    public static Vector3 VectorFromDirection(Direction direction )
    {
        return direction switch
        {
            Direction.NORTH => new (0, 0, 1),
            Direction.EAST => new (1, 0, 0),
            Direction.SOUTH => new (0, 0, -1),
            Direction.WEST => new (-1, 0, 0),
            _ => new(0,0,1),
        };
    }

    public static float AngleFromDirection ( Direction direction )
    {
        return direction switch
        {
            Direction.NORTH => 0,
            Direction.EAST => 90,
            Direction.SOUTH => 180,
            Direction.WEST => 270,
            _ => 0
        };
    }

}
