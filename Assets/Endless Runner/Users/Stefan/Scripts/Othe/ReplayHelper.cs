using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayHelper : MonoBehaviour
{
    public string sceneName;
    public void Replay ( )
    {
        SceneManager.LoadScene (sceneName);
    }
}