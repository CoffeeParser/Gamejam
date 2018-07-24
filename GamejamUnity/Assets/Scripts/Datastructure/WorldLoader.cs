using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class WorldLoader : MonoBehaviour {
    public TextAsset InputFileTextAsset;
    // Use this for initialization
    void Start () {
	    var testObj = JsonConvert.DeserializeObject<RootObject>(InputFileTextAsset.text);
	    var story = testObj.Worlds[0].Persons[0].TherapyStory[0];
        var ersteWelt = testObj.Worlds[0];
        List<Person> personenAusErsterWelt = testObj.Worlds[0].Persons;
        if (story.DialogeType == "Story")
	    {
	        Debug.Log("zeige nur story an");
	    }
	    Debug.Log(testObj);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
