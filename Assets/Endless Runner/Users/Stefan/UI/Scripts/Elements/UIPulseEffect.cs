using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UIPulseEffect;

public class UIPulseEffect : MonoBehaviour
{
    private List<PulseObject> _activeObjects = new ( );

    private HashSet<int> indicesToRemove = new();


    public Color startColor;
    public Color transitionColor;

    public Vector3 minScale, maxScale;

    public float maxDuration;


    private void Update ( )
    {
        for( int i = 0; i < _activeObjects.Count; i++ )
        {
            UpdatePulse (_activeObjects[i],i);
        }

        foreach ( int index in indicesToRemove )
        {
            if ( _activeObjects.Count <= index )
                continue;
            var pulse = _activeObjects[index];

            Destroy (pulse.transform.gameObject);

            _activeObjects.RemoveAt (index);
        }
        indicesToRemove.Clear ( );
    }

    void UpdatePulse(PulseObject pulseObject, int index)
    {
        pulseObject.currentDuration += Time.deltaTime;

        if(pulseObject.currentDuration > pulseObject.maxDuration )
        {
            indicesToRemove.Add (index);
            return;
        }

        float progress = pulseObject.Progress;
        Vector3 scale = Vector3.Lerp (pulseObject.minScale, pulseObject.maxScale, progress);

        pulseObject.transform.localScale = scale;
        
        for ( int i = 0; i < pulseObject.graphics.Count; i++ )
        {
            pulseObject.graphics[i].color = Color.Lerp (startColor, transitionColor, progress);
        }
    }

    public void ApplyEffect(params Transform[] objects )
    {
        foreach ( var obj in objects )
        {
            ApplyEffect (obj);
        }
    }

    public void ApplyEffect(Transform transform)
    {
        PulseObject data = new PulseObject ( );
        data.maxDuration = maxDuration;

        var clone = Instantiate (transform, transform.position, transform.rotation, transform);
        data.transform = clone;

        data.graphics.AddRange (clone.GetComponents<Graphic> ( ));

        data.minScale = minScale;
        data.maxScale = maxScale;


        _activeObjects.Add (data);
    }

    public class PulseObject
    {
        public Transform transform;

        public List<Graphic> graphics = new();

        public Vector3 minScale;
        public Vector3 maxScale;

        public float maxDuration;
        public float currentDuration;

        public float Progress
        {
            get
            {
                return Mathf.InverseLerp (0, maxDuration, currentDuration);
            }
        }

    }
}
