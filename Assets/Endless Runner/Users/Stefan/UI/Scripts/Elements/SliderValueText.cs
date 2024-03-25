using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueText : MonoBehaviour
{
    public Slider slider;

    public TextMeshProUGUI text;

    public string format = "F1";

    private void Start ( )
    {
        slider.onValueChanged.AddListener (( f ) => text.text = f.ToString (format));
    }
}
