using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

using Vector2 = UnityEngine.Vector2 ;
using Quaternion = UnityEngine.Quaternion ;
using JetBrains.Annotations;

public class PartProperties : MonoBehaviour
{
    [SerializeField] float myWidth ;    // The width and height that are used for snapping
    [SerializeField] float myHeight ;
    [SerializeField] float pivotX ;   // The pivot position for the attachment
    [SerializeField] float pivotY ;
    [SerializeField] int myRotation ;   // Only in increments of 90. Stored in degrees
    [SerializeField] bool isAttachment ;

    private bool isGrounded = true ;   // Is used for part removal with the vehicle builder

    // Set the value of isGrounded to value
    public void SetGrounded(bool value)
    {
        isGrounded = value ;
    }

    // Return the value of isGrounded
    public bool GetGrounded()
    {
        return isGrounded ;
    }

    // Return the value of isAttachment
    public bool isAttachmentPart()
    {
        return isAttachment ;
    }

    // Set the value of isAttachment
    public void SetAttachmentBool(bool value)
    {
        isAttachment = value ;
    }

    // Returns the game world width and height of the object
    // ie. with myRotation = 90, returns w and h flipped
    public Vector2 GetDims()
    {
        if (myRotation % 180 == 0)
        {
            return new Vector2(myWidth, myHeight) ;
        }

        return new Vector2(myHeight, myWidth) ;
    }

    // Returns the pivot point for the raycasty detecty stuff
    public Vector2 GetPivotPos()
    {
        Vector2 relativePivot ; 

        if (myRotation % 180 == 0)
        {
            relativePivot = new Vector2(pivotX, pivotY) ;
        }
        else
        {
            relativePivot = new Vector2(pivotY, pivotX) ;
        }

        if (myRotation % 360 < 180)
        {
            return relativePivot ;
        }

        return relativePivot * -1.0f ;
    }

    // Returns the rotation
    public int GetRotation()
    {
        return myRotation ;
    }

    // Sets the rotation to angle
    public void SetRotation(int angle)
    {
        myRotation = angle ;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, myRotation) ;
    }

    // Changes the rotation by angle
    public void ChangeRotation(int angle)
    {
        myRotation += angle ;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, myRotation) ;
    }

    // Sets the dimensions
    public void SetDims(float newWidth, float newHeight)
    {
        myWidth = newWidth ;
        myHeight = newHeight ;
    }

    // Sets the ray dimensions
    public void SetPivotPos(float newX, float newY)
    {
        pivotX = newX ;
        pivotY = newY ;
    }
}
