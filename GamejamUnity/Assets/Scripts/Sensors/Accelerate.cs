using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accelerate : MonoBehaviour
{

    public float minShakeInterval;
    public List<Threshold> thresholds;
    public Vector3 acceleration;
    public Vector3 deltaAcceleration;
    public float accelMagn;
    public float accelMagnDelta;

    private float timeAfterShake;
    private Vector3 lowPassAccel;
    private float accelerometerUpdateInterval = 1.0f / 60.0f;
    private float lowPassKernelWidthInSeconds = 1.0f;
    private float lowPassFilterFactor;

    public EventString OnShake;

    void Awake()
    {
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        lowPassAccel = Input.acceleration;

        if (OnShake == null)
            OnShake = new EventString();

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
                    OnShake.Invoke(threshold.name);
                    timeAfterShake = 0;
                }
            }
        }
    }
}
