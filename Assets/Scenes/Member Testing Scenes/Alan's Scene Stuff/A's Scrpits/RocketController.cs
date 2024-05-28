using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    public static RocketController Instance; // Singleton instance

    private List<RocketFunction> rockets = new List<RocketFunction>(); // List to keep track of all rockets instances

    private void Awake()
    {
        // Initialize singleton instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void RegisterRocket(RocketFunction rocket)
    {
        if (!rockets.Contains(rocket))
        {
            rockets.Add(rocket);
        }
    }


    public void StartAllRockets()
    {
        FindObjectOfType<AudioManager>().Play("RocketSound");
        foreach (var rocket in rockets)
        {
            rocket.moveRocket(true);
        }
    }

    public void StopAllRockets()
    {
        foreach (var rocket in rockets)
        {
            rocket.moveRocket(false);
        }
    }
}
