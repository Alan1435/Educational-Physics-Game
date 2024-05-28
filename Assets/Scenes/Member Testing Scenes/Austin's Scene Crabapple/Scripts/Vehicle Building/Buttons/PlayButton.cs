using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    /*  I moved some of the functionality around so that the play and pause button are two separate things.
     *  It was a lot more organized to use seperate canvases that are hidden/shown depending on the screen you're in
     *  And in doing so, it didn't make much sense to use the same gameobject to do multiple different things
     *  ie. playing and pausing.
     *  So, I created a seperate pause button.
     *  You also don't need a list of game objects to hide, but just a reference to the applicable canvases.
     */

    [SerializeField] Button button ;

    //FB addition
    [SerializeField] GameObject placer;

    public delegate void PlayButtonHandler() ;
    public event PlayButtonHandler OnPlayClick ;

    //Felix's addition, makes it so necessary parts must be placed
    [SerializeField] GameObject mandatoryPartButton;
    [SerializeField] int mandatoryAmount;

    /*
    //List of objects to hide
    [SerializeField] GameObject[] uiElementsToHide;
    [SerializeField] GameObject[] uiElementsToShow;
    */

    //Pause button sprite
    // [SerializeField] Sprite pauseButtonSprite;

    //determines if btn should behave like pause
    // private bool isRunning;
    //private bool isRunning2;

    // [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject buildUI ;
    [SerializeField] GameObject playUI ;

    [SerializeField] GameObject menuControl;

    //FB addition - I'm adding fields for the locked and unlocked version of the play button here, so that it can change when the weight is placed down
    [SerializeField] Sprite lockedPlaySprite;
    [SerializeField] Sprite unlockedPlaySprite;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(PlayClicked);

        // isRunning = false;
        //isRunning2 = false;

        /*
        pauseMenu = Instantiate(prefabMenu, prefabMenu.transform.position, Quaternion.identity);
        menuControl.GetComponent<menuControllerScript>().menu = pauseMenu;
        menuControl.GetComponent<menuControllerScript>().hideMenu();
        */
    }

    void Update()
    {
        if(placer.GetComponent<PlacerPartDeletion>().buildPhase)
        {
            if(mandatoryAmount >= mandatoryPartButton.GetComponent<PartButton>().GetPartNumber())
            {
                button.image.sprite = unlockedPlaySprite;
            }
            else
            {
                button.image.sprite = lockedPlaySprite;
            }
        }
    }

    // Invoke the play click event
    private void PlayClicked()
    {
        /*if(isRunning2)
        {
            Time.timeScale = 1;
            isRunning = false;
        }*/

        /*
        if(isRunning)
        {
            Time.timeScale = 0;
            audioSource.Play();

            menuControl.GetComponent<menuControllerScript>().showPauseMenu();

            playUI.SetActive(false) ;

            Debug.Log("pauseClicked");
            //isRunning2 = true;
        }
        */

        if(mandatoryAmount < mandatoryPartButton.GetComponent<PartButton>().GetPartNumber())
        {
            placer.GetComponent<PlacerSounds>().PlayPlacerSound("noplace");
        }

        //Felix addition, to check if the necessary parts have been placed.
        //If multiple mandatory parts are ever needed, switch this to a seperate function,
        //and have it take in 2 arrays of corresponding gameobjects and ints, and then
        //if everything matches set some boolean to true.
        if(mandatoryAmount >= mandatoryPartButton.GetComponent<PartButton>().GetPartNumber() /* && !isRunning */ )
        {
            OnPlayClick?.Invoke() ;
            Camera.main.GetComponent<CameraMovement>().isMoving = true;
            //FB addition  
            FindObjectOfType<AudioManager>().Play("click");

            placer.GetComponent<PlacerPartDeletion>().buildPhase = false;
            placer.GetComponent<PartPlacer>().ClearPartSelection();
            // button.image.sprite = pauseButtonSprite;

            /*
            for(int i = 0; i < uiElementsToHide.Length; i++)
            {
                uiElementsToHide[i].SetActive(false);
            }
            for(int i = 0; i < uiElementsToShow.Length; i++)
            {
                uiElementsToShow[i].SetActive(true);
            }
            */

            buildUI.SetActive(false) ;
            playUI.SetActive(true) ;

            // isRunning = true;
        }
    }
}
