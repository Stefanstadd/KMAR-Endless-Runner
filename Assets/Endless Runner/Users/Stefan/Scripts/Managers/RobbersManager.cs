using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RobbersManager : MonoBehaviour
{
    public List<Robber> robbers = new ( );
    public int maxActiveRobbers;
    public int robbersStartAmount;
    public int activeRobbers;

    public int nodesBeforePlayer = 3;

    public float robberSpawnDelay;

    public GameObject robberPrefab;

    private float spawnTimer;

    public UnityEvent onRobberCaught;

    private void Start ( )
    {
        //StartGame ( );
    }

    private void StartGame ( )
    {
        for ( int i = 0; i < robbersStartAmount; i++ )
        {
            SpawnRobber ( );
        }
    }

    private void Update ( )
    {
        if ( spawnTimer < robberSpawnDelay )
        {
            spawnTimer += Time.deltaTime;
        }
        else if ( activeRobbers < maxActiveRobbers )
        {
            spawnTimer = 0;
            SpawnRobber ( );
        }
    }

    private void SpawnRobber ( )
    {
        activeRobbers++;
        Debug.Log ("Robber has spawned!");

        WorldNode startnode = WorldNode.NodeAtIndex (Coms.PlayerMovement.CurrentNode, nodesBeforePlayer);

        var robber = Instantiate (robberPrefab, startnode.position, Quaternion.identity).GetComponent<Robber> ( );

        if ( robber )
        {
            robber.SetCurrentNode (startnode);
        }

        robbers.Add (robber);
    }

    private Vector3 CalculateSpawnPos ( )
    {
        return WorldNode.NodeContainingPosition (Coms.PlayerMovement.CurrentNode, Coms.PlayerMovement.transform.position).position;
    }

    public void CatchRobber ( Robber robber )
    {
        robbers.Remove (robber);
        Destroy (robber.gameObject);
        OnRobberCaught ( );
    }

    private void OnRobberCaught ( )
    {
        activeRobbers--;

        onRobberCaught.Invoke ( );
        Debug.Log ("Robber caught!");
    }
}