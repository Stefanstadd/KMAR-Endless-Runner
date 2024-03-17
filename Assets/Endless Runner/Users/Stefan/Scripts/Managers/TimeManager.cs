using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public float maxTime;

    [SerializeField]
    private float _currentTime;

    public UnityEvent OnTimerReachedZero, OnTimerReset;

    public UnityEvent<float> OnTimerUpdate;

    private bool paused;

    public float CurrentTime
    {
        get
        {
            return _currentTime;
        }
        private set
        {
            if ( value == _currentTime )
                return;

            _currentTime = value;
        }
    }

    private void Start ( )
    {
        ResetTimer ( );
    }

    private void Update ( )
    {
        if ( paused )
            return;
        if ( CurrentTime > 0 )
        {
            CurrentTime -= Time.deltaTime;

            if ( CurrentTime <= 0 )
            {
                OnTimerReachedZero.Invoke ( );
            }
            OnTimerUpdate.Invoke (CurrentTime);
        }
    }

    public void SetMaxTime ( float maxTime )
    {
        this.maxTime = maxTime;
    }

    public void ResetTimer ( )
    {
        CurrentTime = maxTime;

        OnTimerReset.Invoke ( );
    }

    public void Pause ( bool paused )
    {
        this.paused = paused;
    }
}