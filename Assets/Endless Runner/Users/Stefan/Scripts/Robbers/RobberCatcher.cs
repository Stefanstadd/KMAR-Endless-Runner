using UnityEngine;

public class RobberCatcher : MonoBehaviour
{
    public PlayerMovement player;
    public float catchDistance;

    private void FixedUpdate ( )
    {
        if ( Input.GetMouseButtonDown (0) )
        {
            foreach ( var robber in Coms.RobbersManager.robbers )
            {
                float dst = Vector3.Distance (robber.body.transform.position, player.playerBody.position);

                if(dst < catchDistance )
                {
                    Coms.RobbersManager.CatchRobber (robber);
                }
            }
        }
    }
}
