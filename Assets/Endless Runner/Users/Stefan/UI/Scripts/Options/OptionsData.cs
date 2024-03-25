using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class OptionsData
{
    const string SAVE_FILE_NAME = "Options.json";

    private static OptionsData m_savedData;

    public static OptionsData GetSavedData ( )
    {
        if(m_savedData == null )
        {
            string path = Path.Combine (Application.persistentDataPath, SAVE_FILE_NAME);

            if ( File.Exists (path) )
            {
                string json = File.ReadAllText (path);

                m_savedData = JsonUtility.FromJson<OptionsData> (json);
            }
            else
            {
                m_savedData = DefaultOptions;
            }
        }
        return m_savedData;
    }

    public int resolutionIndex;
    public bool fullScreen;

    public float volume;

    public float fov;
    public float cameraDst;

    private OptionsData()
    {}

    public void SaveData ( )
    {
        string json = JsonUtility.ToJson (this);

        string path = Path.Combine (Application.persistentDataPath, SAVE_FILE_NAME);

        File.WriteAllText (path, json);

        Debug.Log ("Saved Options");
    }

    /// <summary>
    /// The default options
    /// </summary>
    public static OptionsData DefaultOptions
    {
        get
        {
            return new OptionsData
            {
                resolutionIndex = 1,
                fullScreen = true,
                volume = 0.5f,
                fov = 60,
                cameraDst = 10,
            };
        }
    }

}
