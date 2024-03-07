using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    new Camera camera;
    public PlayerMovement player;
    public Transform target;
    public float movSpeed, rotSpeed;

    public Vector2 fovMap;

    Vector3 movVelocity;
    Vector3 rotVelocity;

    private void Start ( )
    {
        camera = GetComponent<Camera> ( );   
    }
    // Update is called once per frame
    void Update()
    {
        if ( player == null || target == null )
            return;

        transform.position = Vector3.SmoothDamp (transform.position, target.position, ref movVelocity, movSpeed);

        transform.forward = Vector3.SmoothDamp (transform.forward, target.forward, ref rotVelocity, rotSpeed);

        camera.fieldOfView = Mathf.Lerp (fovMap.x, fovMap.y, Mathf.InverseLerp (player.forwardMovementSpeed, player.forwardMaxSpeed, player.CurrentMovementSpeed));

    }
}
