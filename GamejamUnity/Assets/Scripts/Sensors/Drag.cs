using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MobileSensors
{
    /// <summary>
    /// Handle a drag Object
    /// </summary>
    public class Drag : MonoBehaviour
    {

        GameObject dragObj;

        // Use this for initialization
        void Start()
        {
            MobileInput.instance.touch.OnDrag.AddListener(OnDrag);
            MobileInput.instance.touch.OnTouchUp.AddListener(OnTap);
        }

        private void OnTap(Touch touch)
        {
            GameObject hitTrans = MobileInput.instance.CastRayHitFromCam(touch.position, Camera.main);

            if (hitTrans != null && hitTrans.gameObject == gameObject)
                Debug.Log("THI OBJECT HITTET");

            dragObj = null;
        }

        private void OnDrag(Touch touch)
        {

            if (dragObj == null)
            {
                dragObj = MobileInput.instance.CastRayHitFromCam(touch.position, Camera.main);
            }

            if (dragObj != null && dragObj.gameObject == gameObject)
            {
                Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
            }
        }
    }
}