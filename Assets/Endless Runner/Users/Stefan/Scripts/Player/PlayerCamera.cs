using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private new Camera camera;
    public PlayerMovement player;
    public Transform target;
    public Transform objectToMove;
    public float movSpeed, rotSpeed;

    public Vector2 fovMap;

    private Vector3 movVelocity;
    private Vector3 rotVelocity;

    public float basePositionStrength = 1, baseRotStrength = 3;
    float shakeIntensity;
    float shakeDuration;

    private void Start ( )
    {
        camera = GetComponent<Camera> ( );
    }

    // Update is called once per frame
    private void Update ( )
    {
        if(shakeDuration <= 0 )
        {
            shakeDuration -= Time.deltaTime;

            transform.localPosition = basePositionStrength *shakeDuration * shakeIntensity * Random.onUnitSphere;

            transform.localEulerAngles = baseRotStrength * shakeDuration * shakeIntensity * Random.onUnitSphere;
        }

        if ( player == null || target == null )
            return;

        Transform toMove = objectToMove? objectToMove : transform;

        toMove.position = Vector3.SmoothDamp (toMove.position, target.position, ref movVelocity, movSpeed);

        toMove.forward = Vector3.SmoothDamp (toMove.forward, target.forward, ref rotVelocity, rotSpeed);

        camera.fieldOfView = Mathf.Lerp (fovMap.x, fovMap.y, Mathf.InverseLerp (player.forwardMovementSpeed, player.forwardMaxSpeed, player.CurrentMovementSpeed));
    }

    public void Shake(float intensity, float duration )
    {
        shakeIntensity = intensity;
        shakeDuration = duration;
    }
}