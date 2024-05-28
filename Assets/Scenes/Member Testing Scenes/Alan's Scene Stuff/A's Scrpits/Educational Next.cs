using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EducationalNext: MonoBehaviour
{
    [SerializeField] GameObject[] texts;
    [SerializeField] Button button;
    [SerializeField] GameObject educationalUI;
    [SerializeField] GameObject gameWonUI;
    [SerializeField] GameObject previousButton;
    public int i = 0;

    void Start()
    {
        button.onClick.AddListener(NextEduClicked);
        Debug.Log("1");
        texts[0].SetActive(true);
    }

    private void NextEduClicked()
    {
        Debug.Log("2");
        FindObjectOfType<AudioManager>().Play("page");
        texts[i].SetActive(false);
        if(i+1 == texts.Length){
            educationalUI.SetActive(false);
            gameWonUI.SetActive(true);
        }else{
            if (i+1 > 0) previousButton.SetActive(true);
            
            texts[i+1].SetActive(true);
            i++;
        }

    }
}
