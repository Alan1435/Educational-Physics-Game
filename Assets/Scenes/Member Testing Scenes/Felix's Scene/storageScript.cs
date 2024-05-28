using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageScript : MonoBehaviour
{
    public int amount;
    public GameObject storageType;
    [SerializeField] Button btn;
    [SerializeField] GameObject mouseController;
    
    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(delegate {removePart();}) ;
    }

    // Update is called once per frame
    void Update () 
    {

    }
    public void removePart()
    {
        if(amount > 0 && !mouseController.GetComponent<mouseControlsScript>().isCarrying)
        {
            amount -= 1;
            GameObject newPart = Instantiate(storageType, new Vector3(0,0,0), storageType.transform.rotation);
            newPart.GetComponent<followTheMouse>().followsMouse = true;
            mouseController.GetComponent<mouseControlsScript>().isCarrying = true;
            mouseController.GetComponent<mouseControlsScript>().carriedObject = newPart;
        }
        else
        {
            Debug.Log("no more parts or carrying something");
        }
    }
}
