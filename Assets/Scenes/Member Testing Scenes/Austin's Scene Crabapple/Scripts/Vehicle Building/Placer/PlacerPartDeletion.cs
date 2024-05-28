using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlacerPartDeletion : MonoBehaviour
{
    // This script should be on the placer only

    [SerializeField] PlacerSounds placerSounds ;
    [SerializeField] PartPlacer partPlacerScript ;
    [SerializeField] GameObject bodyStarter ;
    [SerializeField] float GROUNDED_GIVE ;

    public delegate void PartDeletionHandler() ;
    public event PartDeletionHandler OnPartDelete ;
    public event PartDeletionHandler OnDeletionFinish ;

    private LayerMask bodyLayer ;
    private LayerMask attachmentLayer ;
    private LayerMask allParts ;

    private Vector2 DIM_INCREASE ;

    //Felix Berliner addition, this is to check if the game is out of build phase
    public bool buildPhase;

    // Start is called before the first frame update
    private void Start()
    {
        // Gets layermasks
        bodyLayer = (1 << 6) ;
        attachmentLayer = (1 << 7) ;
        allParts = bodyLayer | attachmentLayer ;

        DIM_INCREASE = new Vector2(GROUNDED_GIVE, GROUNDED_GIVE) ;

        //FB addition
        buildPhase = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && (partPlacerScript.GetPlacerState() == "remove") /*FB add*/ && buildPhase)
        {
            Vector2 mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition) ;
            Collider2D[] hits = Physics2D.OverlapPointAll(mousePosWorld, allParts) ;

            bool foundPart = false ;
            GameObject partToRemove = null ;

            for (int i = 0 ; i < hits.Length && foundPart == false ; i += 1)
            {
                if (hits[i].gameObject != bodyStarter)
                {
                    partToRemove = hits[i].gameObject ;

                    if (hits[i].gameObject.GetComponent<PartProperties>().isAttachmentPart())
                    {
                        foundPart = true ;
                    }
                }
            }

            if (partToRemove != null)
            {
                RemovePart(partToRemove) ;
                placerSounds.PlayPlacerSound("delete") ;
            }
        }
    }

    // Remove a placed part and any pieces that are no longer connected
    private void RemovePart(GameObject part)
    {
        OnPartDelete?.Invoke() ;
        part.GetComponent<PartDeletion>().DeletePart() ;

        Queue<GameObject> groundedQueue = new Queue<GameObject>() ;
        bodyStarter.GetComponent<PartProperties>().SetGrounded(true) ;
        groundedQueue.Enqueue(bodyStarter) ;

        for (int i = 0 ; groundedQueue.Count > 0 && i < 1000 ; i += 1)
        {
            GameObject currentObj = groundedQueue.Dequeue() ;
            PartProperties currentObjProps = currentObj.GetComponent<PartProperties>() ;
            Vector2 currentObjPos = currentObj.transform.position ;
            Vector2 currentObjDims = currentObjProps.GetDims() ;

            RaycastHit2D[] hits = Physics2D.BoxCastAll(currentObjPos, currentObjDims + DIM_INCREASE, 0.0f, Vector2.zero, allParts) ;

            foreach (RaycastHit2D hit in hits)
            {
                GameObject hitObj = hit.collider.gameObject ;
                PartProperties hitProperties = hitObj.GetComponent<PartProperties>() ;
                if (hitObj != currentObj && !hitProperties.GetGrounded())
                {
                    Debug.Log("Cuuuuuuu") ;

                    if (hitProperties.isAttachmentPart())
                    {
                        Debug.Log("Attach") ;

                        if (TestBoxesDontTouchCorners(currentObjPos, (Vector2)hitObj.transform.position + hitProperties.GetPivotPos(), 
                                            currentObjDims, DIM_INCREASE, true))
                        {
                            hitProperties.SetGrounded(true) ;

                            Debug.Log("Attach grounded") ;
                        }
                    }
                    else
                    {
                        Debug.Log("Body") ;

                        if (TestBoxesDontTouchCorners(currentObjPos, (Vector2)hitObj.transform.position, currentObjDims, hitProperties.GetDims(), false))
                        {
                            hitProperties.SetGrounded(true) ;
                            groundedQueue.Enqueue(hitObj) ;

                            Debug.Log("Body grounded") ;
                        }
                    }
                }
            }

            Debug.Log(i) ;
        }

        OnDeletionFinish?.Invoke() ;
    }

    // Returns true if the boxes aren't just touching in the corner
    private bool TestBoxesDontTouchCorners(Vector2 boxPosA, Vector2 boxPosB, Vector2 boxDimsA, Vector2 boxDimsB, bool isAttachment)
    {
        Vector2 posDist = boxPosA - boxPosB ;
        Vector2 dimsAdded = boxDimsA + boxDimsB ;

        bool withinX = Mathf.Abs(posDist.x) < dimsAdded.x / 2.0f ;
        bool withinY = Mathf.Abs(posDist.y) < dimsAdded.y / 2.0f ;

        if (isAttachment)
        {
            return withinX && withinY ;
        }

        // Theres an issue here with attachments
        if (withinX ^ withinY || withinX && withinY)
        {
            return true ;
        }

        return false ;
    }
}
