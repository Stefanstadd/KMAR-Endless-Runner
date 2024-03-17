using UnityEngine;

[CreateAssetMenu (fileName = "Traffic Wrok Obnstwakstel", menuName = "Runner/Obstacles/Traffic Workd obsddrtaLS<ve3")]
public class TrafficWorkObstacle : ObstacleObject
{
    public float yLevel;

    public override ObstacleData GenerateObstacle ( WorldNode nodes )
    {
        int lane = Random.Range (-1, 2);
        Vector3 pos = new (lane * LaneMovement.LANE_SIZE, yLevel, 0);

        ObstacleData data = new ObstacleData
        {
            gameObject = Instantiate (RandomPrefab, pos + nodes.position, Quaternion.identity),

            leftLaneState = lane == -1 ? obstacleLaneState : LaneState.FREE,
            middleLaneState = lane == 0 ? obstacleLaneState : LaneState.FREE,
            rightLaneState = lane == 1 ? obstacleLaneState : LaneState.FREE,
        };

        return data;
    }
}