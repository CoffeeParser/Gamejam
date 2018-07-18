using UnityEngine;
using PedometerU;
using System.Collections;

namespace MobileSensors
{

    public class Steps : MonoBehaviour
    {

        public int steps;
        public string distance;

        private Pedometer pedometer;
        public bool debbuging;

        void Start()
        {
            MobileInput.inst.OnUnityRemoteStarted.AddListener(StartPedometer);
            MobileInput.inst.location.OnLocationServiceStarted.AddListener(StartPedometer);
        }

        private void StartPedometer()
        {
            if (Input.location.status == LocationServiceStatus.Running)
            {
                if(debbuging)
                    Debug.Log("Start Pedometer");

                // Create a new pedometer
                pedometer = new Pedometer(OnStep);
                // Reset UI
                OnStep(0, 0);
            }
        }

        private void OnStep(int _steps, double _distance)
        {
            // Display the values // Distance in feet
            steps = _steps;
            distance = (_distance * 3.28084).ToString("F2") + " ft";
        }

        private void OnDisable()
        {
            // Release the pedometer
            pedometer.Dispose();
            pedometer = null;
        }
    }
}