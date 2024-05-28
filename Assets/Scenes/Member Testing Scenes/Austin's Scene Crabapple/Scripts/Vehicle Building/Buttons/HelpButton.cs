using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;

public class HelpButton : MonoBehaviour
{
    [SerializeField] GameObject tutorialUI ;

    [SerializeField] Button helpButton ;

    [SerializeField] string tutorial ;

    // Start is called before the first frame update
    void Start()
    {
        helpButton.onClick.AddListener(OnHelp) ;

        if (PlayerPrefs.GetInt(tutorial) != 0)  // True when tutorial is equal to 1, which means it's already been shown
        {
            return ;
        }

        PlayerPrefs.SetInt(tutorial, 1) ;

        tutorialUI.SetActive(true) ;
        tutorialUI.GetComponent<ResetTutorial>().ReadyTutorial() ;
    }

    private void OnHelp()
    {
        FindObjectOfType<AudioManager>().Play("click") ;

        tutorialUI.SetActive(true) ;
        tutorialUI.GetComponent<ResetTutorial>().ReadyTutorial() ;
    }
}
