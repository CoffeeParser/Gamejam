using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MobileSensors
{

    /// <summary>
    /// Handling Gyro Sensor input from your mobile device. 
    /// </summary>
    public class Gyro : MonoBehaviour
    {
        /// <summary>
        /// Altitude of the device
        /// </summary>
        public Quaternion attitude;
        /// <summary>
        /// RotaionRate of the device
        /// </summary>
        public Vector3 rotationRate;
        /// <summary>
        /// Gravity of the device
        /// </summary>
        public Vector3 gravity;
        /// <summary>
        /// The Gyro Class of the device
        /// </summary>
        private Gyroscope gyro;

        // Use this for initialization
        void Awake()
        {
            TryEnableGyro();
        }

        // Update is called once per frame
        void Update()
        {
            if (gyro != null && gyro.enabled)
            {
                //Multiply attitude with a rotation to get a gyroCamera
                attitude = gyro.attitude;
                rotationRate = gyro.rotationRate;
                gravity = gyro.gravity;
            }
        }

        private bool TryEnableGyro()
        {
            if (SystemInfo.supportsGyroscope)
            {
                gyro = Input.gyro;
                gyro.enabled = true;
                return true;
            }
            return false;
        }
    }
}