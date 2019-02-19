using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventAggregation;

public class AudioController : MonoBehaviour
{
    public AudioSource sound;
    public AudioClip fireSound;
    public AudioClip crashBall;
    public AudioClip doorOpen;
    public AudioClip doorUnlock;

    private void Awake()
    {
        EventAggregator.Subscribe<SoundFireEvent>(OnFireSound);
        EventAggregator.Subscribe<SoundBallCrash>(OnBallCrash);
        EventAggregator.Subscribe<SoundDooropen>(OnDoorOpen);
        EventAggregator.Subscribe<SoundDoorUnlocked>(OnDoorUnlock);
    }

    void OnFireSound(IEventBase eventbase)
    {
        sound.clip = fireSound;
        sound.Play();
    }

    void OnBallCrash(IEventBase eventbase)
    {
        sound.clip = crashBall;
        sound.Play();
    }

    void OnDoorOpen(IEventBase eventbase)
    {
        sound.clip = doorOpen;
        sound.Play();
    }

    void OnDoorUnlock(IEventBase eventbase)
    {
        sound.clip = doorUnlock;
        sound.Play();
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
