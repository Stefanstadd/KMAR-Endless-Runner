using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObstacleCollisioon : MonoBehaviour
{
    private void OnCollisionEnter ( Collision collision )
    {
        var col = collision.transform.GetComponent<PlayerCollision> ();

        if ( col )
        {
            col.InstaKillObstacle ( );
        }
    }
}
