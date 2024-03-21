using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntryTracker : MonoBehaviour
{
    public string Name = "PLAYER_NAME";
    public PlayerMovement movement;
    public RobberTracker robberTracker;

    float distanceRan;

    float timeAlive;

    private void Update ( )
    {
        distanceRan += movement.CurrentMovementSpeed * Time.deltaTime;
        timeAlive += Time.deltaTime;
    }
    public void RegisterEntry ( )
    {
        Name = LeaderBoardData.Saved.lastUsedName;

        LeaderBoardEntry entry = new (Name, distanceRan, robberTracker.robberAmount, timeAlive);

        LeaderBoardData.Saved.AddEntry (entry);
    }
}
