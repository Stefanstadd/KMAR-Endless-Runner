using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Car Obstacle",menuName = "Runner/Obstacles/Cars")]
public class CarObstacle : ObstacleObject
{
    public float streetLength;
    public float outerSpawnPosition;
    public float baseYPos;

    public override IEnumerable<GameObject> GenerateObstacle ( WorldNode node )
    {
        Vector3 spawnPos = CalculateSpawnPos (node.direction) + node.position;

        GameObject car = Instantiate (RandomPrefab, spawnPos, Quaternion.identity);
        car.transform.localEulerAngles = Vector3.up * WorldGenerator.AngleFromDirection(node.direction);

        yield return car;
    }

    private Vector3 CalculateSpawnPos(Direction direction )
    {
        float lengthPos = Random.Range (0, streetLength) - streetLength / 2;

        float sidePos = Random.value < 0.5f ? -outerSpawnPosition : outerSpawnPosition;

        return direction switch
        {
            Direction.NORTH => new Vector3 (sidePos, baseYPos, lengthPos),
            Direction.EAST or Direction.WEST => new Vector3 (lengthPos, baseYPos, sidePos),
            _ => Vector3.zero,
        };
    }

}
