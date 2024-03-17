using UnityEngine;

[CreateAssetMenu (fileName = "Tree Obstacle", menuName = "Runner/Obstacles/Tree Obstacles")]
public class TreeObstacle : ObstacleObject
{
    public override ObstacleData GenerateObstacle ( WorldNode node )
    {
        int lane = Random.Range (-1, 2);

        Vector3 checkPos = new Vector3 (lane * LaneMovement.LANE_SIZE, 10, 0) + node.position;

        if ( Physics.Raycast (checkPos, Vector3.down, out RaycastHit hit) )
        {
            return new ObstacleData
            {
                gameObject = Instantiate (RandomPrefab, hit.point, Random.rotation),
                leftLaneState = lane == -1 ? obstacleLaneState : LaneState.FREE,
                middleLaneState = lane == 0 ? obstacleLaneState : LaneState.FREE,
                rightLaneState = lane == 1 ? obstacleLaneState : LaneState.FREE,
            };
        }

        return ObstacleData.NullObstacLe;
    }
}