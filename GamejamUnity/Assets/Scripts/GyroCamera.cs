using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MobileSensors;

[RequireComponent(typeof(Camera))]
public class GyroCamera : MonoBehaviour {

    private Gyro gyro;
    private Camera cam;

	// Use this for initialization
	void Start () {
        gyro = MobileInput.instance.gyro;
        cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.localRotation = gyro.attitude;
        transform.Rotate(0f, 0f, 180f, Space.Self);
        transform.Rotate(90f, 180f, 0f, Space.World);
    }
}
