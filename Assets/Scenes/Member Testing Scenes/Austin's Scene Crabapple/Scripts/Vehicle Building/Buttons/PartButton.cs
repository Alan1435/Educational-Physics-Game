using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;

public class PartButton : MonoBehaviour
{
    [SerializeField] Button button ;
    [SerializeField] GameObject placer ;
    [SerializeField] GameObject partPrefab ;

    [SerializeField] int partCount ;

    private PartPlacer partPlacerScript ;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(SetPlacer) ;
        partPlacerScript = placer.GetComponent<PartPlacer>() ;
    }

    // Set the part placer stuff, called when the button is clicked
    private void SetPlacer()
    {
        Debug.Log("Clicked") ;

        partPlacerScript.SetPlacerFromButton(button, partPrefab) ;
    }

    // Decrease the part count by 1
    public void ChangePartCount(int amount)
    {
        partCount += amount ;
    }

    // Return the current partCount
    public int GetPartNumber()
    {
        return partCount ;
    }
}
