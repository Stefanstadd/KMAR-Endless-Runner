using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : LaneMovement
{
    public Transform playerBody;

    public float maxHorizontalMovement;

    [Header ("References")]
    public Animator animator;

    [Header ("Movement")]
    public float forwardMovementSpeed;

    public float forwardMaxSpeed;

    public float laneSmoothSpeed = 0.1f;

    public float movementAcceleration, movementDeacceleration;

    public float rotationSpeed;

    public float jumpTime;
    public float jumpHeight;
    public float jumpMovementBoost;
    public float jumpCooldown;

    [Header ("Obstacles")]
    public float stumbleTime = 0.3f;

    public float stumbleSpeed = 2f;

    private float stumbleTimer;

    private Vector3 rotationVelocity;

    public float CurrentMovementSpeed
    {
        get; private set;
    }

    private float forAccVelocity, forDeaccVelocity, sideAccVelocity;

    public GameObject gameOverScreen;

    public bool isDead;

    public UnityEvent OnPlayerDied;

    [Header ("Debug")]
    public TextMeshProUGUI speedText;

    private bool isJumping;

    private float currentJumpTimer;

    private Vector3 laneVelocity;

    private void Start ( )
    {
        CurrentMovementSpeed = forwardMovementSpeed;
        SetCurrentNode (Coms.WorldGenerator.head);
    }

    protected void Update ( )
    {
        if ( CurrentNode == null || NextNode == null )
            return;

        //Check for input to switch lanes

        if ( Input.GetButtonDown ("Horizontal") )
        {
            float input = Input.GetAxisRaw ("Horizontal");

            if ( input < 0 ) // Move left
            {
                MoveToLaneLeft ( );
            }
            else if ( input > 0 ) // move right
            {
                MoveToLaneRight ( );
            }
        }

        float possibleSpeed = stumbleTimer > 0 ? stumbleSpeed : forwardMaxSpeed;

        float targetSpeed = Input.GetAxisRaw ("Vertical") > 0 ? possibleSpeed : 0;

        stumbleTimer -= Time.deltaTime;
        if ( stumbleTimer > 0 )
        {
            CurrentMovementSpeed = Mathf.SmoothDamp (CurrentMovementSpeed, targetSpeed, ref forDeaccVelocity, movementDeacceleration);
        }
        else
        {
            CurrentMovementSpeed = Mathf.SmoothDamp (CurrentMovementSpeed, targetSpeed, ref forAccVelocity, movementAcceleration);
        }

        //Calculate Jump

        float jump = 0;

        if ( !isJumping )
        {
            if ( Input.GetButtonDown ("Jump") )
            {
                isJumping = true;
                currentJumpTimer = 0;

                CurrentMovementSpeed += jumpMovementBoost;
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
            else if ( currentJumpTimer + jumpCooldown > jumpTime )
            {
                isJumping = false;
                jump = 0;
            }
        }

        animator.SetBool ("IsJumping", isJumping);

        Vector3 bodyPos = Vector3.SmoothDamp (playerBody.localPosition, CurrentLanePosition, ref laneVelocity, laneSmoothSpeed);
        bodyPos.y = jump;

        playerBody.localPosition = bodyPos;

        // Apply forward movement

        float step = CurrentMovementSpeed * Time.deltaTime;

        float dst = Vector3.Distance (transform.position, NextNode.position);

        step = Mathf.Clamp (step, 0, dst);

        transform.position = Vector3.Lerp (transform.position, NextNode.position + offset, ( step + float.Epsilon ) / dst);

        if ( dst < 0.1f )
        {
            MoveToNextNode ( );
        }

        //Apply rotation

        Vector3 direction = ( NextNode.position - CurrentNode.position ).normalized;

        transform.forward = Vector3.SmoothDamp (transform.forward, direction, ref rotationVelocity, rotationSpeed);

        speedText.text = $"{CurrentMovementSpeed} m/s";
    }

    public void Stumble ( )
    {
        stumbleTimer = stumbleTime;
    }

    public void Die ( )
    {
        isDead = true;
        OnPlayerDied.Invoke ( );

        gameOverScreen.SetActive (true);
    }
}