using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

public class MobileCamera : MonoBehaviour
{

    /// <summary>
    /// The number of frames per second
    /// </summary>
    //private int framesPerSecond = 0;

    /// <summary>
    /// The current frame count
    /// </summary>
    private int frameCount = 0;

    /// <summary>
    /// The frames timer
    /// </summary>
    private DateTime timerFrames = DateTime.MinValue;

    /// <summary>
    /// The selected device index
    /// </summary>
    public int deviceID = -1;
    public string deviceName;

    /// <summary>
    /// The web cam texture
    /// </summary>
    public WebCamTexture camTexture = null;

    public UnityEvent OnCameraTexture;
    public bool debugging;

    private int waitingAttemptsForDevice;

    IEnumerator Start()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            StartCoroutine("WaitForDevice");
        }
        else
        {
            if (debugging)
                Debug.LogWarning("No Camera Authorization!");
        }
    }

    private IEnumerator WaitForDevice()
    {

        while (waitingAttemptsForDevice < 5)
        {
            if (debugging)
                Debug.Log("Waiting for Camera-Device!");

            if (deviceID < WebCamTexture.devices.Length)
            {
                WebCamDevice device = WebCamTexture.devices[deviceID];

                // use the device name
                camTexture = new WebCamTexture(device.name);

                deviceName = device.name;
                if (debugging)
                    Debug.Log("Camera Device" + device.name + "Found!");

                // start playing
                camTexture.Play();
                OnCameraTexture.Invoke();

                yield break;
            }

            yield return new WaitForSeconds(0.5f);

            waitingAttemptsForDevice += 1;
        }

        if (WebCamTexture.devices == null || WebCamTexture.devices.Length <= 0)
            if (debugging)
                Debug.LogWarning("No Camera Devices detected!");
        else if (debugging)
                Debug.LogWarning("No Camera with ID " + deviceID + " found!");
    }

    private void StopCamera()
    {
        // Stop Camera
        if (camTexture != null)
        {
            if (camTexture.isPlaying)
            {
                camTexture.Stop();
            }
        }
    }

    public void CountFrames()
    {
        if (timerFrames < DateTime.Now)
        {
            //framesPerSecond = frameCount;
            frameCount = 0;
            timerFrames = DateTime.Now + TimeSpan.FromSeconds(1);
        }
        ++frameCount;
    }

    public string GetSelectedDevice()
    {
        if (deviceID >= 0 && WebCamTexture.devices.Length > 0)
            return WebCamTexture.devices[deviceID].name;
        return null;
    }
}
