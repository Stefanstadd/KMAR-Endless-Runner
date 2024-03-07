using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    IN_MENU,
    PLAYING,
    PAUSED,
    GAME_OVER,
    LOADING,
}


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameState state;

    public GameState State
    {
        get
        {
            return state;
        }
        private set
        {
            if ( value == state )
                return;

            state = value;

            OnGameStateChanged (state);
        }
    }

    void OnGameStateChanged (GameState state)
    {
        Debug.Log ($"Game State : {state}");
    }
}
