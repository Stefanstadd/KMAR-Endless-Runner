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