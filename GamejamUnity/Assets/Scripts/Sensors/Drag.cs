using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MobileSensors;
using UnityEngine.SceneManagement;


public class Drag : MonoBehaviour
{

    Transform dragObj;

    // Use this for initialization
    void Start()
    {
        MobileInput.inst.touch.OnDrag.AddListener(OnDrag);
        MobileInput.inst.touch.OnTouchUp.AddListener(OnTap);
    }

    private void OnTap(Touch touch)
    {
        Transform hitTrans = MobileInput.inst.CastRayHitFromCam(touch.position, 8);

        if (hitTrans != null && hitTrans.gameObject == gameObject)
            Debug.Log("THI OBJECT HITTET");

        dragObj = null;
    }

    private void OnDrag(Touch touch)
    {

        if (dragObj == null)
        {
            dragObj = MobileInput.inst.CastRayHitFromCam(touch.position, 8);
        }

        if (dragObj != null && dragObj.gameObject == gameObject)
        {
            Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
        }
    }
}
