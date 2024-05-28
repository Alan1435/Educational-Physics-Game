using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    [SerializeField] Button button ;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(RestartClicked) ;
    }

    private void RestartClicked()
    {
        FindObjectOfType<AudioManager>().Play("click");

        Time.timeScale = 1 ;

        string currentScene = SceneManager.GetActiveScene().name ;
        SceneManager.LoadScene(currentScene) ;
    }
}
