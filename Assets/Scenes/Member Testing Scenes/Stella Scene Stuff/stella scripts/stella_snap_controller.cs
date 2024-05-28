using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stella_snap_controller : MonoBehaviour
{

    public List<Transform> snapPoints;
    public List<stella_draggable > draggableObjects;
    public float snapRange = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        foreach(stella_draggable draggable in draggableObjects)
        {   
            // I commented this out so that I could code something myself because this was throwing compiler errors
            // draggable.dragEndedCallback = OnDragEnded;
        }
    }

    private void OnDragEnded(stella_draggable draggable)
    {
        float closestDistance = -1;
        Transform closestSnapPoint = null;

        foreach(Transform snapPoint in snapPoints)
        {
            float currentDistance = Vector2.Distance(draggable.transform.localPosition, snapPoint.localPosition);
            if (closestSnapPoint == null || currentDistance < closestDistance)
            {
                closestSnapPoint = snapPoint;
                closestDistance = currentDistance;
            }

        }

        if(closestSnapPoint != null && closestDistance <= snapRange)
        {
            draggable.transform.localPosition = closestSnapPoint.localPosition;
        }
    }

}
