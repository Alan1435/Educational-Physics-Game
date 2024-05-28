using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PutPartsBack : MonoBehaviour
{
    [SerializeField] Button partSpace;
    [SerializeField] GameObject mouseController;
    // Start is called before the first frame update
    void Start()
    {
        partSpace.onClick.AddListener(delegate {dropPart();}) ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void dropPart()
    {
        if(mouseController.GetComponent<mouseControlsScript>().isCarrying)
        {
            mouseController.GetComponent<mouseControlsScript>().carriedObject.GetComponent<partToStorage>().associatedStorage.GetComponent<StorageScript>().amount += 1;
            Destroy(mouseController.GetComponent<mouseControlsScript>().carriedObject);
            mouseController.GetComponent<mouseControlsScript>().isCarrying = false;
        }
        else
        {
            Debug.Log("Not carrying");
        }
    }
}
