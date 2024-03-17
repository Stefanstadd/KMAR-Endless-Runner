using UnityEngine;

public abstract class ObstacleObject : ScriptableObject
{
    [Header ("Obstacle Data")]
    public GameObject[] obstaclePrefabs;

    public LaneState obstacleLaneState;

    protected GameObject RandomPrefab
    {
        get
        {
            return obstaclePrefabs[Random.Range (0, obstaclePrefabs.Length)];
        }
    }

    public abstract ObstacleData GenerateObstacle ( WorldNode nodes );
}

public struct ObstacleData
{
    public GameObject gameObject;

    public LaneState leftLaneState;
    public LaneState middleLaneState;
    public LaneState rightLaneState;

    public ObstacleData ( GameObject gameObject, LaneState left, LaneState middle, LaneState right )
    {
        this.gameObject = gameObject;
        leftLaneState = left;
        middleLaneState = middle;
        rightLaneState = right;
    }

    public bool IsNull
    {
        get
        {
            return gameObject == null;
        }
    }

    public static ObstacleData NullObstacLe
    {
        get
        {
            return new (null, LaneState.FREE, LaneState.FREE, LaneState.FREE);
        }
    }
}