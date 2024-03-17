using UnityEngine;
using UnityEngine.VFX;

public class ParticlePlayer : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem[] _systems;

    [SerializeField]
    private VisualEffect[] _effects;

    public void Play ( )
    {
        foreach ( var system in _systems )
        {
            system.Play ( );
        }

        foreach ( var effect in _effects )
        {
            effect.Play ( );
        }
    }

    public void Stop ( )
    {
        foreach ( var system in _systems )
        {
            system.Stop ( );
        }

        foreach ( var effect in _effects )
        {
            effect.Stop ( );
        }
    }
}