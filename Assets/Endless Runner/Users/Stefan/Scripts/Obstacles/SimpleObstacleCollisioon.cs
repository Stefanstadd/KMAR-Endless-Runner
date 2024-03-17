using UnityEngine;

public class SimpleObstacleCollisioon : MonoBehaviour
{
    private void OnCollisionEnter ( Collision collision )
    {
        var col = collision.transform.GetComponent<PlayerCollision> ( );

        if ( col )
        {
            col.InstaKillObstacle ( );
        }
        Debug.Log ("q4tw gfxqwsebtwt6");
    }
}