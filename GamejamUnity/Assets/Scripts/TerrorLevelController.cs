using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MobileSensors;



public class TerrorLevelController : MonoBehaviour
{

    private TouchInput touchInput;
    private Mic micInput;
    private Accelerate accel;
    private Camera gyroCam;
    private List<EvilAction> voiceActions;

    private void Start()
    {
        touchInput = MobileInput.instance.touch;
        micInput = MobileInput.instance.mic;
        accel = MobileInput.instance.accel;

        touchInput.OnTapPositionUp.AddListener(OnUserTouch);
        accel.OnShake.AddListener(OnUserShake);
        micInput.OnThresholdExceeded.AddListener(OnUserVoiceThresholdExceeded);
        touchInput.OnMove.AddListener(OnUserSwipe);

        gyroCam = FindObjectOfType<GyroCamera>().GetComponent<Camera>();
    }

    private void Update()
    {

    }

    void OnUserVoiceThresholdExceeded(string thresholdName)
    {
        foreach (EvilAction voiceAction in voiceActions)
        {

        }
    }

    void OnUserSwipe(Touch touch)
    {
        GameObject hitObj = MobileInput.instance.CastRayHitFromCam(touch.position, gyroCam);
        if(TriggerInteraction(hitObj))
        {
            Rigidbody hitRig = hitObj.GetComponent<Rigidbody>();

            if (hitRig != null)
            {
                hitRig.AddForce((gyroCam.transform.up * touch.deltaPosition.y + gyroCam.transform.right * touch.deltaPosition.x) / 0.5f, ForceMode.Impulse);
            }
        }
    }

    void OnUserShake(string shakeThreshold)
    {
        Vector2 shakeLoc = gyroCam.pixelRect.center;
        GameObject hitObj = MobileInput.instance.CastRayHitFromCam(shakeLoc, gyroCam);
        TriggerInteraction(hitObj);
    }

    void OnUserTouch(Vector2 tapPostiion)
    {
        GameObject hitObj = MobileInput.instance.CastRayHitFromCam(tapPostiion, gyroCam);
        TriggerInteraction(hitObj);
    }

    bool TriggerInteraction(GameObject hitObj)
    {
        if (hitObj != null)
        {
            InteractionTrigger objTrigger = hitObj.GetComponent<ObjectTrigger>();

            if (objTrigger != null)
            {
                objTrigger.TriggerAction(CompleteAction);
                return true;
            }
        }
        return false;
    }

    void CompleteAction(EvilAction action)
    {
        Debug.Log("ACTION SOLVED: " + action.ActionType + GameState.instance.SolveAction(action));
    }
}

interface InteractionTrigger
{
    void TriggerAction(ActionComplete action);
    void ActionCompleted();
}

