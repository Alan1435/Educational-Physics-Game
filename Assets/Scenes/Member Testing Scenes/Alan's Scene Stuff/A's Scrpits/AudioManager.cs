using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    /*
    
    To use AudioManager
    Add a new sound Element in the Audio Manager GameObject's Audio Manager Script
    Give it a name
    Attach the sound file
    And adjust Volume/Pitch accordingly 

    To play music/sound effects in this script use the Play() Function 
    Code: Play("Name of Sound effect"); 
    
    To play music/sound effects in other scripts also use the Play() Function 
    Code: FindObjectOfType<AudioManager>().Play("Name of Sound effect");

    */

    private GameObject playButton ;

    public Sound[] sounds;
    void Awake()
    {
        foreach (Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

        }
    }

    void Start(){
        //Play Background Music here
        Play("TinkerTrailsSlow");

        playButton = GameObject.Find("Play Button") ;

        if (playButton != null)
        {
        playButton.GetComponent<PlayButton>().OnPlayClick += SwitchMusic ;
        }
    }
    
    public void Play(string name){

        Sound s = Array.Find(sounds, sounds => sounds.name == name);
        s.source.time = 0.0f;
        //Debug.Log(s.source.time);
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sounds => sounds.name == name) ;
        s.source.Stop() ;
    }
    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sounds => sounds.name == name);
        return s.source.isPlaying;
    }

    private void SwitchMusic()
    {
        Stop("TinkerTrailsSlow") ;
        Play("TinkerTrailsFast") ;
    }
}
