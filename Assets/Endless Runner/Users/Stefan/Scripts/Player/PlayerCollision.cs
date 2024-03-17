using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement movement;

    public void InstaKillObstacle ( )
    {
        movement.Die ( );
    }

    public void StumbleObstacle ( )
    {
        movement.Stumble ( );
    }
}