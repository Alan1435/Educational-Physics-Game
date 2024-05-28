using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartDeletion : MonoBehaviour
{
    // This script should be on the physical parts that get placed only

    private GameObject placer ;
    private GameObject myButton ;
    [SerializeField] PartProperties partPropertiesScript ;
    [SerializeField] string correspondingButtonName ;   // Needs to exactly match the name of the corresponding button

    // Start is called before the first frame update
    void Start()
    {
        placer = GameObject.Find("Placer") ;    // If there's another way to get the particular object in the scene, that would be good
        myButton = GameObject.Find(correspondingButtonName) ;

        placer.GetComponent<PlacerPartDeletion>().OnPartDelete += SetGrounded ;
        placer.GetComponent<PlacerPartDeletion>().OnDeletionFinish += DeleteFromSceneWhenNotGrounded ;
    }

    // Delete this part from the scene if it isn't grounded
    private void DeleteFromSceneWhenNotGrounded()
    {
        if (partPropertiesScript.GetGrounded())
        {
            return ;
        }

        DeletePart() ;
    }

    // Delete this part
    public void DeletePart()
    {
        myButton.GetComponent<PartButton>().ChangePartCount(1) ;
        partPropertiesScript.SetGrounded(true) ;    // It's kind of gross, but this is to prevent the part being counted twice
        Destroy(gameObject) ;
    }

    // Activate when the part is destroyed
    private void OnDestroy()
    {
        if (placer != null)
        {
            placer.GetComponent<PlacerPartDeletion>().OnPartDelete -= SetGrounded ;
            placer.GetComponent<PlacerPartDeletion>().OnDeletionFinish -= DeleteFromSceneWhenNotGrounded ;
        }
    }

    // Set the state of grounded to false, done when a part is removed
    private void SetGrounded()
    {
        partPropertiesScript.SetGrounded(false) ;
    }
}
