using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuControllerScript : MonoBehaviour
{

    public GameObject pauseMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void hidePauseMenu()
    {
        pauseMenuUI.SetActive(false);
        Debug.Log("Menu hidden");
    }

    public void showPauseMenu()
    {
        pauseMenuUI.SetActive(true);
        Debug.Log("Menu shown");
    }
}
