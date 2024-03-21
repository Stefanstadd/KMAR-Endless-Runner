using System;
using System.Collections.Generic;

[System.Serializable]
public struct LeaderBoardEntry : IEquatable<LeaderBoardEntry>, IComparable<LeaderBoardEntry>
{
    public string name;

    public float metersRan;

    public int robbersCaught;

    public float timeAlive;

    public float AverageTimeBetweenRobberCaught
    {
        get
        {
            if ( robbersCaught == 0 )
                return float.NaN;

            return timeAlive / robbersCaught;
        }
    }

    public LeaderBoardEntry ( string name, float metersRan, int robbersCaught, float timeAlive )
    {
        this.name = name;
        this.metersRan = metersRan;
        this.robbersCaught = robbersCaught;
        this.timeAlive = timeAlive;
    }

    public bool Equals ( LeaderBoardEntry entry )
    {
        return entry.name == name &&
            entry.metersRan == metersRan &&
            entry.robbersCaught == robbersCaught;
    }

    public int CompareTo ( LeaderBoardEntry other )
    {
        //-1 equals lower score than other
        // 0 means same score than other
        // 1 means higher score than other

        if ( metersRan < other.metersRan )
        {
            return -1;
        }

        if ( robbersCaught < other.robbersCaught )
        {
            return -1;
        }

        if ( metersRan == other.metersRan && robbersCaught == other.robbersCaught )
        {
            return 0;
        }

        return 1;
    }

    //public override String ToString ( )
    //{
    //    var sb = new StringBuilder ( );
    //
    //    sb.AppendLine ($"name: {name}");
    //    sb.AppendLine ($"Meters Ran: {metersRan}");
    //    sb.AppendLine ($"Robbers Caught: {robbersCaught}");
    //    sb.AppendLine ($"Time Alive: {timeAlive}");
    //
    //    if ( float.IsNaN (AverageTimeBetweenRobberCaught) )
    //    {
    //        sb.AppendLine ($"ATBR: -");
    //    }
    //    else
    //    {
    //        sb.AppendLine ($"ATBR: {AverageTimeBetweenRobberCaught}");
    //    }
    //
    //    return sb.ToString ( );
    //}
}

public class LeaderBoardEntryComparer : IComparer<LeaderBoardEntry>
{

    readonly EntrySortMode mode;
    readonly bool reverse;

    public LeaderBoardEntryComparer ( EntrySortMode mode = EntrySortMode.METERS_RAN, bool reverse = false )
    {
        this.mode = mode;
        this.reverse = reverse;
    }

    public int Compare ( LeaderBoardEntry x, LeaderBoardEntry y )
    {
        if ( reverse )
        {
            return mode switch
            {
                EntrySortMode.METERS_RAN =>x.metersRan.CompareTo (y.metersRan),
                EntrySortMode.ROBBERS_CAUGHT =>x.robbersCaught.CompareTo (y.robbersCaught),
                EntrySortMode.TIME_ALIVE =>x.timeAlive.CompareTo (y.timeAlive),
                EntrySortMode.TIME_BETWEEN_CATCHES =>x.AverageTimeBetweenRobberCaught.CompareTo (y.AverageTimeBetweenRobberCaught),
                EntrySortMode.NAME =>x.name.CompareTo (y.name),
                _ =>x.CompareTo (y),
            };
        }
        else
        {
            return mode switch
            {
                EntrySortMode.METERS_RAN => y.metersRan.CompareTo (x.metersRan),
                EntrySortMode.ROBBERS_CAUGHT => y.robbersCaught.CompareTo (x.robbersCaught),
                EntrySortMode.TIME_ALIVE => y.timeAlive.CompareTo (x.timeAlive),
                EntrySortMode.TIME_BETWEEN_CATCHES => y.AverageTimeBetweenRobberCaught.CompareTo (x.AverageTimeBetweenRobberCaught),
                EntrySortMode.NAME => y.name.CompareTo (x.name),
                _ => y.CompareTo (x),
            };
        }
    }
}
public enum EntrySortMode
{
    METERS_RAN,
    ROBBERS_CAUGHT,
    TIME_ALIVE,
    TIME_BETWEEN_CATCHES,
    NAME
}
