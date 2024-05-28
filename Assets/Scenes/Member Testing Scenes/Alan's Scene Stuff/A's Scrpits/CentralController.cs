using System.Collections.Generic;
using UnityEngine;

public class CentralController : MonoBehaviour
{
    public static CentralController Instance; // Singleton instance

    private List<WheelFunction> wheels = new List<WheelFunction>(); // List to keep track of all wheel instances

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

    // Method to add wheel to the list
    public void RegisterWheel(WheelFunction wheel)
    {
        if (!wheels.Contains(wheel))
        {
            wheels.Add(wheel);
        }
    }

    // Call to start moving all wheels
    public void StartAllWheels()
    {
        foreach (var wheel in wheels)
        {
            wheel.moveCar(true);
        }
    }

    // Call to stop all wheels
    public void StopAllWheels()
    {
        foreach (var wheel in wheels)
        {
            wheel.moveCar(false);
        }
    }
}
