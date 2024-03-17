using TMPro;
using UnityEngine;

public class RobberTracker : MonoBehaviour
{
    public TextMeshProUGUI textElement;

    public int robberAmount;

    public Transform handCuffs;

    public void OnRobberDied ( )
    {
        robberAmount++;

        textElement.text = robberAmount.ToString ( );

        Coms.UIPulseEffect.ApplyEffect (handCuffs, textElement.transform);
    }
}