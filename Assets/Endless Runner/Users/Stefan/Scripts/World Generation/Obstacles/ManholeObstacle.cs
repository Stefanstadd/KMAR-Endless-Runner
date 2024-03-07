using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Manhole Obstacle",menuName ="Runner/Obstacles/Manhole")]
public class ManholeObstacle : ObstacleObject
{
    public float defectiveChance = 50;
    public float streetSize = 2.25f;

    public float yLevel;
    public override IEnumerable<GameObject> GenerateObstacle ( WorldNode node)
    {
        Vector3 pos = new (Random.Range (-1f, 1f) * streetSize, yLevel, Random.Range (-1f, 1f) * streetSize);
        Manhole manhole = Instantiate (RandomPrefab, pos + node.position, Quaternion.identity).GetComponent<Manhole>();

        ( (IObstacle) manhole ).Initialize ( );

        manhole.isDefect = Random.Range (0, 100) < defectiveChance;

        yield return manhole.gameObject;
    }
}
