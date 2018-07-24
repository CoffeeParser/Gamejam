using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class GameState : MonoBehaviour {

    public TextAsset InputFileTextAsset;

    public World CurrentWorld;
    public Person CurrentPerson;

    // Use this for initialization
    void Awake()
    {
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
            Debug.Log($"Selected new Level / Person with name {CurrentPerson.Name}");
            return true;
        }
        return false;
    }
}
