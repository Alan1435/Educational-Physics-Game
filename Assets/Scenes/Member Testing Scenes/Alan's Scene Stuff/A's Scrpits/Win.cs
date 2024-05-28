using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    private LayerMask bodyLayer ;
    private LayerMask attachmentLayer ;
    private LayerMask allParts ;

    public bool gameWon ;

    [SerializeField] GameObject buildUI ;
    [SerializeField] GameObject playUI ;
    [SerializeField] GameObject educationalUI ;
    [SerializeField] GameObject pauseUI ;
    [SerializeField] GameOverBox[] gomJabbars;

    [SerializeField] ParticleSystem[] fireworks ;

    private bool gameOverMe ;

    private void Start()
    {
        // Gets layermasks
        bodyLayer = (1 << 6) ;
        attachmentLayer = (1 << 7) ;
        allParts = bodyLayer | attachmentLayer ;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        gameOverMe = false ;
        foreach (GameOverBox lost in gomJabbars)
        {
            if (lost.gameOver)
            {
                gameOverMe = true ;
            }
        }

        if (LayerMaskContains(other.gameObject.layer, allParts) && !gameWon && !gameOverMe)
        {
            FindObjectOfType<AudioManager>().Play("WinSound");
            Debug.Log("Game won") ;
            GameWon() ;
        }
    }

    // End the game, pull up gameover menu, etc.
    public void GameWon()
    {
        gameWon = true ;  

        buildUI.SetActive(false) ;
        playUI.SetActive(false) ;
        pauseUI.SetActive(false) ;

        educationalUI.SetActive(true) ;

        foreach (ParticleSystem part in fireworks)
        {
            part.Play() ;
        }
    }

    // Return the value of gameOver
    public bool IsGameWon()
    {
        return gameWon ;
    }
    

    // returns true if the layer is contained in layerMask, false otherwise
    private bool LayerMaskContains(int layer, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << layer)) ;
    }
}
