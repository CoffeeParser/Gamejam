using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScreenViewController : MonoBehaviour {

    private GameState _gamestate;
    public GameObject textfieldGO;
    private Text dialogtext;
    private int dialogIndex;

    public Button skipDialog;

    private void Start()
    {
        var go = GameObject.FindGameObjectWithTag("GlobalLifeTime");
        _gamestate = go.GetComponent<GameState>();
        _gamestate.SelectNewPerson(_gamestate.CurrentWorld.Persons[0]); // temp, remove me later
        dialogtext = textfieldGO.GetComponent<Text>(); 

        setText();
    }

  

    public void setText()
    {
        dialogIndex = 0;

        //List<TherapyStory> all_stories = _gamestate.CurrentPerson.TherapyStory;

        dialogtext.text = (_gamestate.CurrentPerson.TherapyStory[dialogIndex].Message);
        /*
        Debug.Log(_gamestate.CurrentPerson.TherapyStory[0]);

        foreach(var story in all_stories)
        {
            Debug.Log(story.Message);
        }
        */

    }

}
