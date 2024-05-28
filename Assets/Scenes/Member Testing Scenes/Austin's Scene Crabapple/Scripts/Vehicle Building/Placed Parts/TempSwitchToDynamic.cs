using System.Collections;
using System.Collections.Generic;
// using UnityEditor.Timeline;
using UnityEngine;

public class TempSwitchToDynamic : MonoBehaviour
{
    private GameObject playButton ;

    // Poop
    private void Start()
    {
        playButton = GameObject.Find("Play Button") ;

        playButton.GetComponent<PlayButton>().OnPlayClick += SwitchToDynamicRb ;
    }

    private void OnDestroy()
    {
        if (playButton != null)
        {
            playButton.GetComponent<PlayButton>().OnPlayClick -= SwitchToDynamicRb ;
        }
    }

    // Set the Rigidbody2D to dynamic
    private void SwitchToDynamicRb()
    {
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic ;
        // Nope, doesn't work
        // gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.GetComponent<PartProperties>().GetRotation()) ;
    }
}
