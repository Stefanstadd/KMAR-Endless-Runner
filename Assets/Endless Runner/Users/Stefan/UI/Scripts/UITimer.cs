using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{
    [SerializeField]
    private Image fill;

    [SerializeField]
    private Transform pointer;

    [SerializeField]
    private float maxTime;

    [SerializeField]
    private float _currentTime;

    [SerializeField]
    private Transform[] uiEffectObjects;

    private float CurrentTime
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

    private void OnCurrentTimeChanged ( )
    {
        float progress = Mathf.InverseLerp (0, maxTime, CurrentTime);

        fill.fillAmount = progress;

        var angles = pointer.localEulerAngles;
        angles.z = 360 * progress;
        pointer.localEulerAngles = angles;
    }

    public void SetMaxTime ( float max )
    {
        maxTime = max;
        OnCurrentTimeChanged ( );
    }

    public void SetTime ( float time )
    {
        CurrentTime = time;
    }

    public void AddTime ( float addTime )
    {
        CurrentTime += addTime;
    }

    public void PingTimer ( )
    {
        Coms.UIPulseEffect.ApplyEffect (uiEffectObjects);
    }
}