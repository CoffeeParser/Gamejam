using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// The Class Mic will allow the developer to acces a camera.
/// </summary>
public class Mic : MonoBehaviour
{

    /// <summary>
    /// Index of device which will be accessed.
    /// </summary>
    public int deviceID = 0;

    /// <summary>
    /// The current microphone level.
    /// </summary>
    public float micLevel;

    /// <summary>
    /// The current microphone level in dB.
    /// </summary>
    public float micLevelinDb;

    /// <summary>
    /// The amount of samples which will be iteratet to find the highest peak.
    /// </summary>
    public int _sampleWindow = 4;

    public List<Threshold> thresholdTriggers;

    public bool debugging;

    //private string[] devices;
    private string deviceName;
    private AudioClip recClip;

    public EventString OnThresholdExceeded;

    // Use this for initialization
    void Awake()
    {
        StartCoroutine(WaitForDevice());

        if (OnThresholdExceeded == null)
            OnThresholdExceeded = new EventString();

    }

    // Update is called once per frame
    void Update()
    {
        if (!string.IsNullOrEmpty(deviceName))
        {
            if (Microphone.IsRecording(deviceName))
            {
                micLevel = GetMaxLevel();
                micLevelinDb = GetDbFromLevel(micLevel);

                for (int i = 0; i < thresholdTriggers.Count; i++)
                {
                    if (micLevelinDb >= thresholdTriggers[i].amount)
                        OnThresholdExceeded.Invoke(thresholdTriggers[i].name);
                }
            }
        }
    }

    private IEnumerator WaitForDevice()
    {
        int waitingAttempts = 5;
        while (waitingAttempts >= 0)
        {
            if (debugging)
                Debug.Log("Waiting for Mic-Device!");

            if (deviceID < Microphone.devices.Length)
            {
                deviceName = Microphone.devices[deviceID];

                if (debugging)
                    Debug.Log("Microphone Device" + deviceName + "Found!");

                recClip = Microphone.Start(deviceName, true, 999, 44100);

                yield break;
            }

            yield return new WaitForSeconds(0.5f);

            waitingAttempts--;
        }

        if (Microphone.devices == null || Microphone.devices.Length <= 0)
        {
            if (debugging)
                Debug.LogWarning("No Microphone Devices detected!");
        } else if (debugging)
        {
            Debug.LogWarning("No Microphone with ID " + deviceID + " found!");
        }
    }

    private void StopMic()
    {
        Microphone.End(deviceName);
    }

    //get max level from mic
    float GetMaxLevel()
    {
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(null) - (_sampleWindow + 1);

        if (micPosition < 0) return 0;

        recClip.GetData(waveData, micPosition);

        // Getting a peak on the last samples in sampleWindow
        for (int i = 0; i < _sampleWindow; i++)
        {
            float wavePeak = waveData[i] * waveData[i];

            //Cache higher peaks into levelMax
            if (levelMax < wavePeak)
                levelMax = wavePeak;
        }
        return levelMax;
    }

    //mic loudness to decibel
    float GetDbFromLevel(float level)
    {
        return 20 * Mathf.Log10(Mathf.Abs(level));
    }
}

