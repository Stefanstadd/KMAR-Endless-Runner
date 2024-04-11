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

    public float catchDst;

    public Animator animator;

    private Vector3 rotationVelocity;
    private Vector3 movementVelocity;

    //Jumping variables
    private bool shouldJump;

    private bool isJumping;

    private float currentJumpTimer;

    bool hasHandledLaneSwitching;
    // Update is called once per frame
    protected void Update ( )
    {
        if ( CurrentNode == null || NextNode == null )
            return;

        // Handle Horizontal Movement

        float nextNodeDst = Vector3.Distance (transform.position, NextNode.position);

        if ( nextNodeDst < minDstToCheckForObstacle && !hasHandledLaneSwitching)
        {
            HandleLaneSwitching ( );
            hasHandledLaneSwitching = true;
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

                animator.SetTrigger ("Jump");
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

        Debug.Log (CurrentLanePosition);

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
            hasHandledLaneSwitching = false;
        }

        HandleCatch ( );
    }

    private void HandleLaneSwitching ( )
    {
        switch ( CurrentLane )
        {
            case -1: // Left lane case

                if ( NextNode.leftLane != LaneState.FREE )
                {
                    if ( ShouldAvoidLane(NextNode.leftLane)) // Move to the next lane to the right(middle lane)
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
                    if ( ShouldAvoidLane(NextNode.middleLane)) // Check if we can jump to the left or right
                    {
                        bool canJumpLeft = !ShouldAvoidLane(NextNode.leftLane);
                        bool canJumpRight = !ShouldAvoidLane(NextNode.rightLane);

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

                            break;
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
                    Debug.Log ("Should jump");

                }

                break;

            case 1: // Right lane case
                if ( NextNode.rightLane != LaneState.FREE )
                {
                    if ( ShouldAvoidLane(NextNode.rightLane)) // Move to the next lane to the left(middle lane)
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

    private bool ShouldAvoidLane(LaneState lane )
    {
        if ( lane == LaneState.BLOCKED )
            return true;

        if ( lane == LaneState.JUMPABLE && Random.value >= 0.5f )
        {
            return true;
        }

        return false;
    }

    void HandleCatch ( )
    {
        if(Vector3.Distance(Coms.PlayerMovement.playerBody.position,body.position) < catchDst || Coms.PlayerMovement.transform.position.z >= transform.position.z)
        {
            Catch ( );
        }
    }
    public void Catch ( )
    {
        Coms.RobbersManager.CatchRobber (this);
    }
}