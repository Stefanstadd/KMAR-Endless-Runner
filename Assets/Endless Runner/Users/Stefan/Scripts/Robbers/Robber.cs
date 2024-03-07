using UnityEngine;

public class Robber : NodeSampler
{
    public float movementSpeed;
    public float horizontalSmoothTime;
    public float rotationSpeed;
    public float horizontalMax;

    public Vector2 moveChangeMinMax;

    public Transform body;

    float timeToChangeMove;
    float moveChangeTimer;

    float targetHorizontal;
    float horizontal;

    float horVelocity;

    Vector3 rotationVelocity;

    // Start is called before the first frame update
    void Start ( )
    {

    }

    // Update is called once per frame
    protected void Update ( )
    {
        if ( CurrentNode == null || NextNode == null )
            return;

        moveChangeTimer += Time.deltaTime;

        if ( moveChangeTimer > timeToChangeMove )
        {
            moveChangeTimer = 0;
            timeToChangeMove = Random.Range (moveChangeMinMax.x, moveChangeMinMax.y);

            targetHorizontal = Random.Range (-horizontalMax, horizontalMax);
        }

        // Calculate horizontal movement

        
        horizontal = Mathf.SmoothDamp(horizontal,targetHorizontal, ref horVelocity, horizontalSmoothTime);

        //apply horizontal movement

        Vector3 bodyPos = new (horizontal, 0, 0);

        body.localPosition = bodyPos;

        //Calculate and apply rotation

        Vector3 direction = ( NextNode.position - CurrentNode.position ).normalized;

        transform.forward = Vector3.SmoothDamp (transform.forward, direction, ref rotationVelocity, rotationSpeed);

        // Apply forward movement

        float step = movementSpeed * Time.deltaTime;

        float dst = Vector3.Distance (transform.position, NextNode.position);

        step = Mathf.Clamp (step, 0, dst);

        transform.position = Vector3.Lerp (transform.position, NextNode.position + offset, ( step + float.Epsilon ) / dst);

        if ( dst < 0.1f )
        {
            MoveToNextNode ( );
        }

    }
}
