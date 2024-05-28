using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevel : MonoBehaviour
{
    [SerializeField] Button button ;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(NextClicked) ;
    }

    private void NextClicked()
    {
        FindObjectOfType<AudioManager>().Play("click");

        Time.timeScale = 1 ;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
