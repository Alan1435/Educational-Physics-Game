using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] int level;

    void Start()
    {
        button.onClick.AddListener(NextClicked) ;
    }

    private void NextClicked()
    {
        FindObjectOfType<AudioManager>().Play("click");

        Time.timeScale = 1 ;

        SceneManager.LoadScene(level);
    }
}
