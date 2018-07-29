using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileSensors
{

    /// <summary>
    /// Handling all sensor input regarding accelerometer. As example you can define a threshold und get a shakeEvent.
    /// </summary>
    public class Accelerate : MonoBehaviour
    {

        /// <summary>
        /// Amount of time between shakes.
        /// </summary>
        public float minShakeInterval;
        /// <summary>
        /// List of independent thresholds.
        /// </summary>
        public List<Threshold> thresholds;
        /// <summary>
        /// The raw mobile acceleration Input
        /// </summary>
        public Vector3 acceleration;
        /// <summary>
        /// The raw mobile delta acceleration Input
        /// </summary>
        public Vector3 deltaAcceleration;
        /// <summary>
        /// The acceleration strength as magnitude
        /// </summary>
        public float accelMagn;
        /// <summary>
        /// The delta acceleration strength as magnitude
        /// </summary>
        public float accelMagnDelta;

        private float timeAfterShake;
        private Vector3 lowPassAccel;
        private float accelerometerUpdateInterval = 1.0f / 60.0f;
        private float lowPassKernelWidthInSeconds = 1.0f;
        private float lowPassFilterFactor;

        /// <summary>
        /// Listen to this event to get Shake.
        /// </summary>
        public EventThresholdLevel OnShake;

        void Awake()
        {
            lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
            lowPassAccel = Input.acceleration;

            if (OnShake == null)
                OnShake = new EventThresholdLevel();

        }

        void Update()
        {
            acceleration = Input.acceleration;

            lowPassAccel = Vector3.Lerp(lowPassAccel, acceleration, lowPassFilterFactor);
            deltaAcceleration = acceleration - lowPassAccel;

            accelMagn = acceleration.sqrMagnitude;
            accelMagnDelta = deltaAcceleration.sqrMagnitude;

            timeAfterShake += Time.deltaTime;

            if (timeAfterShake >= minShakeInterval)
            {
                //Trigger Threshold shake Event if threshold exceeded
                foreach (Threshold threshold in thresholds)
                {
                    if (accelMagnDelta >= threshold.amount)
                    {
                        OnShake.Invoke(threshold.level);
                        timeAfterShake = 0;
                    }
                }
            }
        }
    }
}