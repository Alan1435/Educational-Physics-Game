using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketFunction : MonoBehaviour
{
    [SerializeField] float thrust;

    [SerializeField] bool move = false;

    [SerializeField] float burnLength ;

    [SerializeField] ParticleSystem plumeExhaust ;
    [SerializeField] ParticleSystem plumeFlame ;
    [SerializeField] ParticleSystem firework ;

    private bool hasBeenActivated = false ;
    private float burnTimer ;
    private bool finished ;

    private void Start()
    {
        RocketController.Instance.RegisterRocket(this);

        finished = false ;
    }

    private void FixedUpdate()
    {
        if(move && !hasBeenActivated){
            activateRocket() ;
        }

        if (hasBeenActivated && burnTimer <= burnLength)
        {
            burnTimer += Time.fixedDeltaTime ;
            gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * thrust);
        }
        else
        {
            plumeExhaust.Stop()  ;
            plumeFlame.Stop() ;

            if (hasBeenActivated && !finished)
            {
                firework.Play() ;
                finished = true ;
                FindObjectOfType<AudioManager>().Play("Firework");
            }
        }
        
    }

    public void activateRocket()
    {
        hasBeenActivated = true ;
        finished = false ;
        burnTimer = 0.0f ;

        plumeExhaust.Play() ;
        plumeFlame.Play() ;
    }

    public void moveRocket(bool m){
        move = m;
        // Debug.Log("moved");
        // Debug.Log("moveCar(m): " + move);
    }
    
}
