using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class LeaderBoardData 
{
    const string SAVE_FILE_NAME = "Leaderboard.json";

    private static LeaderBoardData m_saved;
    public static LeaderBoardData Saved
    {
        get
        {
            if ( m_saved == null )
            {
                string path = Path.Combine (Application.persistentDataPath, SAVE_FILE_NAME);

                if ( File.Exists (path) )
                {
                    string json = File.ReadAllText (path);

                    m_saved = JsonUtility.FromJson<LeaderBoardData> (json);
                }
                else
                {
                    m_saved = new ( );
                }
            }

            return m_saved;
        }
    }


    public LeaderBoardEntriesData savedEntries = new ( );

    public LeaderBoardEntry lastEntry;

    public string lastUsedName;

    public void AddEntry ( LeaderBoardEntry entry )
    {
        lastEntry = entry;
        savedEntries.data.Add (entry);
        Save ( );
    }

    public void Save ( )
    {
        string json = JsonUtility.ToJson (this, true);

        string path = Path.Combine (Application.persistentDataPath, SAVE_FILE_NAME);

        File.WriteAllText (path, json);

        Debug.Log ("Saved Leaderboard!");
    }

    public void ClearData ( )
    {
        savedEntries.data.Clear ( );
        lastEntry = new ( );

        Save ( );
    }
}

[System.Serializable]
public class LeaderBoardEntriesData
{
    public List<LeaderBoardEntry> data = new List<LeaderBoardEntry> ( );

}
