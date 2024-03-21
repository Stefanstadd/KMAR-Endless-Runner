using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public TMP_InputField inputField;
    public ReplayHelper sceneLoader;
    public GameObject invalidText;

    private void Start ( )
    {
        inputField.text = LeaderBoardData.Saved.lastUsedName;
    }
    public void PlayGame ( )
    {
        if(inputField.text.Length >= 1 )
        {
            LeaderBoardData.Saved.lastUsedName = inputField.text;
            sceneLoader.Replay ( );
        }
        else
        {
            invalidText.SetActive (true);
        }
    }
}
