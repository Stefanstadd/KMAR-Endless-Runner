using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayHelper : MonoBehaviour
{
    public void Replay ( )
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene ( ).name);
    }
}