using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

using Vector2 = UnityEngine.Vector2 ;
using Vector3 = UnityEngine.Vector3 ;
using Quaternion = UnityEngine.Quaternion ;
using Unity.VisualScripting;

public class PartPlacer : MonoBehaviour
{
    [SerializeField] GameObject parentObject ;
    [SerializeField] PartProperties partProperties ;
    [SerializeField] RaycastAndMath raycastAndMath ;
    [SerializeField] PlacerSounds placerSounds ;

    [SerializeField] Transform vehicleEmptyParentTransform ;

    [SerializeField] GameObject boundary ;

    [SerializeField] float dampingRatio = 0.2f;
    [SerializeField] float frequency = 3.0f; 
    [SerializeField] float angle = 90.0f;
    [SerializeField] float correctionScale = 0.05f;
    [SerializeField] float maxForce = 1000.0f; 
    [SerializeField] float maxTorque = 1000.0f;
    
    private Button partOrigin ;    // The button that corresponds to the currently selected part
    private bool isHolding ;
    private GameObject selectedPartPrefab ;
    private PartButton partButtonScript ;
    private string placerState = "remove" ; // Currently, should only ever be in remove or build

    // Start is called before the first frame update
    private void Start()
    {
        isHolding = false ;
        ClearPartSelection() ;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isHolding)
        {
            return ;
        }

        // Rotate the placer
        float scroll = Input.GetAxis("Mouse ScrollWheel") ;

        if (Mathf.Abs(scroll) > 0.0f)
        {
            partProperties.ChangeRotation((int)(scroll / Mathf.Abs(scroll)) * 90) ;
            raycastAndMath.UpdateRayDirs(partProperties.GetDims()) ;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            partProperties.ChangeRotation(90) ;
            raycastAndMath.UpdateRayDirs(partProperties.GetDims()) ;
        }

        // Snap the placer
        Vector2 newPos = FindPlacerPosition() ;
        transform.position = newPos ;

