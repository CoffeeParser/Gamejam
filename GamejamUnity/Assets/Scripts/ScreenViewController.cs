using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScreenViewController : MonoBehaviour {

    
    private GameState _gamestate;

    public GameObject textfieldGO;

    private Sprite backGround;

    private Text dialogtext;
    private int dialogIndex;

    public Button skipDialog;

    private void Start()
    {      
        var go = GameObject.FindGameObjectWithTag("GlobalLifeTime");
        _gamestate = go.GetComponent<GameState>();
        _gamestate.SelectNewPerson(_gamestate.CurrentWorld.Persons[0]); // temp, remove me later
        dialogtext = textfieldGO.GetComponent<Text>();

        skipDialog.onClick.AddListener(SkipDialogMessage);


        dialogIndex = 0;
        setText();
    }

    private void ChangeBackGround()
    {

    }

    private void SkipDialogMessage()
    {
        dialogIndex++;


        if (dialogIndex >= _gamestate.CurrentPerson.TherapyStory.Count) // max
        {

            // hier ende, lade szene raum
            return;
        }

        Debug.Log(dialogIndex);
        setText();

    }

    public void setText()
    {


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
