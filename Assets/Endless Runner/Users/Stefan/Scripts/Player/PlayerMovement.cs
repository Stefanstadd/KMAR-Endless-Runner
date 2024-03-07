using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : NodeSampler
{
    public Transform playerBody;

    public float maxHorizontalMovement;

    [Header("Movement")]
    public float forwardMovementSpeed;
    public float forwardMaxSpeed;

    public float sidewaysSpeed;
    public float sidewaysAcceleration;

    public float movementAcceleration, movementDeacceleration;

    public float rotationSpeed;

    public float jumpTime;
    public float jumpHeight;
    public float jumpMovementBoost;
    public float jumpCooldown;

    [Header ("Obstacles")]
    public float stumbleTime = 0.3f;
    public float stumbleSpeed = 2f;

    float stumbleTimer;

    Vector3 rotationVelocity;

    public float CurrentMovementSpeed{ get; private set; }

    float forAccVelocity, forDeaccVelocity, sideAccVelocity;

    public GameObject gameOverScreen;

    public bool isDead;

    public UnityEvent OnPlayerDied;

    [Header ("Debug")]
    public TextMeshProUGUI speedText;
    private bool sideWays;

    private bool isJumping;

    private float currentJumpTimer;

    float xInput;

    float horizontalMovement;

    private void Start ( )
    {
        CurrentMovementSpeed = forwardMovementSpeed;
        SetCurrentNode (Coms.WorldGenerator.head);
    }

    protected void Update ( )
    {

        if ( CurrentNode == null || NextNode == null )
            return;

        xInput = Mathf.SmoothDamp(xInput,Input.GetAxis ("Horizontal"),ref sideAccVelocity, sidewaysAcceleration );

        sideWays = Mathf.Abs(xInput) > 0.1f;

        stumbleTimer -= Time.deltaTime;
        if(stumbleTimer > 0)
        {
            CurrentMovementSpeed = Mathf.SmoothDamp (CurrentMovementSpeed, stumbleSpeed, ref forDeaccVelocity, movementDeacceleration);
        }
        if (sideWays)
        {
            CurrentMovementSpeed = Mathf.SmoothDamp (CurrentMovementSpeed, forwardMovementSpeed, ref forDeaccVelocity, movementDeacceleration);
        }
        else
        {
            CurrentMovementSpeed = Mathf.SmoothDamp (CurrentMovementSpeed, forwardMaxSpeed, ref forAccVelocity, movementAcceleration);
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

                float sin = Mathf.Sin(progress * Mathf.PI * 2 - Mathf.PI / 2);

                float jumpLerp = Mathf.InverseLerp (-1, 1, sin);

                jump = Mathf.Lerp (0, jumpHeight, jumpLerp);
            }
            else if ( currentJumpTimer + jumpCooldown > jumpTime )
            {
                isJumping = false;
                jump = 0;
            }
        }
            
        //Apply horizontal movement

        horizontalMovement += xInput * sidewaysSpeed * Time.deltaTime;

        horizontalMovement = Mathf.Clamp (horizontalMovement, -maxHorizontalMovement, maxHorizontalMovement);

        Vector3 bodyPos = new (horizontalMovement, jump, 0);

        playerBody.localPosition = bodyPos;


        // Apply forward movement

        float step = CurrentMovementSpeed * Time.deltaTime;

        float dst = Vector3.Distance (transform.position, NextNode.position);

        step = Mathf.Clamp (step, 0, dst);

        transform.position = Vector3.Lerp(transform.position, NextNode.position + offset, (step + float.Epsilon) / dst);

        if(dst < 0.1f )
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
