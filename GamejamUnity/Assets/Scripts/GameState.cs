using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class GameState : MonoBehaviour {

    public static GameState instance;

    public TextAsset InputFileTextAsset;
    public World rawData;
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
        rawData = rootObject.Worlds[0];
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
            currentIndex++;
            CurrentWorld.Persons[currentIndex].Unlocked = true;
        }
    }

    public bool SelectAnLevel(Person newPerson)
    {
        if (newPerson.Unlocked)
        {
            CurrentPerson = newPerson;
            Debug.Log($"newPersonSelected name{CurrentPerson.Name}");
            BroadcastMessage("LevelStarted"); // Receiver: ScreenViewHandler.cs
            return true;
        }
        return false;
    }

    public void ResetLevel(Person person)
    {
        int index = CurrentWorld.Persons.IndexOf(person);
        person = rawData.Persons[index];
        foreach (EvilAction action in person.SolvedActions) {
            person.EvilAction.Add(action);
        }
        person.SolvedActions.Clear();
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
