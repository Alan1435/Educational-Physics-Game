using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

using Vector2 = UnityEngine.Vector2 ;

public class RaycastAndMath : MonoBehaviour
{
    [SerializeField] float PLACEMENT_GIVE ;
    [SerializeField] float ATTACHMENT_CONNECTION_R ;
    [SerializeField] PartProperties partProperties ;
    [SerializeField] float raycastSpacing ; // Should be small enough so that no parts can fit between the gaps

    private LayerMask bodyLayer ;
    private LayerMask attachmentLayer ;
    private LayerMask allParts ;

    List<Vector2> raycastDirs ;
    Vector2 myDims ;
    Vector2 myPivot ;
    private bool placeable ;
    private bool nearValid ;

    // Start is called once before the first frame update
    private void Start()
    {
        // Gets layermasks
        bodyLayer = (1 << 6) ;
        attachmentLayer = (1 << 7) ;
        allParts = bodyLayer | attachmentLayer ;
    }

    // Returns the value of placeable
    public bool IsPlaceable()
    {
        return placeable ;
    }

    // Returns the value of nearValid, which should be true if the part being placed intersects a placed part, false otherwise
    public bool IsNearValid()
    {
        return nearValid ;
    }

    // Will return the final snap position of the object and set placeable
    public Vector2 SnapAtPos(Vector2 pos)
    {
        if (partProperties.isAttachmentPart())
        {
            return AttachmentSnapAtPos(pos) ;
        }

        return BodySnapAtPos(pos) ;
    }

    // Returns the snap for an attachment part
    private Vector2 AttachmentSnapAtPos(Vector2 pos)
    {
        myDims = partProperties.GetDims() ;
        myPivot = partProperties.GetPivotPos() ;
        Vector2 snapPos = TryAttachmentSnapAtPos(pos) ;
        // snapPos = TryAttachmentSnapAtPos(snapPos) ;

        if (!Physics2D.BoxCast(snapPos + myPivot, new Vector2(ATTACHMENT_CONNECTION_R, ATTACHMENT_CONNECTION_R), 0.0f, Vector2.zero, 0.0f))    // Test to see if the part at the snap position is inside of something
        {
            Debug.Log("Pivot not inside body") ;

            placeable = false ;
            return pos ;
        }

        if (Physics2D.BoxCast(snapPos, (myDims - new Vector2(PLACEMENT_GIVE, PLACEMENT_GIVE)), 0.0f, Vector2.zero, 0.0f, attachmentLayer))    // Test to see if the part at the snap position is inside of something
        {
            Debug.Log("Attachment overlapping attachment") ;
            
            placeable = false ;
            return pos ;
        }

        return snapPos ;
    }

    // Returns the snap for a body part
    private Vector2 BodySnapAtPos(Vector2 pos)
    {
        myDims = partProperties.GetDims() ;
        Vector2 snapPos = TryBodySnapAtPos(pos) ;
        Vector2 boxcastDims = myDims - new Vector2(PLACEMENT_GIVE, PLACEMENT_GIVE) ;

        // Do this instead of what I had on Scratch where you do the snapping twice
        if (Physics2D.BoxCast(snapPos, boxcastDims, 0.0f, Vector2.zero, 0.0f, bodyLayer))    // Test to see if the part at the snap position is inside of something
        {
            placeable = false ;
            return pos ;
        }

        // Do a boxcast that can only hit attachments.
        // Loop through all attachments it hit and check if their pivot is ever inside of ourself.
        // If it ever is, then abort the placement. Otherwise, continue on.
        RaycastHit2D[] attachmentHits = Physics2D.BoxCastAll(snapPos, boxcastDims, 0.0f, Vector2.zero, 0.0f, attachmentLayer) ;
        foreach (RaycastHit2D hit in attachmentHits)
        {
            Vector2 hitPivotPos = hit.collider.GetComponent<PartProperties>().GetPivotPos() + (Vector2)hit.transform.position;
            if ((Mathf.Abs(hitPivotPos.x - snapPos.x) < (myDims.x / 2.0f) + PLACEMENT_GIVE) && (Mathf.Abs(hitPivotPos.y - snapPos.y) < (myDims.y / 2.0f) + PLACEMENT_GIVE))
            {
                placeable = false ;
                return pos ;
            }
        }

        
        return snapPos ;
    }

