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
    public void AccomplishActualLevel()
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
    }
}
