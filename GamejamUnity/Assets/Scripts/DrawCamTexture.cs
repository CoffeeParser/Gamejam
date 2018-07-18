using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileSensors
{
    public class DrawCamTexture : MonoBehaviour
    {

        MobileCamera camMain;

        // Use this for initialization
        void Start()
        {
            camMain = MobileInput.inst.camMain;
            camMain.OnCameraTexture.AddListener(SetCameraTexture);
        }

        void SetCameraTexture()
        {
            GetComponent<Renderer>().material.mainTexture = camMain.camTexture;
        }

        private void Update()
        {
            //GetComponent<Renderer>().material.mainTexture = camWeb.camTexture;
        }
    }
}
