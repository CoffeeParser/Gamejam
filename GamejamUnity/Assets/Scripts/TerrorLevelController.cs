using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MobileSensors;



public class TerrorLevelController : MonoBehaviour {

    private TouchInput touchInput;
    private Mic micInput;

    private List<EvilAction> voiceActions;

    private void Start()
    {
        touchInput = MobileInput.instance.touch;
        micInput = MobileInput.instance.mic;

        touchInput.OnTapPositionUp.AddListener(OnUserTouch);
        micInput.OnThresholdExceeded.AddListener(OnUserVoiceThresholdExceeded);
    }

    private void Update()
    {
        
    }

    void OnUserVoiceThresholdExceeded(string thresholdName)
    {
        
    }

    void OnUserTouch(Vector2 tapPostiion)
    {
        GameObject hitObj = MobileInput.instance.CastRayHitFromCam(tapPostiion);

        if (hitObj != null)
        {
            ObjectTrigger objTrigger = hitObj.GetComponent<ObjectTrigger>();

            objTrigger.DoAction(CompleteAction);
        }
    }

    void CompleteAction(EvilAction action)
    {
        GameState.instance.SolveAction(action);
    }
}

interface InteractionTrigger
{
    void DoAction(ActionComplete action);
    void ActionCompleted();
}

