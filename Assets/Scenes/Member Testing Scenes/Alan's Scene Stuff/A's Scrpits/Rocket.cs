using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb2D;
    [SerializeField] float thrust = 0f;

    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb2D.AddForce(transform.up * thrust);
        // Alternatively, specify the force mode, which is ForceMode2D.Force by default
        //rb2D.AddForce(transform.up * thrust, ForceMode2D.Impulse);
    }
}
