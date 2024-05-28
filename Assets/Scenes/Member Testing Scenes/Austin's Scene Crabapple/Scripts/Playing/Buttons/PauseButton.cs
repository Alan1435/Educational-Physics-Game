using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [SerializeField] Button button ;

    [SerializeField] GameObject menuControl;

    [SerializeField] GameObject playUI ;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(PauseClicked);
    }

    // Execute this when pause is clicked
    private void PauseClicked()
    {
        Time.timeScale = 0;
        FindObjectOfType<AudioManager>().Play("click");

        menuControl.GetComponent<menuControllerScript>().showPauseMenu();

        playUI.SetActive(false) ;

        Debug.Log("pauseClicked");
    }
}
