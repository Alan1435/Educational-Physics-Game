using UnityEngine;
using UnityEngine.UI;

public class PlayPauseController : MonoBehaviour
{
    public Button playPauseButton;
    public Text buttonText;
    public GameObject pauseMenu;

    private bool isPlaying = false;

    void Start()
    {
        playPauseButton.onClick.AddListener(TogglePlayPause);
        pauseMenu.SetActive(false);
    }

    void TogglePlayPause()
    {
        isPlaying = !isPlaying;

        if (isPlaying)
        {
            buttonText.text = "Pause";
            // need code to start the car movement here
        }
        else
        {
            buttonText.text = "Play";
            pauseMenu.SetActive(true);
            // need code to pause the car movement here
        }
    }

    public void ResumeGame()
    {
        isPlaying = true;
        buttonText.text = "Pause";
        pauseMenu.SetActive(false);
        // need code to resume the car movement here
    }

    public void RestartLevel()
    {
        //need code to restart
    }

    public void GoToHomeMenu()
    {
        //need code to go to home
    }
}