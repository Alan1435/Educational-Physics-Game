using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacerSounds : MonoBehaviour
{
    [SerializeField] AudioSource audioSource ;
    [SerializeField] AudioClip place, noplace, delete ;

    public void PlayPlacerSound(string var)
    {
        switch (var)
        {
            case "place": 
                audioSource.clip = place ;
                break ;

            case "noplace":
                audioSource.clip = noplace ;
                break ;

            case "delete":
                audioSource.clip = delete ;
                break ;

            default:
                // How did we get here?
                audioSource.clip = delete ;
                break ;
        }

        audioSource.Play() ;
    }
}
