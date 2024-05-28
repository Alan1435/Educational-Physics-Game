using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI ;

public class PartButtonClick : MonoBehaviour
{
    [SerializeField] Button button ;

    // Start is called before the first frame update
    private void Start()
    {
        button.onClick.AddListener(ButtonClick) ;
    }

    // Update is called once per frame
    private void ButtonClick()
    {
        FindObjectOfType<AudioManager>().Play("click");
    }
}
