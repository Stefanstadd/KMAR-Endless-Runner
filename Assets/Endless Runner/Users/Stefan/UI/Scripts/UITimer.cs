using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{
    [SerializeField]
    Image fill;

    [SerializeField]
    Transform pointer;

    [SerializeField]
    float maxTime;

    [SerializeField]
    float _currentTime;

    float CurrentTime
    {
        get
        {
            return _currentTime;
        }
        set
        {
            if ( value == _currentTime )
                return;

            _currentTime = value;

            OnCurrentTimeChanged ( );
        }
    }

    void OnCurrentTimeChanged ( )
    {
        float progress = Mathf.InverseLerp (0, maxTime, CurrentTime);

        fill.fillAmount = progress;

        var angles = pointer.localEulerAngles;
        angles.z = 360 * progress;
        pointer.localEulerAngles = angles;
    }

    public void SetMaxTime(float max )
    {
        maxTime = max;
        OnCurrentTimeChanged( );
    }

    public void SetTime(float time )
    {
        CurrentTime = time;
    }
    public void AddTime(float addTime )
    {
        CurrentTime += addTime;
    }
}
