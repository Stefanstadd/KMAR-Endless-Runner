using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement movement;
    public PlayerCamera camera;

    public float killShakeStrength, stumbleShakeStrength;
    public float shakeDuration = 0.8f;

    public void InstaKillObstacle ( )
    {
        movement.Die ( );
        camera.Shake (killShakeStrength, shakeDuration);

    }

    public void StumbleObstacle ( )
    {
        movement.Stumble ( );
        camera.Shake (stumbleShakeStrength, shakeDuration);
    }
}