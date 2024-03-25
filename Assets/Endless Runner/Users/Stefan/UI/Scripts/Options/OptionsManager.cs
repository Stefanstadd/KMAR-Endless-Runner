using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public delegate void OptionsEvent ( OptionsData data );

    public event OptionsEvent ApplyOptions;

    public Selector resolutionSelector;

    public Toggle fullScreenToggle;

    public Slider volumeSlider, fovSlider, camDstSlider;

    public GameObject applyButton;

    public bool isDirty = false;
    private void OnEnable ( )
    {
        LoadElements ( );
    }

    public void MarkAsDirty ( )
    {
        if ( !isDirty )
            applyButton.SetActive (true);

        isDirty = true;
    }

    public void ApplyChanges ( )
    {
        isDirty = false;

        applyButton.SetActive (false);

        SaveElements ( );

        OnOptionsApplied ( );
    }

    public void LoadElements ( )
    {
        OptionsData data = OptionsData.GetSavedData ( );

        resolutionSelector.SetIndex (data.resolutionIndex);

        fullScreenToggle.SetIsOnWithoutNotify (data.fullScreen);

        volumeSlider.value = data.volume;

        fovSlider.value = data.fov;

        camDstSlider.value = data.cameraDst;

        data.SaveData ( );
    }

    void SaveElements ( )
    {
        OptionsData data = OptionsData.GetSavedData ( );

        data.resolutionIndex = resolutionSelector.GetIndex ();

        data.fullScreen = fullScreenToggle.isOn;

        data.volume = volumeSlider.value;

        data.fov = fovSlider.value;

        data.cameraDst = camDstSlider.value;
    }

    public void OnOptionsApplied ( )
    {
        ApplyOptions?.Invoke (OptionsData.GetSavedData ( ));
    }
}
