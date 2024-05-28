using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followTheMouse : MonoBehaviour
{
        public bool followsMouse;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(followsMouse)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }
}
