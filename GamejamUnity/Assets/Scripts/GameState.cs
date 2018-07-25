using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class GameState : MonoBehaviour {

    public static GameState instance;

    public TextAsset InputFileTextAsset;
    public World CurrentWorld;
    public Person CurrentPerson;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        var rootObject = JsonConvert.DeserializeObject<RootObject>(InputFileTextAsset.text);
        CurrentWorld = rootObject.Worlds[0];
    }

    /// <summary>
    /// Finished the CurrentPerson and Unlocks the next Person in the CurrentWorld
    /// </summary>
    public void FinishPerson()
    {
        CurrentPerson.Finished = true;
        int currentIndex = CurrentWorld.Persons.IndexOf(CurrentPerson);
        if(currentIndex < CurrentWorld.Persons.Count)
        {
            CurrentWorld.Persons[currentIndex++].Unlocked = true;
        }
    }

    public bool SelectNewPerson(Person newPerson)
    {
        if (newPerson.Unlocked && !newPerson.Finished && newPerson != CurrentPerson)
        {
            CurrentPerson = newPerson;
            Debug.Log($"newPersonSelected name{CurrentPerson.Name}");
            BroadcastMessage("PersonChanged"); // Receiver: ScreenViewHandler.cs
            return true;
        }
        return false;
    }

    public bool SolveAction(EvilAction action)
    {
        if (CurrentPerson.EvilAction.Contains(action))
        {
            CurrentPerson.SolvedActions.Add(action);
            CurrentPerson.EvilAction.Remove(action);
            return true;
        }
        return false;
        //bool actionfound = false;
        //EvilAction evilAction = null;
        //foreach(var action in CurrentPerson.EvilAction)
        //{
        //    if (action.ActionType.Equals(action))
        //    {
        //        evilAction = action;
        //        actionfound = true;
        //        break;
        //    }
        //}
        //if (actionfound)
        //{
        //    CurrentPerson.SolvedActions.Add(evilAction);
        //    CurrentPerson.EvilAction.Remove(evilAction);
        //    return true;
        //}
        //return false;
        //if(CurrentPerson.EvilAction.Contains(EvilAction))
    }
}
