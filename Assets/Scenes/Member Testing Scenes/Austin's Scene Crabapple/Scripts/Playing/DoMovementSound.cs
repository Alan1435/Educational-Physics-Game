using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoMovementSound : MonoBehaviour
{

    private bool touchingTerrain ;

    [SerializeField] AudioSource terrainSound ;

    [SerializeField] float speedMax ;   // Will set volume to volumeMax is speed >= speedMax
    [SerializeField] float volumeMax ;

    private Rigidbody2D myRigidBody2D ;

    void Start ()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>() ;
    }


    public void SetTouchingTerrain(bool touching)
    {
        Debug.Log("Set touchingTerrain") ;

        if (touching)
        {
            touchingTerrain = touching ;
        }
    }

    // Update is called once per frame
    void Update()
    {
        terrainSound.volume = Mathf.Min(myRigidBody2D.velocity.magnitude / speedMax, 1) * volumeMax ;

        if (Time.timeScale == 0)
        {
            terrainSound.Stop() ;

            terrainSound.loop = false ;
            return ;
        }


        Debug.Log(touchingTerrain) ;

        if (touchingTerrain && terrainSound.loop == false)
        {
            terrainSound.Play() ;

            Debug.Log("Playing grass sound") ;

            terrainSound.loop = true ;
        }
        else if (!touchingTerrain && terrainSound.loop == true)
        {
            terrainSound.loop = false ;

            Debug.Log("Stopping grass sound") ;

            terrainSound.Stop() ;
        }

        touchingTerrain = false ;
    }
}
