using UnityEngine;

public class Robber : LaneMovement
{
    public float movementSpeed;
    public float movementSmoothTime;

    public float jumpTime = 0.8f;
    public float jumpHeight = 1.5f;

    public float rotationSpeed;
    public Transform body;

    public float minDstToCheckForObstacle = 5;
    public float minDstToJumpObstacle = 2;

    private Vector3 rotationVelocity;
    private Vector3 movementVelocity;

    //Jumping variables
    private bool shouldJump;

    private bool isJumping;

    private float currentJumpTimer;

    // Update is called once per frame
    protected void Update ( )
    {
        if ( CurrentNode == null || NextNode == null )
            return;

        // Handle Horizontal Movement

        float nextNodeDst = Vector3.Distance (transform.position, NextNode.position);

        if ( nextNodeDst < minDstToCheckForObstacle )
        {
            HandleLaneSwitching ( );
        }

        //Calculate jumping

        //Calculate Jump

        float jump = 0;

        if ( !isJumping )
        {
            if ( shouldJump )
            {
                isJumping = true;
                currentJumpTimer = 0;
            }
        }

        if ( isJumping )
        {
            currentJumpTimer += Time.deltaTime;

            if ( currentJumpTimer < jumpTime )
            {
                float progress = Mathf.InverseLerp (0, jumpTime, currentJumpTimer);

                progress = Mathf.Clamp01 (progress);

                float sin = Mathf.Sin (progress * Mathf.PI * 2 - Mathf.PI / 2);

                float jumpLerp = Mathf.InverseLerp (-1, 1, sin);

                jump = Mathf.Lerp (0, jumpHeight, jumpLerp);
            }
            else if ( currentJumpTimer > jumpTime )
            {
                isJumping = false;
                jump = 0;

                shouldJump = false;
            }
        }

        Vector3 targetPos = Vector3.SmoothDamp (body.localPosition, CurrentLanePosition, ref movementVelocity, movementSmoothTime);

        //TODO: Add jumping
        targetPos.y = jump;

        body.localPosition = targetPos;

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

    private void HandleLaneSwitching ( )
    {
        switch ( CurrentLane )
        {
            case -1: // Left lane case

                if ( NextNode.leftLane != LaneState.FREE )
                {
                    if ( NextNode.leftLane == LaneState.BLOCKED ) // Move to the next lane to the right(middle lane)
                    {
                        MoveToLaneRight ( );
                        break;
                    }

                    //Robber can jump over obstacle
                    shouldJump = true;
                }

                break;

            case 0: // Middle lane case

                if ( NextNode.middleLane != LaneState.FREE )
                {
                    if ( NextNode.middleLane == LaneState.BLOCKED ) // Check if we can jump to the left or right
                    {
                        bool canJumpLeft = NextNode.leftLane != LaneState.BLOCKED;
                        bool canJumpRight = NextNode.rightLane != LaneState.BLOCKED;

                        if ( canJumpLeft && canJumpRight )  //we can jump to both sides so choose randomly
                        {
                            if ( Random.Range (0, 100) < 50 )
                            {
                                MoveToLaneLeft ( );
                            }
                            else
                            {
                                MoveToLaneRight ( );
                            }
                        }
                        else if ( canJumpLeft )
                        {
                            MoveToLaneLeft ( );
                        }
                        else if ( canJumpRight )
                        {
                            MoveToLaneRight ( );
                        }
                        else //all lanes are blocked, something went wrong in generation
                        {
                            Debug.LogError ($"All lanes are blocked on node {NextNode}", gameObject);
                        }
                        break;
                    }

                    //Robber can jump over obstacle
                    shouldJump = true;
                }

                break;

            case 1: // Right lane case
                if ( NextNode.rightLane != LaneState.FREE )
                {
                    if ( NextNode.rightLane == LaneState.BLOCKED ) // Move to the next lane to the left(middle lane)
                    {
                        MoveToLaneLeft ( );
                        break;
                    }

                    //Robber can jump over obstacle
                    shouldJump = true;
                }
                break;

            default:
                Debug.LogError ($"Invalid Lane: {CurrentLane}");
                break;
        }
    }
}