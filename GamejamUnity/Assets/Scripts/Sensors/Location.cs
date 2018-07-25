using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace MobileSensors
{
    public class Location : MonoBehaviour
    {

        public LocationInfo locationInfo;
        public bool debugging;
        public UnityEvent OnLocationServiceStarted;
        public EventLocation OnLocationUpdate;

        private void Awake()
        {
            if (OnLocationServiceStarted == null)
                OnLocationServiceStarted = new UnityEvent();

            if (OnLocationUpdate == null)
                OnLocationUpdate = new EventLocation();
        }

        // Use this for initialization
        void Start()
        {
            OnLocationServiceStarted.AddListener(UpdateLocationInfo);
            MobileInput.instance.OnUnityRemoteStarted.AddListener(delegate()
            {
                StartCoroutine("StartLocationService");
            });
        }

        private IEnumerator StartLocationService()
        {
            while (Input.location.status != LocationServiceStatus.Running)
            {
                // check if user has location service enabled
                if (!Input.location.isEnabledByUser)
                {
                    if (debugging)
                        Debug.Log("No locations enabled in the device");
                    yield break;
                }

                // Start location service
                Input.location.Start();
                if (debugging)
                    Debug.Log("Location Service starting up!");

                // Wait for initialization
                int initAttempts = 10;
                while (Input.location.status == LocationServiceStatus.Initializing && initAttempts > 0)
                {
                    if (debugging)
                        Debug.Log("Location Service INITIALIZING!");
                    initAttempts--;
                    yield return new WaitForSeconds(0.5f);
                }

                if (Input.location.status == LocationServiceStatus.Failed)
                {
                    if (debugging)
                        Debug.LogWarning("Starting location service failed!");
                    yield break;
                }

                yield return new WaitForSeconds(5);
            }

            if (debugging)
                Debug.Log("Location Services started succesfully!");

            OnLocationServiceStarted.Invoke();
        }

        private void UpdateLocationInfo()
        {
            if (Input.location.status == LocationServiceStatus.Running)
            {
                LocationInfo tempLocInfo = Input.location.lastData;
                if (locationInfo.latitude != tempLocInfo.latitude ||
                    locationInfo.longitude != tempLocInfo.longitude ||
                    locationInfo.altitude != tempLocInfo.altitude)
                {
                    locationInfo = tempLocInfo;
                    OnLocationUpdate.Invoke(locationInfo);
                }

                Invoke("UpdateLocationInfo", 2);
            } else
            {
                CancelInvoke("UpdateLocationInfo");
            }
        }

    }
}