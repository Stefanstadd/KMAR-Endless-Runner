using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LeaderBoardUI : MonoBehaviour
{
    public Transform leaderBoardParent;

    public GameObject statistic;

    public GameObject noData;

    public int maxEntriesToShow = 200;

    private void Start ( )
    {
        LoadStatistics ( );
    }

    public void LoadStatistics ( EntrySortMode mode = EntrySortMode.METERS_RAN, bool reverse = false)
    {
        ClearPreviousStatistics();

        var entries = LeaderBoardData.Saved.savedEntries.data;

        if(entries.Count == 0 )
        {
            noData.SetActive (true);
            return;
        }

        noData.SetActive (false);

        IComparer<LeaderBoardEntry> comparer = new LeaderBoardEntryComparer (mode, reverse);

        entries.Sort (comparer);

        for ( int i = 0; i < Mathf.Min(maxEntriesToShow,entries.Count); i++ )
        {
            LeaderBoardElement element = Instantiate (statistic, leaderBoardParent).GetComponent<LeaderBoardElement>();

            element.Initialize (i + 1, entries[i]);
        }
    }

    void ClearPreviousStatistics ( )
    {
        leaderBoardParent.DeleteChildren ( );
    }

    public void LoadByMeters ( ) => LoadStatistics (EntrySortMode.METERS_RAN);
    public void LoadByRobbersCaught ( ) => LoadStatistics (EntrySortMode.ROBBERS_CAUGHT);
    public void LoadByTimeAlive ( ) => LoadStatistics (EntrySortMode.TIME_ALIVE);
    public void LoadByTimeBetweenCatches ( ) => LoadStatistics (EntrySortMode.TIME_BETWEEN_CATCHES);
    public void LoadByName ( ) => LoadStatistics (EntrySortMode.NAME);
}
