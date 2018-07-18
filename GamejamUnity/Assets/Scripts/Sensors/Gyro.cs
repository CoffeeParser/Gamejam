using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyro : MonoBehaviour {

    public Quaternion attitude;
    public Vector3 rotationRate;
    public Vector3 gravity;
    private Gyroscope gyro;

	// Use this for initialization
	void Awake () {
        TryEnableGyro();
	}
	
	// Update is called once per frame
	void Update () {
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
