using UnityEngine;

[CreateAssetMenu (fileName = "Manhole Obstacle", menuName = "Runner/Obstacles/Manhole")]
public class ManholeObstacle : ObstacleObject
{
    public float defectiveChance = 50;
    public float streetSize = 2.25f;

    public float yLevel;

    public override ObstacleData GenerateObstacle ( WorldNode node )
    {
        int lane = Random.Range (-1, 2);
        Vector3 pos = new (lane * LaneMovement.LANE_SIZE, yLevel, 0);

        Manhole manhole = Instantiate (RandomPrefab, pos + node.position, Quaternion.identity).GetComponent<Manhole> ( );

        ( (IObstacle) manhole ).Initialize ( );

        manhole.isDefect = Random.Range (0, 100) < defectiveChance;

        ObstacleData data = new ObstacleData
        {
            gameObject = manhole.gameObject,
            leftLaneState = lane == -1 ? obstacleLaneState : LaneState.FREE,
            middleLaneState = lane == 0 ? obstacleLaneState : LaneState.FREE,
            rightLaneState = lane == 1 ? obstacleLaneState : LaneState.FREE,
        };

        return data;
    }
}