using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardClearer : MonoBehaviour
{
    public void ClearLeaderboard ( )
    {
        LeaderBoardData.Saved.ClearData ( );
    }
}
