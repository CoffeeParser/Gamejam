using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    public GameObject Camera;

    [Header("DrEvilSpeak")]
    public AudioClip DrEvilLaugh;
    public bool LaughTooPlay;
    public AudioClip DrEvilTalk;
    public AudioClip DrEvilUnSolved;

    [Header("AchievementSound")]
    public AudioClip AchievementSound;

    [Header("PatientSpeak")]
    public AudioClip PatientTalk;
    public AudioClip PatientScream;

    [Header("Musik")]
    public AudioClip StartScreenMusik;
    public AudioClip DialogMusik;
    public AudioClip NightRoomMusik;

    [Header("InGameSound")]
    public AudioClip doorOpen;

    public AudioSource voice;
    public AudioSource musik;


    private void Start()
    {

        AudioListener audioListener = Camera.GetComponent<AudioListener>();
        // Disable when TerroScene!!!
        audioListener.enabled = true;

        PlayLaughSound();
        PlayStartMusic();
    }


    public void PlayLaughSound()
    {
        voice.clip = DrEvilLaugh;
        voice.Play();
    }

    public void StopLaughSound()
    {
        Debug.Log("Stopt");
        //StopCoroutine(PlayLaughIterator());
        voice.Stop();
    }

    /*
    public void StartPlayLaughIterator()
    {
        StartCoroutine(PlayLaughIterator());
    }

    private IEnumerator PlayLaughIterator()
    {
        while (true)
        {
            voice.clip = DrEvilLaugh;
            voice.Play();
            yield return new WaitForSeconds(2);
        }

    }
    */

    public void PlayDayScreenMusic()
    {
        musik.clip = DialogMusik;
        musik.Play();
    }
    public void StopDayScreenMusic()
    {
        musik.clip = DialogMusik;
        musik.Stop();
    }

    public void PlayStartMusic()
    {
        musik.clip = StartScreenMusik;
        musik.Play();
    }
    public void StopStartMusic()
    {
        musik.clip = StartScreenMusik;
        musik.Stop();
    }
}
