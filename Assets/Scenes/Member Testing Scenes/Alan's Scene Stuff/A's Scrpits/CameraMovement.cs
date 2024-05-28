using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public bool isMoving;

    [SerializeField] bool parallax;

    [SerializeField] GameObject[] backgrounds ;

    void Start()
    {
        // isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            transform.position = target.position + offset;
        }

        if(parallax){
            foreach (GameObject background in backgrounds)
            {
                background.GetComponent<Parallax>().UpdateParallaxPosition() ;
            }
        }

    }
}
