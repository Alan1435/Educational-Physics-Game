using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

using Vector2 = UnityEngine.Vector2 ;
using Quaternion = UnityEngine.Quaternion ;

public class EditedPartPlacer : MonoBehaviour
{   
    public GameObject parentObject;
    public List<GameObject> ArrayOfVehicleParts;
    [SerializeField] PartProperties partProperties ;
    [SerializeField] RaycastAndMath raycastAndMath ;

    [SerializeField] GameObject boundary ;
    
    private Button partOrigin ;    // The button that corresponds to the currently selected part
    private bool isHolding = false;
    private GameObject selectedPartPrefab ;
    private PartButton partButtonScript ;

    // Start is called before the first frame update
    private void Start()
    {
        isHolding = false ;
        ClearPartSelection() ;
    }

    // Update is called once per frame
    private void Update()
    {
        //Debug.Log("uuuuuu") ;

        if (!isHolding)
        {
            //Debug.Log("ooohhh") ;
            return ;
        }

        Debug.Log("ooooooo") ;

        // Rotate the placer
        float scroll = Input.GetAxis("Mouse ScrollWheel") ;

        if (Mathf.Abs(scroll) > 0.0f)
        {
            partProperties.ChangeRotation((int)(scroll / Mathf.Abs(scroll)) * 90) ;
            raycastAndMath.UpdateRayDirs(partProperties.GetDims()) ;
        }

        // Snap the placer
        Vector2 newPos = FindPlacerPosition() ;
        transform.position = newPos ;

        // Test to see if we should place the part
        if (Input.GetMouseButtonDown(0) && raycastAndMath.IsPlaceable() && (partButtonScript.GetPartNumber() > 0))
        {
            if (SelectedPartWithinBoundsAtPos(newPos))
            {
                PlacePartAtPos(newPos) ;
            }
        }
    }

    // Instantiate the selected prefab at the placer's transform.position
    private void PlacePartAtPos(Vector2 pos)
    {
        partButtonScript.ChangePartCount(-1) ;

        GameObject newPlacedPart = Instantiate(selectedPartPrefab) ;

        newPlacedPart.transform.position = pos ;
        newPlacedPart.transform.rotation = Quaternion.Euler(0.0f, 0.0f, partProperties.GetRotation()) ;
        newPlacedPart.GetComponent<PartProperties>().SetRotation(partProperties.GetRotation()) ;
        CreateFixedJoint(newPlacedPart);
        ArrayOfVehicleParts.Add(newPlacedPart);
    }

    // Return true if the selected part is within the bounds, otherwise false
    private bool SelectedPartWithinBoundsAtPos(Vector2 pos)
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
        Debug.Log("nnnoooo") ;
        partButtonScript = partOrigin.GetComponent<PartButton>() ;

        SetPlacerProperties(selectedPartPrefab) ;
    }

    // Sets partOrigin to null and isHolding to false; clears the selection
    private void ClearPartSelection()
    {
        partOrigin = null ;
        isHolding = false ;

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
    private void CreateFixedJoint(GameObject childObject){
        if (childObject.CompareTag("Body")) {
            FixedJoint2D fixedJoint = childObject.AddComponent<FixedJoint2D>();
            UnityEngine.Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 anchorPosition = mouseWorldPosition - parentObject.transform.position;
            fixedJoint.connectedBody = parentObject.GetComponent<Rigidbody2D>();
            fixedJoint.anchor = anchorPosition;
        }
    }

}

