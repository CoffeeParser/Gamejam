using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScreenViewHandler : MonoBehaviour {

    public GameObject StartScreen;
    public Button EnterMapMenu;
    public GameObject MapViewGameObject;
    public GameObject EvilScreenGameObject;
    public GameObject PatientScreenGameObject;

    public GameObject DialogField;
    public Text dialogFieldtext;

    private GameState _gameState;

    public Button SkipDialogBtn;




    void Awake () {
        _gameState = GameObject.FindGameObjectWithTag("GlobalLifeTime").GetComponent<GameState>();

        StartScreen.SetActive(true);
        EnterMapMenu.onClick.AddListener(() =>
        {
            MapViewGameObject.SetActive(true);
            StartScreen.SetActive(false);
        });

        SkipDialogBtn.onClick.AddListener(SkipEvilDialogScreen);
    }

    private void SkipEvilDialogScreen()
    {
        EvilScreenGameObject.SetActive(false);
        PatientScreenGameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update () {
		
	}

    void PersonChanged()
    {
        MapViewGameObject.SetActive(false);
        EvilScreenGameObject.SetActive(true);
        DialogField.SetActive(true);
        dialogFieldtext.text = _gameState.Begruessung;
    }

<<<<<<< HEAD
    public void SetPatientDialogText()
    {
        Debug.Log(_gameState.CurrentPerson.SolvedActions.Count);
        Debug.Log(_gameState.CurrentPerson.EvilAction.Count);


        UIController.MenuisActive = false;
        dialogIndex++;
        Debug.Log(dialogIndex);

        // How many Actions are solved
        int solvedActions = _gameState.CurrentPerson.SolvedActions.Count;
        // How many Actions are to do
        int actionsToDo = _gameState.CurrentPerson.EvilAction.Count;

        if(solvedActions == 0)
        {
            // Show TherapieStory again
            if (dialogIndex >= _gameState.CurrentPerson.TherapyStory.Count) // max
            {

                //hier ende, lade szene raum
                //Debug.Log("End");
                NightScreen.SetActive(true);
                DialogField.SetActive(false);
                PatientScreenGameObject.SetActive(false);
                StartCoroutine(LoadAsynchonusly(LoadLevelString));

                dialogIndex = -1;
                return;
            }
            else
            {
                dialogFieldtext.text = (_gameState.CurrentPerson.TherapyStory[dialogIndex].Message);
            }

        }
        else if (solvedActions > 0 && solvedActions < actionsToDo)
        {
            // show Review with part of
        }
        else if (solvedActions == actionsToDo)
        {
            // Show compledet Review 
            // Show ArchivementScreen
        }


 



    }

    
    public void SkipNightScreen()
    {
        Debug.Log("SkipNiht");
        NightScreen.SetActive(false);

        // Set MainMenu Activity false
        UIController.MenuisActive = false;
        // Load Level And wait until
        //StartCoroutine(LoadAsynchonusly(LoadLevelString));

    }
    

    public IEnumerator LoadAsynchonusly(string sceneString)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneString, LoadSceneMode.Additive);

        Debug.Log("LoadLevel");

        NightScreen.SetActive(true);

        while (!operation.isDone)
        {
            Debug.Log(".");
            SkipNightScreenBtn.enabled = false;
            yield return null;
        }
        SkipNightScreenBtn.enabled = true;
        //Debug.Log("LevelLoad is finish");
        //NightScreen.SetActive(false);
        //LevelAccomplishTest.instance.onLevelIsOver.AddListener(isLevelAccomplished);
    }

    public void isLevelAccomplished(bool isAccomplished)
    {
        SceneManager.UnloadSceneAsync(LoadLevelString);
        Debug.Log("LevelUnloaded");
        // UnLoadlevel after ever Session

        // Get Count of solved information 


        // Obsolet maybe for alternativ loadScreen
        if (isAccomplished)
        {
            // Finish leave Screen, only skipable when Level is Unload      
            LeaveFinishScreen.SetActive(true);
            LeaveFinishVBtn.onClick.AddListener(LoadSecondDialog);
        }
        else if (!isAccomplished)
        {
            // Unfinsih LeaveScreen Only Skipable wenn Level is unloaded 
            LeaveUnFinishScreen.SetActive(true);
            LeaveUnFinishVBtn.onClick.AddListener(LoadSecondDialog);
        }
        else if (1==2)
        {
            // When parts accoplished load House leave Screen, only skipable when Level is Unload
            // Get Count of solved information 
        }
    }


    public void LoadSecondDialog()
    {
        // Wenn nichts gelöst TherapiheStroy von vorne 
        

        // Wenn teile gelöst dann Review Story anhand der:
        // Solved actions

        //  Wenn alle Gelöst Solved actins count = Actions to do Count  
        //    EinweisungsScreen 
        // Dann minimap (muss dann umgeschaltet sein)


        LeaveFinishScreen.SetActive(false);
        LeaveUnFinishScreen.SetActive(false);
        PersonChanged();
=======
    public void SetDialogText()
    {
        
>>>>>>> 65916427a8f2de1ce74f08720ec8c2245d34e3ff
    }
}
