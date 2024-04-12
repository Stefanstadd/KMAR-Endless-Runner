using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardElement : MonoBehaviour
{
    public Color numberOneColor, numberTwoColor, numberThreeColor, otherColor;

    public Vector3 numOneScale;

    public TextMeshProUGUI numberElement, nameElement, distanceElement, robberCaughtElement, timeAliveElement, timePerRobberElement;

    public void Initialize ( int number, LeaderBoardEntry entry )
    {
        numberElement.text = $"{number}.";

        nameElement.text = entry.name;
        distanceElement.text = $"{entry.metersRan:F1} M";
        robberCaughtElement.text = $"{entry.robbersCaught}";
        timeAliveElement.text = TimeSpan.FromSeconds (entry.timeAlive).ToString (@"hh\:mm\:ss");

        float time = entry.AverageTimeBetweenRobberCaught;

        if ( float.IsNaN (time) )
        {
            timePerRobberElement.text = "-";
        }
        else
        {
            timePerRobberElement.text = $"{entry.AverageTimeBetweenRobberCaught:F2}";
        }

        switch ( number )
        {
            case 1:
                numberElement.transform.localScale = numOneScale;

                SetColors (numberOneColor);
                break;
            case 2:
                SetColors (numberTwoColor);
                break;
            case 3:
                SetColors (numberThreeColor);
                break;
            default:
                SetColors (otherColor);
                break;
        }
    }

    void SetColors ( Color colr )
    {
        SetElementsColor (colr, numberElement, nameElement, distanceElement, robberCaughtElement, timeAliveElement, timePerRobberElement);
    }

    void SetElementsColor ( Color col, params Graphic[] graphics )
    {
        foreach ( var g in graphics )
        {
            g.color = col;
        }
    }
}
