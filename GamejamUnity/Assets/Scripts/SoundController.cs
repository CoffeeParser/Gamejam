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

    public AudioSource voiceLoop;
    public AudioSource voice;
    public AudioSource musik;

    public AudioListener audioListener;

    private void Start()
    {

        audioListener = Camera.GetComponent<AudioListener>();
        // Disable when TerroScene!!!
        audioListener.enabled = true;

        //PlayLaughSound();
        //PlayStartMusic();
    }

    public void StopAllSources()
    {
        voiceLoop.Stop();
        voice.Stop();
        musik.Stop();
    }

    // PlayVoidLoop
    public void PlayVoiceLoop(AudioClip audioClip)
    {
        voiceLoop.loop = true;
        voiceLoop.clip = audioClip;
        voiceLoop.Play();
    }

    public void StopVoiceLoop(AudioClip audioClip)
    {
        voiceLoop.Stop();
    }

    //PlayVoice 1x
    public void PlayVoice(AudioClip audioClip)
    {
        voice.clip = audioClip;
        voice.Play();
    }

    //PlayMusic
    public void PlayMusicLoop(AudioClip audioClip)
    {
        voiceLoop.loop = true;
        musik.clip = audioClip;
        musik.Play();
    }

    public void StopMusicLoop(AudioClip audioClip)
    {
        musik.Stop();
    }

    /*
    // DrEvilLaugh

    public void PlayLaughSoundLoop()
    {
        voiceLoop.clip = DrEvilLaugh;
        voiceLoop.Play();
    }

    public void StopLaughSoundLoop()
    {
        Debug.Log("Stopt");
        //StopCoroutine(PlayLaughIterator());
        voiceLoop.Stop();
    }

    // 
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
    */
}
