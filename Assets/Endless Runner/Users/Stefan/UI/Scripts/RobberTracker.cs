using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RobberTracker : MonoBehaviour
{
    public TextMeshProUGUI textElement;

    public int robberAmount;

    public Transform handCuffs;

    public void OnRobberDied( )
    {
        robberAmount++;

        textElement.text = robberAmount.ToString ( );

        Coms.UIPulseEffect.ApplyEffect (handCuffs, textElement.transform);

    }
}
