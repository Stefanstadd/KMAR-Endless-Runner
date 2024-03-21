using UnityEngine;

public class MainMenuCamera : LaneMovement
{
    public float movementSpeed;

    public float sideSmoothTime;

    public float rotSmoothTime;



    Vector3 sideVelocity, rotVelocity;

    public Transform body;

    private void Start ( )
    {
    }
    // Update is called once per frame
    void Update ( )
    {
        if ( CurrentNode == null || NextNode == null )
        {
            SetCurrentNode (Coms.WorldGenerator.head);
            return;
        }

        if ( Input.GetButtonDown ("Horizontal") )
        {
            float inp = Input.GetAxisRaw ("Horizontal");
            if ( inp < 0 )
            {
                MoveToLaneLeft ( );
            }
            else if ( inp > 0 )
            {
                MoveToLaneRight ( );
            }
        }

        //Horizontal movement
        Vector3 targetPos = Vector3.SmoothDamp (body.localPosition, CurrentLanePosition + offset, ref sideVelocity, sideSmoothTime);

        body.localPosition = targetPos;

        float step = movementSpeed * Time.deltaTime;

        float dst = Vector3.Distance (transform.position, NextNode.position);

        step = Mathf.Clamp (step, 0, dst);

        transform.position = Vector3.Lerp (transform.position, NextNode.position, ( step + float.Epsilon ) / dst);

        //Rotation

        Vector3 dir = ( NextNode.position - CurrentNode.position ).normalized;

        transform.forward = Vector3.SmoothDamp (transform.forward, dir, ref rotVelocity, rotSmoothTime);

        if ( dst <= 0.1f )
        {
            MoveToNextNode ( );
        }
    }
}