    // Tries to do an attachment snap at a given position
    private Vector2 TryAttachmentSnapAtPos(Vector2 pos)
    {
        int rayDirsLength ;
        bool collided ;
        Vector2 curRayDir ;
        RaycastHit2D raycastHit ;
        rayDirsLength = raycastDirs.Count ;
        List<float>[] edgeSnaps = new List<float>[4] ;
        float[] snapPoints = new float[4] ;
        Vector2 snapPos ;

        nearValid = false ;

        for (int i = 0 ; i < 4 ; i++)   // Initialize all of the lists
        {
            edgeSnaps[i] = new List<float>() ;
        }

        if (Physics2D.OverlapPoint(pos) != null)    // Test to see if the position is literally inside of something already
        {
            placeable = false ;
            nearValid = true ;
            return pos ;
        }

        collided = false ;

        for (int i = 0 ; i < rayDirsLength ; i++)   // Perform all of the raycasts
        {
            curRayDir = raycastDirs[i] ;
            raycastHit = Physics2D.Raycast(pos, curRayDir, curRayDir.magnitude, bodyLayer) ;

            if (raycastHit)
            {
                collided = true ;
                edgeSnaps = GetEdgeSnapValues(raycastHit.collider.gameObject, CalculateRayFacing(curRayDir), pos, edgeSnaps) ;
            }
        }

        if (!collided)  // If nothing was hit, exit and don't place
        {
            placeable = false ;
            return pos ;
        }

        // We hit something, so set nearValid to true
        nearValid = true ;

        snapPoints = GetSnapPoints(edgeSnaps) ;
        // Debug.Log(String.Format("{0}, {1}, {2}, {3}", snapPoints[0], snapPoints[1], snapPoints[2], snapPoints[3])) ;

        if (Mathf.Abs(snapPoints[3] - pos.x) > Mathf.Abs(snapPoints[1] - pos.x))    // Figure out if pos is closer to the left or right snap and set snapX to the closer one
        {
            snapPos.x = snapPoints[1] ;
        }
        else
        {
            snapPos.x = snapPoints[3] ;
        }

        if (Mathf.Abs(snapPoints[2] - pos.y) > Mathf.Abs(snapPoints[0] - pos.y))    // Figure out if pos is closer to the up and down snap and set snapY to the closer one
        {
            snapPos.y = snapPoints[0] ;
        }
        else
        {
            snapPos.y = snapPoints[2] ;
        }

        if (Mathf.Abs(snapPos.y) == 1000000.0f)
        {
            snapPos.y = pos.y ;
        }

        if (Mathf.Abs(snapPos.x) == 1000000.0f)
        {
            snapPos.x = pos.x ;
        }

        // if (Mathf.Abs(snapPoints[1] - snapPoints[3]) < myDims.x - PLACEMENT_GIVE)   // If it can't fit horizontally
        // {
        //     placeable = false ;
        //     return snapPos ;
        // }

        // if (Mathf.Abs(snapPoints[0] - snapPoints[2]) < myDims.y - PLACEMENT_GIVE)   // If it can't fit vertically
        // {
        //     placeable = false ;
        //     return snapPos ;
        // }

        Debug.Log("Attachment placeable") ;

        placeable = true ;
        return snapPos - partProperties.GetPivotPos() ;
    }

