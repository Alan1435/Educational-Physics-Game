using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewBehaviourScript : MonoBehaviour 
{    
    private bool isDragging = false;
    private Vector3 offset;
    private Rigidbody2D rb;

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody2D>();
        // Make Rigidbody not affect the movement initially
        rb.isKinematic = true;
    }

    void Update()
    {
        if (isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Move the Rigidbody towards the ray hit point
                rb.MovePosition(hit.point + offset);
            }
        }
    }

    void OnMouseDown()
    {
        isDragging = true;
        rb.isKinematic = false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            offset = transform.position - hit.point;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        rb.isKinematic = true;
    }

}
