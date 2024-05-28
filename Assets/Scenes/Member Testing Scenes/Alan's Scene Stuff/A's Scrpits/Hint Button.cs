using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;

public class HintButton : MonoBehaviour
{
    [SerializeField] GameObject HintUI ;
    [SerializeField] Button hintButton ;

    // Start is called before the first frame update
    void Start()
    {
        hintButton.onClick.AddListener(OnHint) ;
    }

    private void OnHint()
    {
        FindObjectOfType<AudioManager>().Play("click") ;
        HintUI.SetActive(true) ;
        HintUI.GetComponent<ResetTutorial>().ReadyTutorial() ;
    }
}