    // Tries to do a body snap at pos and returns where it thinks it should snap to, also sets placeable
    private Vector2 TryBodySnapAtPos(Vector2 pos)
    {
        int rayDirsLength ;
        bool collided ;
        Vector2 curRayDir ;
        RaycastHit2D raycastHit ;
        rayDirsLength = raycastDirs.Count ;
        List<float>[] edgeSnaps = new List<float>[4] ;
        float[] snapPoints = new float[4] ;
        Vector2 snapPos ;

        nearValid = false ;

        for (int i = 0 ; i < 4 ; i++)   // Initialize all of the lists
        {
            edgeSnaps[i] = new List<float>() ;
        }

        if (Physics2D.OverlapPoint(pos) != null)    // Test to see if the position is literally inside of something already
        {
            placeable = false ;
            nearValid = true ;
            return pos ;
        }

        collided = false ;

        for (int i = 0 ; i < rayDirsLength ; i++)   // Perform all of the raycasts
        {
            curRayDir = raycastDirs[i] ;
            raycastHit = Physics2D.Raycast(pos, curRayDir, curRayDir.magnitude, bodyLayer) ;

            if (raycastHit)
            {
                collided = true ;
                edgeSnaps = GetEdgeSnapValues(raycastHit.collider.gameObject, CalculateRayFacing(curRayDir), pos, edgeSnaps) ;
            }
        }

        if (!collided)  // If nothing was hit, exit and don't place
        {
            placeable = false ;
            return pos ;
        }

        // We're inside of something or at least touching it
        nearValid = true ;

        snapPoints = GetSnapPoints(edgeSnaps) ;
        // Debug.Log(String.Format("{0}, {1}, {2}, {3}", snapPoints[0], snapPoints[1], snapPoints[2], snapPoints[3])) ;

        if (Mathf.Abs(snapPoints[3] - pos.x) > Mathf.Abs(snapPoints[1] - pos.x))    // Figure out if pos is closer to the left or right snap and set snapX to the closer one
        {
            snapPos.x = snapPoints[1] ;
        }
        else
        {
            snapPos.x = snapPoints[3] ;
        }

        if (Mathf.Abs(snapPoints[2] - pos.y) > Mathf.Abs(snapPoints[0] - pos.y))    // Figure out if pos is closer to the up and down snap and set snapY to the closer one
        {
            snapPos.y = snapPoints[0] ;
        }
        else
        {
            snapPos.y = snapPoints[2] ;
        }

        if (Mathf.Abs(snapPos.y) == 1000000.0f)
        {
            snapPos.y = pos.y ;
        }

        if (Mathf.Abs(snapPos.x) == 1000000.0f)
        {
            snapPos.x = pos.x ;
        }

        if (Mathf.Abs(snapPoints[1] - snapPoints[3]) < myDims.x - PLACEMENT_GIVE)   // If it can't fit horizontally
        {
            placeable = false ;
            return snapPos ;
        }

        if (Mathf.Abs(snapPoints[0] - snapPoints[2]) < myDims.y - PLACEMENT_GIVE)   // If it can't fit vertically
        {
            placeable = false ;
            return snapPos ;
        }

        placeable = true ;
        return snapPos ;
    }

    // Get the maximum value in snapUp and snapRight, minimum in snapDown and snapLeft
    private float[] GetSnapPoints(List<float>[] edgeSnaps)
    {
        float snapUp, snapRight, snapDown, snapLeft ;

        try     // Get the snap value for snapUp
        {
            snapUp = edgeSnaps[0].Max() ;
            // Debug.Log("got max of snapUp") ;
        }
        catch (InvalidOperationException)
        {
            snapUp = -1000000.0f ;
        }

        try     // Get the snap value for snapRight
        {
            snapRight = edgeSnaps[1].Max() ;
            // Debug.Log("got max of snapRight") ;
        }
        catch (InvalidOperationException)
        {
            snapRight = -1000000.0f ;
        }

        try     // Get the snap value for snapDown
        {
            snapDown = edgeSnaps[2].Min() ;
            // Debug.Log("got min of snapDown") ;
        }
        catch (InvalidOperationException)
        {
            snapDown = 1000000.0f ;
        }

        try     // Get the snap value for snapLeft
        {
            snapLeft = edgeSnaps[3].Min() ;
            // Debug.Log("got min of snapLeft") ;
        }
        catch (InvalidOperationException)
        {
            snapLeft = 1000000.0f ;
        }

        float[] snapPoints = {snapUp, snapRight, snapDown, snapLeft} ;
        return snapPoints ;
    }

    // Basically, if the x or y component is 0, keep them 0, otherwise, set to 1 but keep same sign
    private Vector2 CalculateRayFacing(Vector2 rayDir)
    {
        if (rayDir.x * rayDir.y == 0)
        {
            if (rayDir.x == 0)
            {
                return new Vector2(rayDir.x, rayDir.y / Mathf.Abs(rayDir.y)) ;
            }
            else
            {
                return new Vector2(rayDir.x / Mathf.Abs(rayDir.x), rayDir.y) ;
            }
        }

        return new Vector2(rayDir.x / Mathf.Abs(rayDir.x), rayDir.y / Mathf.Abs(rayDir.y)) ;
    }

