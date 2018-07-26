using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    //public GameObject Camera;
    /// <summary>
    /// Hold all Voice an Music for Screenplay
    /// Let Play & Stop all AudioSource
    /// </summary>
    [Header("DrEvilSpeak")]
    public AudioClip DrEvilLaugh;
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

    /// <summary>
    ///  Play AudioSource Voice and let Loop
    /// </summary>
    /// <param name="audioClip"></param>

    // PlayVoidLoop
    public void PlayVoiceLoop(AudioClip audioClip)
    {
        voiceLoop.loop = true;
        voiceLoop.clip = audioClip;
        voiceLoop.Play();
    }

    /// <summary>
    ///  Stop AudioSource VoiceLoop 
    /// </summary>
    /// <param name="audioClip"></param>

    public void StopVoiceLoop(AudioClip audioClip)
    {
        voiceLoop.Stop();
    }


    /// <summary>
    /// Play AudioSource voice 1x
    /// </summary>
    /// <param name="audioClip"></param>
    //PlayVoice 1x
    public void PlayVoice(AudioClip audioClip)
    {
        voice.clip = audioClip;
        voice.Play();
    }

    /// <summary>
    /// Play AudioSource Music in Loop
    /// </summary>
    /// <param name="audioClip"></param>

    //PlayMusic
    public void PlayMusicLoop(AudioClip audioClip)
    {
        voiceLoop.loop = true;
        musik.clip = audioClip;
        musik.Play();
    }

    /// <summary>
    ///  Stop AudioSource Music Loop
    /// </summary>
    /// <param name="audioClip"></param>

    public void StopMusicLoop(AudioClip audioClip)
    {
        musik.Stop();
    }

}
