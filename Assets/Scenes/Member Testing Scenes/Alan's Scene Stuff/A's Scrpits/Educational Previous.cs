using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EducationalPrevious : MonoBehaviour
{
    [SerializeField] GameObject[] texts;
    [SerializeField] Button button;
    [SerializeField] EducationalNext nextScript;
    private int i;

    void Start()
    {
        button.onClick.AddListener(PrevEduClicked) ;
    }

    private void PrevEduClicked()
    {
        FindObjectOfType<AudioManager>().Play("page");
        texts[nextScript.i].SetActive(false);
        if(nextScript.i-1 >= 0){
            texts[nextScript.i-1].SetActive(true);
            nextScript.i--;
        }
        if(nextScript.i == 0){
            this.gameObject.SetActive(false);        
        }

    }
}
