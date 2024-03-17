using UnityEngine;

public class NodeSampleTest : LaneMovement
{
    public float movementSpeed = 5;

    public float rotationSpeed;

    private Vector3 rotVelocity;

    protected void Update ( )
    {
        if ( CurrentNode == null || NextNode == null )
            return;

        Debug.Log (WorldNode.PositionInNode (CurrentNode, SamplePosition));

        // Rotation
        Vector3 direction = ( CurrentNode.position - NextNode.position ).normalized;

        Vector3 rotation = new (direction.x * -90, 0, 0);

        transform.forward = Vector3.SmoothDamp (transform.forward, rotation, ref rotVelocity, rotationSpeed);

        //Moving

        float dst = Vector3.Distance (transform.position, NextNode.position + offset);

        float step = movementSpeed * Time.deltaTime;

        step = Mathf.Clamp (step, 0f, dst);

        transform.position = Vector3.Lerp (transform.position, NextNode.position + offset, ( step + float.Epsilon ) / dst);

        if ( dst < 0.2f )
        {
            MoveToNextNode ( );
        }
    }
}