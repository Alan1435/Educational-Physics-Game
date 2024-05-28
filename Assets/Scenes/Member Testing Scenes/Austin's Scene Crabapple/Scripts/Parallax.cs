using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] Vector2 positionOffset ;
    [SerializeField] float xSpeed ;
    [SerializeField] float ySpeed ;

    // Update is called once per frame
    public void UpdateParallaxPosition()
    {
        // Doesn't really work well. Has a shit load of stuttering for some reason
        transform.position = positionOffset - new Vector2(Camera.main.transform.position.x * xSpeed, Camera.main.transform.position.y * ySpeed) ;
        // GetComponent<Renderer>().material.mainTextureOffset = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y) ;
    }
}
