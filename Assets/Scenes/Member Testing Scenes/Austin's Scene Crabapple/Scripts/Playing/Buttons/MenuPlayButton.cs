using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPlayButton : MonoBehaviour
{
    
    [SerializeField] Button button ;
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject playUI ;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(menuPlayClicked);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void menuPlayClicked()
    {
        FindObjectOfType<AudioManager>().Play("click");
        Time.timeScale = 1;

        pauseMenuUI.SetActive(false);
        playUI.SetActive(true) ;


        Debug.Log("Menu Play button Clicked");
    }
}
