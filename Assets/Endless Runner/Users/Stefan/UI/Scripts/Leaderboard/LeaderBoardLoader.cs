using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardLoader : UISelectable
{
    public static LeaderBoardLoader Selected;
    public LeaderBoardUI leaderBoardUI;
    public EntrySortMode sortMode;

    public bool isDefault;
    public Transform indicator;
    bool reverse = false;

    protected override void Start ( )
    {
        if ( isDefault )
        {
            SortAndLoad ( );
        }
    }

    public override void OnClick ( )
    {
        base.OnClick ( );
        SortAndLoad ( );
    }
    protected override void Update ( )
    {
        base.Update ( );

        if ( Selected == this  )
        {
            indicator.gameObject.SetActive (true);

            if ( reverse)
            {
                indicator.transform.up = Vector3.up;
            }
            else
            {
                indicator.transform.up = Vector3.down;
            }
        }
        else
        {
            indicator.gameObject.SetActive (false);
            reverse = false;
        }
    }
    public void SortAndLoad ( )
    {
        leaderBoardUI.LoadStatistics (sortMode,reverse);

        Selected = this;

        reverse = !reverse;
    }
}
