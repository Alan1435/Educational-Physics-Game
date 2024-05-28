using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ZoomButtonScript : MonoBehaviour
{
    //btn is the object the script is attached to
    [SerializeField] Button btn;
    //text is the text attached to btn
    //[SerializeField] TMP_Text text;
    //This array is a list of the ui elements that are disabled/enabled when zooming in/out
    [SerializeField] GameObject[] uiElements;
    //This is the camera that the zoom button controls
    [SerializeField] Camera cam;
    //gets placer
    [SerializeField] GameObject placer;
    [SerializeField] Sprite zoomInSprite;
    [SerializeField] Sprite zoomOutSprite;
    //This is the integer that determine's the camera's size when zoomed in
    [SerializeField] int zoomInSize;
    //This is the int for the zoomed out size
    [SerializeField] int zoomOutSize;
    //This int determines the zoom speed
    [SerializeField] float zoomSpeed;
    [SerializeField] Vector3 zoomInPosition;
    [SerializeField] Vector3 zoomOutPosition;
    //[SerializeField] float zoomPosSpeed;
    [SerializeField] AudioSource audioSource;

    //This bool marks whether the screen is zoomed in or out
    public bool zoomedOut;
    //These bools mark if the zooming out / in coroutines are happening respectively
    private bool zoomingIn;
    private bool zoomingOut;
    private float xMoveRatio;
    private float yMoveRatio;
    private Vector3 incrementMovement;

    private float zoomTime;
    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(delegate {determineZoom();});
        zoomingIn = false;
        zoomingOut = false;
        /*xMoveRatio = zoomOutPosition.x - zoomInPosition.x;
        yMoveRatio = zoomOutPosition.y - zoomInPosition.y;
        incrementMovement = new Vector3(zoomPosSpeed * xMoveRatio, zoomPosSpeed * yMoveRatio, 0);*/
        //Debug.Log(incrementMovement.x);
        zoomTime = 0;
        //Debug.Log(zoomOutPosition.x);
    }

    // Update is called once per frame
    void Update()
    {
        if(zoomingOut)
        {
            //Changes camera size while zooming out
            //if(/*cam.orthographicSize*/Camera.main.orthographicSize<zoomOutSize)
           // {
                //cam.orthographicSize = cam.orthographicSize + (1 * Time.deltaTime);
                //Camera.main.orthographicSize = Camera.main.orthographicSize + (zoomSpeed * Time.deltaTime);
            //    Camera.main.orthographicSize = Mathf.Lerp(zoomInSize,zoomOutSize,zoomTime);
            //}
            //if(Camera.main.transform.position.x < zoomOutPosition.x)
            //{
                /*Camera.main.GetComponent<CameraMovement>().offset = incrementMovement * Time.deltaTime;
                Camera.main.GetComponent<CameraMovement>().offset = new Vector3 (Camera.main.GetComponent<CameraMovement>().offset.x, Camera.main.GetComponent<CameraMovement>().offset.y, -10);
                Debug.Log(Camera.main.GetComponent<CameraMovement>().offset.x);*/

                /*Camera.main.transform.position = new Vector3 (Mathf.Lerp(zoomInPosition.x,zoomOutPosition.x,zoomTime), Mathf.Lerp(zoomInPosition.y, zoomOutPosition.y, zoomTime), -10);
                zoomTime += zoomPosSpeed * Time.deltaTime;
                Debug.Log(Camera.main.transform.position.x);
            }
            if(Camera.main.orthographicSize>=zoomOutSize && Camera.main.transform.position.x >= zoomOutPosition.x)
            {
                Debug.Log("Moving done");
                zoomingOut = false;
                Camera.main.transform.position = zoomOutPosition;
            }*/
            if(zoomTime < 1)
            {
                Camera.main.orthographicSize = Mathf.Lerp(zoomInSize, zoomOutSize, zoomTime);
                Camera.main.transform.position = new Vector3 (Mathf.Lerp(zoomInPosition.x, zoomOutPosition.x, zoomTime), Mathf.Lerp(zoomInPosition.y, zoomOutPosition.y, zoomTime), -10);
                zoomTime += zoomSpeed * Time.deltaTime;
            }
            else
            {
                zoomingOut = false;
            }
        }
        if(zoomingIn)
        {
            //changes camera size while zooming in
            /*if(Camera.main.orthographicSize>zoomInSize)
            {
                //Camera.main.orthographicSize = Camera.main.orthographicSize - (zoomSpeed * Time.deltaTime); 
                Camera.main.orthographicSize = Mathf.Lerp(zoomOutSize, zoomInSize, zoomTime);
            }
            if(Camera.main.transform.position.x > zoomInPosition.x)
            {
                /*Camera.main.GetComponent<CameraMovement>().offset = incrementMovement * Time.deltaTime;
                Camera.main.GetComponent<CameraMovement>().offset = new Vector3(Camera.main.GetComponent<CameraMovement>().offset.x, Camera.main.GetComponent<CameraMovement>().offset.y, -10);
                Debug.Log(Camera.main.GetComponent<CameraMovement>().offset.x);*/
            /*    Camera.main.transform.position = new Vector3 (Mathf.Lerp(zoomOutPosition.x,zoomInPosition.x,zoomTime), Mathf.Lerp(zoomOutPosition.y, zoomInPosition.y, zoomTime), -10);
                zoomTime += zoomPosSpeed * Time.deltaTime;
            }
            if(Camera.main.orthographicSize<=zoomInSize && Camera.main.transform.position.x <= zoomInPosition.x)
            {
                Debug.Log("Moving Done");
                zoomingIn = false;
                //Enables UI elements when zoomed in
                for(int i = 0; i < uiElements.Length; i++)
                {
                    uiElements[i].SetActive(true);
                }
                //Camera.main.transform.position = zoomInPosition;
            }*/
            if(zoomTime < 1)
            {
                Camera.main.orthographicSize = Mathf.Lerp (zoomOutSize, zoomInSize, zoomTime);
                Camera.main.transform.position = new Vector3 (Mathf.Lerp(zoomOutPosition.x, zoomInPosition.y, zoomTime), Mathf.Lerp(zoomOutPosition.y, zoomInPosition.y, zoomTime), -10);
                zoomTime += zoomSpeed * Time.deltaTime;
            }
            else
            {
                zoomingIn = false;
                
                for(int i = 0; i < uiElements.Length; i++)
                {
                    uiElements[i].SetActive(true);
                }
            }
        }
    }
    void determineZoom()
    {
        
        if(zoomedOut && !zoomingOut)
        {
            //Screen is zoomed out, click means to zoom in
            zoomIn();
            Debug.Log("Zooming in");
            audioSource.Play();
            btn.image.sprite = zoomOutSprite;
            zoomTime = 0;
        }
        else if (!zoomedOut && !zoomingIn)
        {
            //Screen is zoomed in, click means to zoom out
            zoomOut();
            Debug.Log("Zooming out");
            audioSource.Play();
            btn.image.sprite = zoomInSprite;
            zoomTime = 0;
        }
    }
    void zoomOut()
    {
        //text.text = "Zoom In";
        zoomingOut = true;
        zoomedOut = true;
        //Disables UI elements when zoomed out (the player should only be building when zoomed in)
        for(int i = 0; i < uiElements.Length; i++)
        {
            uiElements[i].SetActive(false);
        }
        placer.GetComponent<PartPlacer>().ClearPartSelection();
    }
    void zoomIn()
    {
        //text.text = "Zoom Out";
        zoomingIn = true;
        zoomedOut = false;
    }
}
