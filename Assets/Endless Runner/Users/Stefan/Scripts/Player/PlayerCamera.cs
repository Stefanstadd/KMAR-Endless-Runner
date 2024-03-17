using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private new Camera camera;
    public PlayerMovement player;
    public Transform target;
    public float movSpeed, rotSpeed;

    public Vector2 fovMap;

    private Vector3 movVelocity;
    private Vector3 rotVelocity;

    private void Start ( )
    {
        camera = GetComponent<Camera> ( );
    }

    // Update is called once per frame
    private void Update ( )
    {
        if ( player == null || target == null )
            return;

        transform.position = Vector3.SmoothDamp (transform.position, target.position, ref movVelocity, movSpeed);

        transform.forward = Vector3.SmoothDamp (transform.forward, target.forward, ref rotVelocity, rotSpeed);

        camera.fieldOfView = Mathf.Lerp (fovMap.x, fovMap.y, Mathf.InverseLerp (player.forwardMovementSpeed, player.forwardMaxSpeed, player.CurrentMovementSpeed));
    }
}