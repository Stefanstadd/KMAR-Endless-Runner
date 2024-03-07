using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class Manhole : MonoBehaviour, IObstacle
{
    public float triggerRange, explodeRange;

    public bool isDefect;

    bool triggered, exploded;

    public Transform cover;

    public ParticlePlayer triggerEffect, explodeEffect;

    void IObstacle.Initialize ( )
    {
        triggered = false;
        exploded = false;
    }

    private void Update ( )
    {
        if ( !isDefect )
            return;

        float dst = Vector3.Distance (Coms.PlayerMovement.playerBody.position, transform.position);

        if (!triggered && dst < triggerRange )
        {
            //TODO: play particle]
            triggerEffect.Play ( );
            triggered = true;

            Debug.Log ("Triggered Manhole");
        }

        if (exploded == false && triggered && dst < explodeRange )
        {
            Explode ( );

            //TODO: Play Particle
            explodeEffect.Play ( );


            Destroy (cover.gameObject);
        }

    }

    void Explode ( )
    {
        exploded = true;
        Destroy (gameObject, 2F);
        Debug.Log ("Exploded");
    }

    private void OnCollisionEnter ( Collision collision )
    {
        if ( !exploded )
            return;

        var col = collision.transform.GetComponent<PlayerCollision> ( );

        if ( col )
        {
            col.InstaKillObstacle ( );
        }
    }
}
