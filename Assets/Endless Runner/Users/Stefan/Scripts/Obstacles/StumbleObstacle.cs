using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StumbleObstacle : MonoBehaviour
{
    private void OnCollisionEnter ( Collision collision )
    {
        PlayerCollision col = collision.transform.GetComponent<PlayerCollision> ( );

        if ( col )
        {
            col.StumbleObstacle ( );
        }
    }
}
