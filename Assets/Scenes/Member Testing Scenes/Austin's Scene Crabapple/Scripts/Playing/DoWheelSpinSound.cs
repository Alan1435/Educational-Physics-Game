using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoWheelSpinSound : MonoBehaviour
{
    [SerializeField] AudioSource woodWheelSpin1 ;
    [SerializeField] AudioSource woodWheelSpin2 ;

    [SerializeField] float spinOffset1 ;
    [SerializeField] float spinOffset2 ; 
    [SerializeField] float spinRepeatRot ;

    private float spin1RotationPrev = 0 ;
    private float spin2RotationPrev = 0 ;

    private LayerMask terrainMask = (1 << 3) ;

    private Rigidbody2D myRigidBody ;
    private PartProperties myPartProperties ;
    private DoMovementSound doMovementSound ;

    private GameObject mainBlock ;

    private float curRot ;

    void Start()
    {
        myRigidBody = gameObject.GetComponent<Rigidbody2D>() ;
        myPartProperties = gameObject.GetComponent<PartProperties>() ;

        spin1RotationPrev = spinOffset1 * spinRepeatRot ;
        spin2RotationPrev = spinOffset2 * spinRepeatRot ;

        mainBlock = GameObject.Find("Body Starter Variant") ;
        doMovementSound = mainBlock.GetComponent<DoMovementSound>() ;
    }

    // Update is called once per frame
    void Update()
    {
        if (myRigidBody.bodyType == RigidbodyType2D.Dynamic)
        {
            curRot = myRigidBody.rotation ;

            RaycastHit2D hit = Physics2D.BoxCast(gameObject.transform.position, myPartProperties.GetDims(), 0.0f, Vector2.zero, 0.0f, terrainMask) ;

            if (!hit)
            {
                Debug.Log("Wheel hit nothing") ;
                return ;
            }

            doMovementSound.SetTouchingTerrain(true) ;

            if (Mathf.Abs(curRot - spin1RotationPrev) > spinRepeatRot)
            {
                woodWheelSpin1.Play() ;

                spin1RotationPrev = curRot ;

                Debug.Log("Wheel Noise") ;
            }

            if (Mathf.Abs(curRot - spin2RotationPrev) > spinRepeatRot)
            {
                woodWheelSpin2.Play() ;

                spin2RotationPrev = curRot ;

                Debug.Log("Wheel Noise") ;
            }
        }
    }
}
