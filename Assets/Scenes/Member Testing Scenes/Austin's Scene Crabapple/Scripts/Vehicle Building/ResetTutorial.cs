using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTutorial : MonoBehaviour
{
    [SerializeField] GameObject previousButton ;
    [SerializeField] GameObject nextButton ;
    [SerializeField] GameObject firstSlide ;
    
    public void ReadyTutorial()
    {
        previousButton.SetActive(false) ;
        nextButton.SetActive(true) ;
        firstSlide.SetActive(true) ;

        nextButton.GetComponent<EducationalNext>().i = 0 ;
    }
}