        // Test to see if we should place the part
        if (Input.GetMouseButtonDown(0) && (placerState == "build"))
        {
            if (raycastAndMath.IsPlaceable() && (partButtonScript.GetPartNumber() > 0) && SelectedPartWithinBoundsAtPos(newPos))
            {
                    PlacePartAtPos(newPos) ;
                    placerSounds.PlayPlacerSound("place") ;
            }
            else if (raycastAndMath.IsNearValid())
            {
                placerSounds.PlayPlacerSound("noplace") ;
            }
        }
    }

    // Instantiate the selected prefab at the placer's transform.position
    private void PlacePartAtPos(Vector2 pos)
    {
        partButtonScript.ChangePartCount(-1) ;

        GameObject newPlacedPart = Instantiate(selectedPartPrefab, vehicleEmptyParentTransform) ;

        Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, partProperties.GetRotation()) ;

        newPlacedPart.transform.position = pos ;
        newPlacedPart.transform.rotation = rotation ;
        newPlacedPart.GetComponent<PartProperties>().SetRotation(partProperties.GetRotation()) ;
        Physics2D.SyncTransforms() ;    // When we set the rotation, it doesn't update the rigidbody until the next physics update
                                        // The fixed joint remebers what the relative rotation was and constrains it to that
                                        // So we call this right here so that when we make the joint, the rotation is already synced
        CreateFixedJoint(newPlacedPart, pos) ;
    }

    // Return true if the selected part is within the bounds, otherwise false
    public bool SelectedPartWithinBoundsAtPos(Vector2 pos)
    {
        BoundaryProperties boundProperties = boundary.GetComponent<BoundaryProperties>() ;
        Vector2 boundaryCenter = boundProperties.GetBoundCenter() ;
        Vector2 boundaryDims = boundProperties.GetBoundDims() ;
        Vector2 partDims = partProperties.GetDims() ;

        Vector2 relPos = pos - boundaryCenter ;

        if (Mathf.Abs(relPos.x) + (partDims.x / 2.0f) > boundaryDims.x / 2.0f)
        {
            return false ;
        }

        if (Mathf.Abs(relPos.y) + (partDims.y / 2.0f) > boundaryDims.y / 2.0f)
        {
            return false ;
        }

        return true ;
    }

    // Position the placer
    private Vector2 FindPlacerPosition()
    {
        Vector2 mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition) ;
        Vector2 snapPos = raycastAndMath.SnapAtPos(mousePosWorld) ;

        Debug.Log(raycastAndMath.IsPlaceable()) ;

        return snapPos ;
    }

    // Sets the placer from a button and the corresponding prefab. If this button is selected, you clear the selection.
    public void SetPlacerFromButton(Button newOrigin, GameObject partPrefab)
    {
        if (newOrigin == partOrigin)
        {
            ClearPartSelection() ;
            return ;
        }

        SetPartOrigin(newOrigin, partPrefab) ;
        raycastAndMath.UpdateRayDirs(partProperties.GetDims()) ;
    }

    // Sets the value of partOrigin to newOrigin and sets isHolding to true
    private void SetPartOrigin(Button newOrigin, GameObject partPrefab)
    {
        partOrigin = newOrigin ;
        selectedPartPrefab = partPrefab ;
        isHolding = true ;
        partButtonScript = partOrigin.GetComponent<PartButton>() ;
        placerState = "build" ;

        SetPlacerProperties(selectedPartPrefab) ;
    }

    // Sets partOrigin to null and isHolding to false; clears the selection
    public void ClearPartSelection()
    {
        partOrigin = null ;
        isHolding = false ;
        placerState = "remove" ;

        GetComponent<SpriteRenderer>().sprite = null ;
    }

    // Set the dimensions of the placer, the rotation, and the sprite
    private void SetPlacerProperties(GameObject partPrefab)
    {
        Sprite partSprite = partPrefab.GetComponent<SpriteRenderer>().sprite ;
        PartProperties prefabProperties = partPrefab.GetComponent<PartProperties>() ;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>() ;
        spriteRenderer.sprite = partSprite ;

        Vector2 prefabDims = prefabProperties.GetDims() ;
        Vector2 prefabPivotPos = prefabProperties.GetPivotPos() ;
        partProperties.SetDims(prefabDims.x, prefabDims.y) ;
        partProperties.SetPivotPos(prefabPivotPos.x, prefabPivotPos.y) ;
        partProperties.SetRotation(0) ;
        partProperties.SetAttachmentBool(prefabProperties.isAttachmentPart()) ;
    }

    // Returns the value of placerState
    public string GetPlacerState()
    {
        return placerState ;
    }

    //FB addition - gets the current partButtonScript
    public PartButton getPartButtonScript()
    {
        return partButtonScript;
    }

    //FB addition - gets the state of isHolding
    public bool getIsHolding()
    {
        return isHolding;
    }

    // 
    private void CreateFixedJoint(GameObject childObject, Vector2 pos)
    {
        if (childObject.CompareTag("Wheel")) 
        {
            WheelJoint2D wheelJoint = parentObject.AddComponent<WheelJoint2D>();
            Vector2 anchorPosition = pos - (Vector2)parentObject.transform.position;
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

            return ;
        }

        if (childObject.CompareTag("Body")) 
        {
            RelativeJoint2D relativeJoint = childObject.AddComponent<RelativeJoint2D>() ;
            // fixedJoint.autoConfigureConnectedAnchor = false ;
            relativeJoint.autoConfigureOffset = true ;
            //Vector2 anchorPosition = pos;
            relativeJoint.connectedBody = parentObject.GetComponent<Rigidbody2D>() ;
            // fixedJoint.anchor = Vector2.zero ;
            // fixedJoint.connectedAnchor = anchorPosition ;
            relativeJoint.correctionScale = correctionScale ;
            relativeJoint.maxForce = maxForce ;
            relativeJoint.maxTorque = maxTorque ;

            return ; 
        }
    }
}