    // Returns a list of all the positions things want you to snap to
    private List<float>[] GetEdgeSnapValues(GameObject placed, Vector2 rayDir, Vector2 myPos, List<float>[] edgeSnaps)
    {
        PartProperties placedProperties = placed.GetComponent<PartProperties>() ;

        Vector2 placedPos = placed.transform.position ;
        Vector2 placedDims = placedProperties.GetDims() ;
        Vector2 snapDims ;

        // If it's an attachment, set the dimensions to 0 because we're, y'know, an attachment and want to snap to the edge
        // Otherwise, get the dimensions of the object
        if (partProperties.isAttachmentPart())
        {
            snapDims = Vector2.zero ;
        }
        else
        {
            snapDims = partProperties.GetDims() ;
        }

        Vector2 placedCorner ;

        // If the ray is just going in a cardinal direction, no extra computation necessary.
        // However, it may be possible to condense the code by removing the section. I'm not sure.
        if (rayDir.x * rayDir.y == 0)
        {
            if (rayDir.x == 0)
            {
                if (rayDir.y > 0)
                {
                    edgeSnaps[2].Add(placedPos.y - ((placedDims.y + snapDims.y) / 2.0f)) ;
                }
                else
                {
                    edgeSnaps[0].Add(placedPos.y + ((placedDims.y + snapDims.y) / 2.0f)) ;
                }
            }
            else
            {
                if (rayDir.x > 0)
                {
                    edgeSnaps[3].Add(placedPos.x - ((placedDims.x + snapDims.x) / 2.0f)) ;
                }
                else
                {
                    edgeSnaps[1].Add(placedPos.x + ((placedDims.x + snapDims.x) / 2.0f)) ;
                }
            }

            return edgeSnaps ;
        }
        
        // This is used to tell if we hit a horizontal or vertical edge.
        placedCorner.x = placedPos.x - (((placedDims.x + snapDims.x) / 2.0f) * rayDir.x) ;
        placedCorner.y = placedPos.y - (((placedDims.y + snapDims.y) / 2.0f) * rayDir.y) ;

        if (Mathf.Abs(placedCorner.x - myPos.x) / myDims.x > Mathf.Abs(placedCorner.y - myPos.y) / myDims.y)
        {
            if (rayDir.y > 0)
            {
                edgeSnaps[2].Add(placedPos.y - ((placedDims.y + snapDims.y) / 2.0f)) ;
            }
            else
            {
                edgeSnaps[0].Add(placedPos.y + ((placedDims.y + snapDims.y) / 2.0f)) ;
            }
        }
        else
        {
            if (rayDir.x > 0)
            {
                edgeSnaps[3].Add(placedPos.x - ((placedDims.x + snapDims.x) / 2.0f)) ;
            }
            else
            {
                edgeSnaps[1].Add(placedPos.x + ((placedDims.x + snapDims.x) / 2.0f)) ;
            }
        }

        return edgeSnaps ;
    }

    // Update the raycastDirs list
    public void UpdateRayDirs(Vector2 partDim)
    {
        raycastDirs = GetRayDirs(partDim) ;
    }

    // Return a bunch of points along the edge of a rectange of given height and width
    private List<Vector2> GetRayDirs(Vector2 partDim)
    {
        List<Vector2> rayDirTemp = new List<Vector2>() ;

        for (float incrX = -partDim.x / 2.0f ; incrX < partDim.x / 2.0f ; incrX += raycastSpacing)  // Add points along the top and bottom edge
        {
            rayDirTemp.Add(new Vector2(incrX, partDim.y / 2.0f)) ;
            rayDirTemp.Add(new Vector2(-incrX, -partDim.y / 2.0f)) ;
        }

        for (float incrY = -partDim.y / 2.0f ; incrY < partDim.y / 2.0f ; incrY += raycastSpacing)  // Add points along the left and right edge
        {
            rayDirTemp.Add(new Vector2(partDim.x / 2.0f, -incrY)) ;
            rayDirTemp.Add(new Vector2(-partDim.x / 2.0f, incrY)) ;
        }

        // Add points in the four cardinal directions. Note, may cause duplicates
        rayDirTemp.Add(new Vector2(partDim.x / 2.0f, 0.0f)) ;
        rayDirTemp.Add(new Vector2(-partDim.x / 2.0f, 0.0f)) ;
        rayDirTemp.Add(new Vector2(0.0f, partDim.y / 2.0f)) ;
        rayDirTemp.Add(new Vector2(0.0f, -partDim.y / 2.0f)) ;

        return rayDirTemp ;
    } 
}