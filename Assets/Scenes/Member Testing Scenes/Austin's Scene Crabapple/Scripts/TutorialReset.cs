using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialReset : MonoBehaviour
{
    [SerializeField] string[] playerPrefs ;

    // Start is called before the first frame update
    void Start()
    {
        foreach (string prefName in playerPrefs)
        {
            PlayerPrefs.SetInt(prefName, 0) ;
        }
    }
}
