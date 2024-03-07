using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DistanceTracker : MonoBehaviour
{
    public PlayerMovement player;

    public TextMeshProUGUI textElement;

    double currentDistance; //total distance in meters


    private void Update ( )
    {
        if ( !player )
            return;

        currentDistance += player.CurrentMovementSpeed * Time.deltaTime;

        if(currentDistance <= 5000 )
        {
            textElement.text = $"{currentDistance:F1} M";
        }
        else
        {
            textElement.text = $"{(currentDistance / 1000):F1} KM";
        }

    }
}
