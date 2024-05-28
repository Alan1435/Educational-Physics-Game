using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParent : MonoBehaviour
{
    public GameObject parentObject;
    public GameObject childObject;
    public float dampingRatio = 0.2f;
    public float frequency = 3.0f; 
    public float angle = 90.0f;
    private bool wheelJointAdded = false;
    private bool fixedJointAdded = false;

    void Start()
    {
        // if (childObject.CompareTag("Body")) 
        // {
        //     FixedJoint2D fixedJoint = parentObject.AddComponent<FixedJoint2D>();
        //     Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     Vector2 anchorPosition = mouseWorldPosition - parentObject.transform.position;
        //     fixedJoint.connectedBody = childObject.GetComponent<Rigidbody2D>();
        //     fixedJoint.anchor = anchorPosition;
        // }
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && childObject.CompareTag("Wheel") && !wheelJointAdded) 
        {
            WheelJoint2D wheelJoint = parentObject.AddComponent<WheelJoint2D>();
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 anchorPosition = mouseWorldPosition - parentObject.transform.position;
            wheelJoint.connectedBody = childObject.GetComponent<Rigidbody2D>();
            wheelJoint.anchor = anchorPosition;
            // Get the current suspension settings
            JointSuspension2D suspension = wheelJoint.suspension;
            suspension.dampingRatio = dampingRatio;
            suspension.frequency = frequency;
            // Note: The angle property expects radians, so convert degrees to radians
            suspension.angle = angle;
            // Apply the modified suspension settings back to the wheel joint
            wheelJoint.suspension = suspension;
            wheelJointAdded = true;
        }

        if (Input.GetMouseButtonDown(0) && childObject.CompareTag("Body") && !fixedJointAdded) 
        {
            FixedJoint2D fixedJoint = parentObject.AddComponent<FixedJoint2D>();
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 anchorPosition = mouseWorldPosition - parentObject.transform.position;
            fixedJoint.connectedBody = childObject.GetComponent<Rigidbody2D>();
            fixedJoint.anchor = anchorPosition;
            fixedJointAdded = true;
        }
    }
}
