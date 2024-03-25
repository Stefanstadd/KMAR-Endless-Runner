using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public TextMeshProUGUI textElement;

    public string[] options = new[]{"Option 1", "Option 2", "Option 3" };

    private int currentIndex;

    public void PreviousOption ( )
    {
        currentIndex--;
        if(currentIndex < 0 )
        {
            currentIndex = options.Length - 1;
        }

        OnOptionIndexChanged ( );
    }

    public void NextOption ( )
    {
        currentIndex++;
        if(currentIndex > options.Length - 1 )
        {
            currentIndex = 0;
        }

        OnOptionIndexChanged ( );
    }

    public void SetIndex (int value)
    {
        currentIndex = value;
        OnOptionIndexChanged ( );
    }

    public int GetIndex ( ) => currentIndex;
    void OnOptionIndexChanged ( )
    {
        Debug.Log ("Seks");
        textElement.text = options[currentIndex];
    }
}
