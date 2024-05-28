using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelFunction : MonoBehaviour
{
    [SerializeField] float speed;

    [SerializeField] bool move = false;

    private void Start()
    {
        // Register this wheel with the CentralController
        CentralController.Instance.RegisterWheel(this);
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        // Debug.Log(move);
        if(move){
            gameObject.GetComponent<Rigidbody2D>().AddTorque(-1f * speed * Time.fixedDeltaTime) ;
            Debug.Log("Applying Torque: " + (speed * Time.fixedDeltaTime));
            if(!FindObjectOfType<AudioManager>().IsPlaying("BatterySound")){
                FindObjectOfType<AudioManager>().Play("BatterySound");
            }
            
        }else{
            if(FindObjectOfType<AudioManager>().IsPlaying("BatterySound")){
                FindObjectOfType<AudioManager>().Stop("BatterySound");
            }
        }
        
    }

    public void moveCar(bool m){
        move = m;
        // Debug.Log("moved");
        // Debug.Log("moveCar(m): " + move);
    }
    

}
