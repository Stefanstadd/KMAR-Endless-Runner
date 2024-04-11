using UnityEngine;

[CreateAssetMenu (fileName = "Car Obstacle", menuName = "Runner/Obstacles/Cars")]
public class CarObstacle : ObstacleObject
{
    public float streetLength;
    public float spawnOffset;
    public float baseYPos;

    public float scale = 0.82322f;

    public override ObstacleData GenerateObstacle ( WorldNode node )
    {
        int lane = Random.Range (-1, 1);
        Vector3 spawnPos = new Vector3 (lane < 0 ? -LaneMovement.LANE_SIZE - spawnOffset : LaneMovement.LANE_SIZE + spawnOffset, baseYPos, 0) + node.position;

        GameObject car = Instantiate (RandomPrefab, spawnPos, Quaternion.identity);
        car.transform.localEulerAngles = Vector3.up * WorldGenerator.AngleFromDirection (node.direction);
        car.transform.localScale = Vector3.one * scale;

        ObstacleData data = new ObstacleData
        {
            gameObject = car,
            leftLaneState = lane == -1 ? obstacleLaneState : LaneState.FREE,
            middleLaneState = LaneState.FREE,
            rightLaneState = lane == 0 ? obstacleLaneState : LaneState.FREE,
        };

        return data;
    }
}