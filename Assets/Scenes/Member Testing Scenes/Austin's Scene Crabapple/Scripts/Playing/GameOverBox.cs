using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverBox : MonoBehaviour
{
    private LayerMask bodyLayer ;
    private LayerMask attachmentLayer ;
    private LayerMask allParts ;

    public bool gameOver ;

    [SerializeField] GameObject buildUI ;
    [SerializeField] GameObject playUI ;
    [SerializeField] GameObject gameOverUI ;
    [SerializeField] GameObject pauseUI ;
    [SerializeField] Win won;

    [SerializeField] GameObject vehicleParent ;

    private void Start()
    {
        // Gets layermasks
        bodyLayer = (1 << 6) ;
        attachmentLayer = (1 << 7) ;
        allParts = bodyLayer | attachmentLayer ;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (LayerMaskContains(other.gameObject.layer, allParts) && !gameOver && !won.gameWon)
        {
            Debug.Log("Game Over") ;
            GameOver() ;
            FindObjectOfType<AudioManager>().Play("GameOverSound");
        }
        else if (LayerMaskContains(other.gameObject.layer, allParts) && !gameOver)
        {
            DestroyJointsOfCar() ;
        }
    }

    // End the game, pull up gameover menu, etc.
    public void GameOver()
    {
        gameOver = true ;  

        buildUI.SetActive(false) ;
        playUI.SetActive(false) ;
        pauseUI.SetActive(false) ;

        gameOverUI.SetActive(true) ;

        DestroyJointsOfCar() ;
    }

    // Return the value of gameOver
    public bool IsGameOver()
    {
        return gameOver ;
    }

    // When called, will destroy all of the joints on the vehicle
    public void DestroyJointsOfCar()
    {
        Debug.Log("Break all joints") ;
        
        Transform[] vehicleParts = vehicleParent.GetComponentsInChildren<Transform>() ;

        foreach (Transform part in vehicleParts)
        {
            if (part != vehicleParent.transform)
            {
                GameObject partObject = part.gameObject ;

                WheelJoint2D[] wheelJoints2D = partObject.GetComponents<WheelJoint2D>() ;
                RelativeJoint2D[] relativeJoints2D = partObject.GetComponents<RelativeJoint2D>() ;

                foreach (WheelJoint2D wheelJoint2D in wheelJoints2D)
                {
                    Destroy(wheelJoint2D) ;
                }

                foreach (RelativeJoint2D relativeJoint2D in relativeJoints2D)
                {
                    Destroy(relativeJoint2D) ;
                }
            }
        }
    }    

    // returns true if the layer is contained in layerMask, false otherwise
    private bool LayerMaskContains(int layer, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << layer)) ;
    }
}
