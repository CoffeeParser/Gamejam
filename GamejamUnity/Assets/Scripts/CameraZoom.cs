using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileSensors
{

    public class CameraZoom : MonoBehaviour
    {

        private Camera cam;

        // Use this for initialization
        void Awake()
        {

            cam = GetComponent<Camera>();

        }

        // Update is called once per frame
        void Update()
        {

            cam.fieldOfView += MobileInput.inst.pinch.deltaMagn;
            cam.transform.rotation *= Quaternion.Euler(0, 0, MobileInput.inst.pinch.deltaRot);

        }
    }
}