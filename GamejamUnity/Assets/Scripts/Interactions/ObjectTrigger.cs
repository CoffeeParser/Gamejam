using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MobileSensors;


public delegate void ActionComplete(EvilAction action);

public class ObjectTrigger : MonoBehaviour, InteractionTrigger {

    public int evilActionID;
    public EvilAction actionTrigger;

    private void Start()
    {
        actionTrigger = GameState.instance.CurrentPerson.EvilAction[evilActionID];
    }

    public void ActionCompleted()
    {
        throw new System.NotImplementedException();
    }

    public void DoAction(ActionComplete actionComplete)
    {
        actionComplete(actionTrigger);
    }
}

