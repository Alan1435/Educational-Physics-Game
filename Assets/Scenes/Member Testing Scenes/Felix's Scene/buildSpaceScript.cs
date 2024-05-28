using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildSpaceScript : MonoBehaviour
{
    [SerializeField] Button btn;
    [SerializeField] GameObject mouseController;
    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(delegate {dropPart();}) ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void dropPart()
    {
        if(mouseController.GetComponent<mouseControlsScript>().isCarrying)
        {
            GameObject tempPart = mouseController.GetComponent<mouseControlsScript>().carriedObject;
            GameObject droppedPart = Instantiate(tempPart, tempPart.transform.position, tempPart.transform.rotation);
            Destroy(mouseController.GetComponent<mouseControlsScript>().carriedObject);

            mouseController.GetComponent<mouseControlsScript>().isCarrying = false;
            droppedPart.GetComponent<followTheMouse>().followsMouse = false;
        }
        else
        {
            Debug.Log("not carrying");
        }
    }
}
