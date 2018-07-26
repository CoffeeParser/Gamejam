using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MobileSensors;


public delegate void ActionComplete(EvilAction action);
public class ObjectTrigger : MonoBehaviour, InteractionTrigger {
    public string EvilActionIdentifier;
    public EvilAction actionTrigger;
    public float triggerCompleteTime;
    public Animation anim;
    public bool isSolved;
    public bool isTriggered;
    public float holdingTime;
    public float holdingTimeThreshold;
    public GameObject actionObject;

    private void Start()
    {
        anim = GetComponent<Animation>();
    }

    public void ActionCompleted()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator TriggerAction(ActionComplete CompleteCallback)
    {
        if (!isTriggered)
        {
            isTriggered = true;
            yield return new WaitForSeconds(triggerCompleteTime);
            CompleteCallback(actionTrigger);
        }
    }

    public void StopAction()
    {
        anim.Stop();
    }
}

