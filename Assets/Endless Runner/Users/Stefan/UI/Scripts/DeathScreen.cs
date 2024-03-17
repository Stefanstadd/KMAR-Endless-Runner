using TMPro;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI loseReason, distance, robbersCaught;

    public void SetDeathScreen ( string deathReason, double distance, int robbersCaught )
    {
        loseReason.text = deathReason;

        this.distance.text = $"Distance Ran: {distance:F1}M";

        this.robbersCaught.text = $"Robbers Caught: {robbersCaught}";
    }

    public void Enable ( bool enabled )
    {
        gameObject.SetActive (enabled);
    }
}