using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPartPlaced : MonoBehaviour
{
    [SerializeField] GameObject attachedStorage;
    [SerializeField] GameObject button;
    public int initalpartNumber;

    void Update()
    {
        Debug.Log(attachedStorage.GetComponent<PartButton>().GetPartNumber());
        if(attachedStorage.GetComponent<PartButton>().GetPartNumber() == initalpartNumber){
            button.SetActive(false);
        }
    }
}
