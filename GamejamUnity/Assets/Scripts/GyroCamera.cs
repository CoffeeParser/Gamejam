using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MobileSensors;

namespace MobileSensors
{
    /// <summary>
    /// Applying the gyro input to the gyro camera
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class GyroCamera : MonoBehaviour
    {

        private Gyro gyro;

        // Use this for initialization
        void Start()
        {
            gyro = MobileInput.instance.gyro;
        }

        // Update is called once per frame
        void Update()
        {
            transform.localRotation = gyro.attitude;
            transform.Rotate(0f, 0f, 180f, Space.Self);
            transform.Rotate(90f, 180f, 0f, Space.World);
        }
    }
}